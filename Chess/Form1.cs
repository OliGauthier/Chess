using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{

    
    public partial class Form1 : Form
    {
        public static int widthOfSquare = 80;
        public static Size sizeOfSquare = new Size(widthOfSquare, widthOfSquare);
        public static (int, int) currentPos = (-1, -1);
        public static (int,int) nextPos;
        public static bool inMove;
        public static MouseEventArgs previousMousePos = null;
        public static int widthOfHighlight = 4;
        public static List<(int, int)> highLightedSquares;
        public static string pieceToAdd=null;
        public static bool holdingAPiece = false;
        public static bool wasACapture = false;
        TaskCompletionSource<bool> task = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        /*
         Draws the board
             */
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graph = panel1.CreateGraphics();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {

                    graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[i, j].Color), new Rectangle(Program.mainBoard.Grid[i, j].Corner, sizeOfSquare));
                    //graph.DrawString(Program.mainBoard.Grid[i, j].Name, SystemFonts.DefaultFont, new SolidBrush(Color.Red), new Point(Program.mainBoard.Grid[i,j].Corner.X+sizeOfSquare/2, Program.mainBoard.Grid[i, j].Corner.Y + sizeOfSquare / 2));
                    //graph.DrawString($"{Program.mainBoard.Grid[i, j].File}&{Program.mainBoard.Grid[i, j].Rank}", SystemFonts.DefaultFont, new SolidBrush(Color.Red), new Point(Program.mainBoard.Grid[i, j].Corner.X + sizeOfSquare / 2, Program.mainBoard.Grid[i, j].Corner.Y + 3 * sizeOfSquare / 4));
                    if (Program.mainBoard.Grid[i, j].Piece != null)
                        graph.DrawImage(Program.mainBoard.Grid[i, j].Piece.PieceImage, Program.mainBoard.Grid[i, j].PosOfImage);
                }
            }
            //graph.DrawImage(Program.blackB, new Point(100, 100));

        }

        /*
         Draws the letters on bottom of board
             */
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics graph = panel2.CreateGraphics();
            for (int i = 0; i < 8; i++)
                graph.DrawString(Convert.ToString(Convert.ToChar(97 + i)), SystemFonts.DefaultFont, new SolidBrush(Color.Black), new Point(i * widthOfSquare + widthOfSquare / 2, 0));
        }

        /*
         Draws the numbers on left side of board
             */
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

            Graphics graph = panel3.CreateGraphics();
            for (int i = 0; i < 8; i++)
                graph.DrawString($"{8 - i}", SystemFonts.DefaultFont, new SolidBrush(Color.Black), new Point(0, i * widthOfSquare + widthOfSquare / 2));
        }

        //raise a piece
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            currentPos = calculateRankAndFile(e.X, e.Y);
            if (Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece != null)
            {
                if (Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.White==Program.mainBoard.WhiteTurn)
                {
                    holdingAPiece = true;
                    Graphics graph = panel1.CreateGraphics();
                    this.Cursor = Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PieceCursor;
                    graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Color), new Rectangle(Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Corner, sizeOfSquare));
                    Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.CalculateAvaibleMoves(Program.mainBoard);
                    if (Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PossibleMoves.Count > 0)
                    {
                        foreach ((int, int) pos in Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PossibleMoves)
                        {
                            graph.DrawRectangle(new Pen(Color.Red, widthOfHighlight), new Rectangle(Program.mainBoard.Grid[pos.Item1, pos.Item2].Corner.X + widthOfHighlight / 2, Program.mainBoard.Grid[pos.Item1, pos.Item2].Corner.Y + widthOfHighlight / 2, widthOfSquare - widthOfHighlight, widthOfSquare - widthOfHighlight));
                        }
                    }
                    highLightedSquares = Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PossibleMoves;
                }
            }
            
            inMove = true;
        }

        //drop a piece
        private async void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Graphics graph = panel1.CreateGraphics();
            this.Cursor = Cursors.Default;
            (int, int) nextPos = calculateRankAndFile(e.X, e.Y);
            if (currentPos.Item1 != -1 & currentPos.Item2 != -1)
            { 
                //if you took a piece
                if (holdingAPiece)
                {
                    //if the square the piece is dropped in is a legal one 
                    //TO DO : also check the legality of the move with respect to checks and all that
                    if (Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PossibleMoves.Contains((nextPos.Item1, nextPos.Item2)))
                    {
                        //previous enpassant move is no longer possible
                        string previousEnpassant = Program.mainBoard.EnPassantPossible;
                        Program.mainBoard.EnPassantPossible = null;

                        //previous wasACapture no longer considered
                        wasACapture = false;

                        //if its a white pawn reaching the last rank
                        if (Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PieceType == "P" & nextPos.Item1 == 7)
                        {
                            if (nextPos.Item2 < 7)
                                panel4.Location = new Point(Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Corner.X, 0);
                            else//other wise it goes out of the board
                                panel4.Location = new Point(Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2 - 1].Corner.X, 0);
                            panel4.Visible = true;
                            pieceToAdd = null;
                            task = new TaskCompletionSource<bool>();
                            await task.Task;
                            panel4.Visible = false;
                            if (pieceToAdd != null)
                            {
                                Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece = new Piece(pieceToAdd, nextPos);
                            }
                        }
                        //if its a black pawn reaching the last rank
                        else if (Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PieceType == "p" & nextPos.Item1 == 0)
                        {
                            if (nextPos.Item2 < 7)
                                panel5.Location = new Point(Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Corner.X, 6 * widthOfSquare);
                            else
                                panel5.Location = new Point(Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2 - 1].Corner.X, 6 * widthOfSquare);
                            panel5.Visible = true;
                            pieceToAdd = null;
                            task = new TaskCompletionSource<bool>();
                            await task.Task;
                            panel5.Visible = false;
                            if (pieceToAdd != null)
                            {
                                Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece = new Piece(pieceToAdd, nextPos);
                            }
                        }
                        //if it'S a white pawn moving two forward, en passant is possible
                        else if (Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PieceType == "P" & nextPos.Item1 == 3 & currentPos.Item1 == 1)
                        {
                            Program.mainBoard.EnPassantPossible = Program.mainBoard.Grid[2, nextPos.Item2].Name;
                            Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece = Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece;
                            Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece.Position = nextPos;
                            Program.mainBoard.EnPassantPossible = Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2].Name;
                        }
                        //if it'S a black pawn moving two forward, en passant is possible
                        else if (Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PieceType == "p" & nextPos.Item1 == 4 & currentPos.Item1 == 6)
                        {
                            Program.mainBoard.EnPassantPossible = Program.mainBoard.Grid[5, nextPos.Item2].Name;
                            Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece = Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece;
                            Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece.Position = nextPos;
                            Program.mainBoard.EnPassantPossible = Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2].Name;
                        }
                        //castling
                        else if ((Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PieceType == "K" || Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PieceType == "k") & (nextPos.Item2 == (currentPos.Item2 + 2) || nextPos.Item2 == (currentPos.Item2 - 2)))
                        {
                            //king side castle
                            if (nextPos.Item2 == (currentPos.Item2 + 2))
                            {
                                Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2 + 1].Piece = Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2 + 3].Piece;
                                Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2 + 1].Piece.Position = (currentPos.Item1, currentPos.Item2 + 1);
                                graph.DrawImage(Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2 + 1].Piece.PieceImage, Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2 + 1].PosOfImage);
                                Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2 + 3].Piece = null;
                                graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2 + 3].Color), new Rectangle(Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2 + 3].Corner, sizeOfSquare));
                            }
                            // queen side castle
                            else if (nextPos.Item2 == (currentPos.Item2 + -2))
                            {
                                Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2 - 1].Piece = Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2 - 4].Piece;
                                Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2 - 1].Piece.Position = (currentPos.Item1, currentPos.Item2 - 1);
                                graph.DrawImage(Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2 - 1].Piece.PieceImage, Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2 - 1].PosOfImage);
                                Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2 - 4].Piece = null;
                                graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2 - 4].Color), new Rectangle(Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2 - 4].Corner, sizeOfSquare));
                            }
                            //redraw the king
                            Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece = Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece;
                            Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece.Position = nextPos;
                        }
                        else //any other sort of move
                        {
                            if (Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece != null & Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece != Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece)
                                wasACapture = true;
                            Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece = Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece;
                            Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece.Position = nextPos;
                        }


                        //it was a white pawn taking en passant
                        if (Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PieceType == "P" & Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Name == previousEnpassant)
                        {
                            wasACapture = true;
                            Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2].Piece = null;
                            graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2].Color), new Rectangle(Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2].Corner, sizeOfSquare));
                        }
                        //it was a black pawn taking en passant
                        else if (Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PieceType == "p" & Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Name == previousEnpassant)
                        {
                            wasACapture = true;
                            Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2].Piece = null;
                            graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2].Color), new Rectangle(Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2].Corner, sizeOfSquare));
                        }

                        //removes the possibility of castling
                        if (Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece.PieceType == "K")
                        {
                            Program.mainBoard.WhiteCanCastleK = false;
                            Program.mainBoard.WhiteCanCastleQ = false;
                        }
                        else if (Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece.PieceType == "k")
                        {
                            Program.mainBoard.BlackCanCastleK = false;
                            Program.mainBoard.BlackCanCastleQ = false;
                        }
                        else if (Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece.PieceType == "R")
                        {
                            if (currentPos == (0, 0))
                                Program.mainBoard.WhiteCanCastleQ = false;
                            else if (currentPos == (0, 7))
                                Program.mainBoard.WhiteCanCastleK = false;
                        }
                        else if (Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece.PieceType == "r")
                        {
                            if (currentPos == (7, 0))
                                Program.mainBoard.BlackCanCastleQ = false;
                            else if (currentPos == (7, 7))
                                Program.mainBoard.BlackCanCastleK = false;
                        }
                        //redraw the destination square in case there was a piece aka a capture
                        if (Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece != null)
                            graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Color), new Rectangle(Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Corner, sizeOfSquare));
                        
                        //draw piece on new square
                        graph.DrawImage(Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece.PieceImage, Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].PosOfImage);

                        //deals withthe increment of the counter for the 50 move rule
                        if (Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PieceType == "p" || Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PieceType == "P" || wasACapture)
                        {
                            Program.mainBoard.MoveCounter_50rule = 0;
                        }
                        else
                            Program.mainBoard.MoveCounter_50rule++;

                        if (currentPos.Item1 != nextPos.Item1 || currentPos.Item2 != nextPos.Item2)
                            Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece = null;

                        //alternate the player turn
                        if (!Program.mainBoard.WhiteTurn)
                            Program.mainBoard.TurnCount++;
                        Program.mainBoard.WhiteTurn = !Program.mainBoard.WhiteTurn;
                        
                    }
                    //draw piece back on its square
                    else
                        graph.DrawImage(Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PieceImage, Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].PosOfImage);
                    //removes all hightlighted squares
                    try
                    {
                        if (highLightedSquares.Count > 0)
                        {
                            foreach ((int, int) pos in highLightedSquares)
                            {
                                graph.DrawRectangle(new Pen(Program.mainBoard.Grid[pos.Item1, pos.Item2].Color, widthOfHighlight), new Rectangle(Program.mainBoard.Grid[pos.Item1, pos.Item2].Corner.X + widthOfHighlight / 2, Program.mainBoard.Grid[pos.Item1, pos.Item2].Corner.Y + widthOfHighlight / 2, widthOfSquare - widthOfHighlight, widthOfSquare - widthOfHighlight));
                            }
                        }
                        highLightedSquares = null;
                    }
                    catch (System.NullReferenceException)
                    { //do nothing it's fine
                    }
                    holdingAPiece = false;
                }
            }
            inMove = false;
            textBox1.Text = Program.mainBoard.CreateFenFromBoard();
            previousMousePos = null;// static object used for the mouse move function. must then be reset to avoid fucking stuff with a past move
            currentPos = (-1, -1);
        }

       
        //gives the rank and file from two positions (cursor usually)
        private (int,int) calculateRankAndFile(int posX, int posY)
        {
            int file = posX / widthOfSquare;
            int rank = 7 - posY / widthOfSquare;
            return (rank, file);
        }
        //
        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text += textBox2.Text;
            textBox2.Text = "";
            
        }

        //panel used by white to promote
        private void panel4_MouseClick(object sender, MouseEventArgs e)
        {
            pieceToAdd=null;//default value which means we clicked outside the panel
            //click the queen
            if (e.X >= 0 & e.X < panel4.Width / 2 & e.Y >= 0 & e.Y < panel4.Height / 2)
                pieceToAdd = "Q";
            //click the knight
            else if (e.X >= panel4.Width / 20 & e.X < panel4.Width & e.Y >= 0 & e.Y < panel4.Height / 2)
                pieceToAdd = "N";
            //click the rook (just a queen with broken legs tbh)
            else if (e.X >= 0 & e.X < panel4.Width / 2 & e.Y >= panel4.Height / 2 & e.Y < panel4.Height)
                pieceToAdd = "R";
            //click the bishop (honestly why would someone do this except to troll)
            else if (e.X >= panel4.Width / 20 & e.X < panel4.Width & e.Y >= panel4.Height / 2 & e.Y < panel4.Height)
                pieceToAdd = "B";
            //click outside the panel to actually make a different move
           
            task?.TrySetResult(true);
        }
        //panel used by white to promote
        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            Graphics graph = panel4.CreateGraphics();
            graph.DrawImage(Program.whiteQ, (panel4.Width / 2 - Program.sizeOfPieces) / 2, (panel4.Height / 2 - Program.sizeOfPieces) / 2);
            graph.DrawImage(Program.whiteN, panel4.Width / 2 + (panel4.Width / 2 - Program.sizeOfPieces) / 2, (panel4.Height / 2 - Program.sizeOfPieces) / 2);
            graph.DrawImage(Program.whiteR, (panel4.Width / 2 - Program.sizeOfPieces) / 2, panel4.Height / 2 + (panel4.Height / 2 - Program.sizeOfPieces) / 2);
            graph.DrawImage(Program.whiteB, panel4.Width / 2 + (panel4.Width / 2 - Program.sizeOfPieces) / 2, panel4.Height / 2 + (panel4.Height / 2 - Program.sizeOfPieces) / 2);
        }
        //panel used by black to promote
        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            Graphics graph = panel5.CreateGraphics();
            graph.DrawImage(Program.blackQ, (panel5.Width / 2 - Program.sizeOfPieces) / 2, (panel5.Height / 2 - Program.sizeOfPieces) / 2);
            graph.DrawImage(Program.blackN, panel5.Width / 2 + (panel5.Width / 2 - Program.sizeOfPieces) / 2, (panel5.Height / 2 - Program.sizeOfPieces) / 2);
            graph.DrawImage(Program.blackR, (panel5.Width / 2 - Program.sizeOfPieces) / 2, panel5.Height / 2 + (panel5.Height / 2 - Program.sizeOfPieces) / 2);
            graph.DrawImage(Program.blackB, panel5.Width / 2 + (panel5.Width / 2 - Program.sizeOfPieces) / 2, panel5.Height / 2 + (panel5.Height / 2 - Program.sizeOfPieces) / 2);
        }
        //panel used by black to promote
        private void panel5_MouseClick(object sender, MouseEventArgs e)
        {
            pieceToAdd = null;//default value which means we clicked outside the panel
            //click the queen
            if (e.X >= 0 & e.X < panel5.Width / 2 & e.Y >= 0 & e.Y < panel5.Height / 2)
                pieceToAdd = "q";
            //click the knight
            else if (e.X >= panel5.Width / 20 & e.X < panel5.Width & e.Y >= 0 & e.Y < panel5.Height / 2)
                pieceToAdd = "n";
            //click the rook (just a queen with broken legs tbh)
            else if (e.X >= 0 & e.X < panel5.Width / 2 & e.Y >= panel5.Height / 2 & e.Y < panel5.Height)
                pieceToAdd = "r";
            //click the bishop (honestly why would someone do this except to troll)
            else if (e.X >= panel5.Width / 20 & e.X < panel5.Width & e.Y >= panel5.Height / 2 & e.Y < panel5.Height)
                pieceToAdd = "b";
            //click outside the panel to actually make a different move

            task?.TrySetResult(true);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            //bullshit to get around the fact that the mouse click methods don't fire for panels if you don't click in them (duh)
            if(panel4.Visible)
                task?.TrySetResult(true);
            else if(panel5.Visible)
                task?.TrySetResult(true);
        }
    }
}
