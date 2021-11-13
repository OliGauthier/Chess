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
        public static int compteurDeMerde = 0;


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


        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Graphics graph = panel1.CreateGraphics();

            Point posImg = calculatePosOfImage(e.X, e.Y);
            
            (int, int) rankAndFile = calculateRankAndFile(e.X, e.Y);
            if (Program.mainBoard.Grid[rankAndFile.Item1, rankAndFile.Item2].Piece != null)
            {
                graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[rankAndFile.Item1, rankAndFile.Item2].Color), new Rectangle(Program.mainBoard.Grid[rankAndFile.Item1, rankAndFile.Item2].Corner, sizeOfSquare));
                currentPos = rankAndFile;
                graph.DrawImage(Program.mainBoard.Grid[rankAndFile.Item1, rankAndFile.Item2].Piece.PieceImage, Program.mainBoard.Grid[rankAndFile.Item1, rankAndFile.Item2].PosOfImage);

            }
            
            inMove = true;

        }


        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Graphics graph = panel1.CreateGraphics();

            (int, int) nextPos = calculateRankAndFile(e.X, e.Y);
            if (currentPos.Item1 != -1 & currentPos.Item2 != -1)
            { 
                if (Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece != null)
                {
                    graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Color), new Rectangle(Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Corner, sizeOfSquare));
                    graph.DrawImage(Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PieceImage, Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].PosOfImage);

                    Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece = Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece;
                    if(currentPos.Item1!=nextPos.Item1 || currentPos.Item2!=nextPos.Item2)
                        Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece = null;


                    //depending on where the piece is dropped in the square we may have to redraw other squares around it
                    if (e.X % widthOfSquare > (widthOfSquare - Program.sizeOfPieces / 2) & nextPos.Item2 < 7)//need to redraw right square
                    {
                        graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2 + 1].Color), new Rectangle(Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2 + 1].Corner, sizeOfSquare));
                        if (Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2 + 1].Piece != null & (nextPos.Item1 != currentPos.Item1 || nextPos.Item2 != currentPos.Item2))
                            graph.DrawImage(Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2 + 1].Piece.PieceImage, Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2 + 1].PosOfImage);
                    }
                    if (e.X % widthOfSquare < Program.sizeOfPieces / 2 & nextPos.Item2 > 0)//need to redraw left square
                    {
                        graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2 - 1].Color), new Rectangle(Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2 - 1].Corner, sizeOfSquare));
                        if (Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2 - 1].Piece != null & (nextPos.Item1 != currentPos.Item1 || nextPos.Item2 != currentPos.Item2))
                            graph.DrawImage(Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2 - 1].Piece.PieceImage, Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2 - 1].PosOfImage);
                    }
                    if (e.Y % widthOfSquare > (widthOfSquare - Program.sizeOfPieces / 2) & nextPos.Item1 > 0)//need to redraw bottom square
                    {
                        graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2].Color), new Rectangle(Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2].Corner, sizeOfSquare));
                        if (Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2].Piece != null & (nextPos.Item1 != currentPos.Item1 || nextPos.Item2 != currentPos.Item2))
                            graph.DrawImage(Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2].Piece.PieceImage, Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2].PosOfImage);
                    }
                    if (e.Y % widthOfSquare < Program.sizeOfPieces / 2 & nextPos.Item1 < 7)//need to redraw top square
                    {
                        graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2].Color), new Rectangle(Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2].Corner, sizeOfSquare));
                        if (Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2].Piece != null & (nextPos.Item1 != currentPos.Item1 || nextPos.Item2 != currentPos.Item2))
                            graph.DrawImage(Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2].Piece.PieceImage, Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2].PosOfImage);
                    }
                    if ((e.X % widthOfSquare > (widthOfSquare - Program.sizeOfPieces / 2) & nextPos.Item2 < 7) & (e.Y % widthOfSquare > (widthOfSquare - Program.sizeOfPieces / 2) & nextPos.Item1 > 0) & (nextPos.Item1 > 0 & nextPos.Item2 < 7))//need to redraw bottom right square
                    {
                        graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2 + 1].Color), new Rectangle(Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2 + 1].Corner, sizeOfSquare));
                        if (Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2 + 1].Piece != null & (nextPos.Item1 != currentPos.Item1 || nextPos.Item2 != currentPos.Item2))
                            graph.DrawImage(Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2 + 1].Piece.PieceImage, Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2 + 1].PosOfImage);
                    }
                    if ((e.X % widthOfSquare < Program.sizeOfPieces / 2 & nextPos.Item2 > 0) & (e.Y % widthOfSquare > (widthOfSquare - Program.sizeOfPieces / 2) & nextPos.Item1 > 0) & (nextPos.Item1 > 0 & nextPos.Item2 > 0))//need to redraw bottom left square
                    {
                        graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2 - 1].Color), new Rectangle(Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2 - 1].Corner, sizeOfSquare));
                        if (Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2 - 1].Piece != null & (nextPos.Item1 != currentPos.Item1 || nextPos.Item2 != currentPos.Item2))
                            graph.DrawImage(Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2 - 1].Piece.PieceImage, Program.mainBoard.Grid[nextPos.Item1 - 1, nextPos.Item2 - 1].PosOfImage);
                    }
                    if ((e.X % widthOfSquare > (widthOfSquare - Program.sizeOfPieces / 2) & nextPos.Item2 < 7) & (e.Y % widthOfSquare < Program.sizeOfPieces / 2 & nextPos.Item1 < 7) & (nextPos.Item1 < 7 & nextPos.Item2 < 7))//need to redraw top right square
                    {
                        graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2 + 1].Color), new Rectangle(Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2 + 1].Corner, sizeOfSquare));
                        if (Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2 + 1].Piece != null & (nextPos.Item1 != currentPos.Item1 || nextPos.Item2 != currentPos.Item2))
                            graph.DrawImage(Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2 + 1].Piece.PieceImage, Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2 + 1].PosOfImage);
                    }
                    if ((e.X % widthOfSquare < Program.sizeOfPieces / 2 & nextPos.Item2 > 0) & (e.Y % widthOfSquare < Program.sizeOfPieces / 2 & nextPos.Item1 > 0) & (nextPos.Item1 < 7 & nextPos.Item2 > 0))//need to redraw top left square
                    {
                        graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2 - 1].Color), new Rectangle(Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2 - 1].Corner, sizeOfSquare));
                        if (Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2 - 1].Piece != null & (nextPos.Item1 != currentPos.Item1 || nextPos.Item2 != currentPos.Item2))
                            graph.DrawImage(Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2 - 1].Piece.PieceImage, Program.mainBoard.Grid[nextPos.Item1 + 1, nextPos.Item2 - 1].PosOfImage);
                    }
                }
            }
            inMove = false;
            textBox1.Text = Program.mainBoard.CreateFenFromBoard();
            previousMousePos = null;// static object used for the mouse move function. must then be reset to avoid fucking stuff with a past move
            currentPos = (-1, -1);
        }

        private Point calculatePosOfImage(int posX, int posY)
        {
            (int,int) pos = calculateRankAndFile(posX, posY);
            return Program.mainBoard.Grid[pos.Item1, pos.Item2].PosOfImage;
        }

        private (int,int) calculateRankAndFile(int posX, int posY)
        {
            int file = posX / 80;
            int rank = 7 - posY / 80;
            return (rank, file);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X>0 & e.Y>0 & e.X<8*widthOfSquare & e.Y<8*widthOfSquare)
            { Graphics graph = panel1.CreateGraphics();

                if (inMove)
                {
                    if (previousMousePos != null)
                    {
                        //deals with the squares the cursor goes through but not the squares that overlap with the piece image
                        (int, int) posCursor = calculateRankAndFile(previousMousePos.X, previousMousePos.Y);
                        graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[posCursor.Item1, posCursor.Item2].Color), new Rectangle(Program.mainBoard.Grid[posCursor.Item1, posCursor.Item2].Corner, sizeOfSquare));
                        if (Program.mainBoard.Grid[posCursor.Item1, posCursor.Item2].Piece != null & (posCursor.Item1 != currentPos.Item1 & posCursor.Item2 != currentPos.Item2))
                            graph.DrawImage(Program.mainBoard.Grid[posCursor.Item1, posCursor.Item2].Piece.PieceImage, Program.mainBoard.Grid[posCursor.Item1, posCursor.Item2].PosOfImage);

                        
                        if (e.X % widthOfSquare > (widthOfSquare - Program.sizeOfPieces/2) & posCursor.Item2 < 7)//need to redraw right square
                        {
                            graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[posCursor.Item1, posCursor.Item2 + 1].Color), new Rectangle(Program.mainBoard.Grid[posCursor.Item1, posCursor.Item2 + 1].Corner, sizeOfSquare));
                            if (Program.mainBoard.Grid[posCursor.Item1, posCursor.Item2 + 1].Piece != null & (posCursor.Item1!=currentPos.Item1 || posCursor.Item2!=currentPos.Item2))
                                graph.DrawImage(Program.mainBoard.Grid[posCursor.Item1, posCursor.Item2 + 1].Piece.PieceImage, Program.mainBoard.Grid[posCursor.Item1, posCursor.Item2 + 1].PosOfImage);
                        }
                        if (e.X % widthOfSquare < Program.sizeOfPieces / 2 & posCursor.Item2 > 0)//need to redraw left square
                        {
                            graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[posCursor.Item1, posCursor.Item2 - 1].Color), new Rectangle(Program.mainBoard.Grid[posCursor.Item1, posCursor.Item2 - 1].Corner, sizeOfSquare));
                            if (Program.mainBoard.Grid[posCursor.Item1, posCursor.Item2 - 1].Piece != null & (posCursor.Item1 != currentPos.Item1 || posCursor.Item2 != currentPos.Item2))
                                graph.DrawImage(Program.mainBoard.Grid[posCursor.Item1, posCursor.Item2 - 1].Piece.PieceImage, Program.mainBoard.Grid[posCursor.Item1, posCursor.Item2 - 1].PosOfImage);
                        }
                        if (e.Y % widthOfSquare > (widthOfSquare - Program.sizeOfPieces/2) & posCursor.Item1 > 0)//need to redraw bottom square
                        {
                            graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[posCursor.Item1 - 1, posCursor.Item2].Color), new Rectangle(Program.mainBoard.Grid[posCursor.Item1 - 1, posCursor.Item2].Corner, sizeOfSquare));
                            if (Program.mainBoard.Grid[posCursor.Item1 - 1, posCursor.Item2].Piece != null & (posCursor.Item1 != currentPos.Item1 || posCursor.Item2 != currentPos.Item2))
                                graph.DrawImage(Program.mainBoard.Grid[posCursor.Item1 - 1, posCursor.Item2].Piece.PieceImage, Program.mainBoard.Grid[posCursor.Item1 - 1, posCursor.Item2].PosOfImage);
                        }
                        if (e.Y % widthOfSquare < Program.sizeOfPieces / 2 & posCursor.Item1 < 7)//need to redraw top square
                        {
                            graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[posCursor.Item1 + 1, posCursor.Item2].Color), new Rectangle(Program.mainBoard.Grid[posCursor.Item1 + 1, posCursor.Item2].Corner, sizeOfSquare));
                            if (Program.mainBoard.Grid[posCursor.Item1 + 1, posCursor.Item2].Piece != null & (posCursor.Item1 != currentPos.Item1 || posCursor.Item2 != currentPos.Item2))
                                graph.DrawImage(Program.mainBoard.Grid[posCursor.Item1 + 1, posCursor.Item2].Piece.PieceImage, Program.mainBoard.Grid[posCursor.Item1 + 1, posCursor.Item2].PosOfImage);
                        }
                        if ((e.X % widthOfSquare > (widthOfSquare - Program.sizeOfPieces/2) & posCursor.Item2 < 7) & (e.Y % widthOfSquare > (widthOfSquare - Program.sizeOfPieces/2) & posCursor.Item1 > 0) & (posCursor.Item1 > 0 & posCursor.Item2 < 7))//need to redraw bottom right square
                        {
                            graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[posCursor.Item1 - 1, posCursor.Item2 + 1].Color), new Rectangle(Program.mainBoard.Grid[posCursor.Item1 - 1, posCursor.Item2 + 1].Corner, sizeOfSquare));
                            if (Program.mainBoard.Grid[posCursor.Item1 - 1, posCursor.Item2 + 1].Piece != null & (posCursor.Item1 != currentPos.Item1 || posCursor.Item2 != currentPos.Item2))
                                graph.DrawImage(Program.mainBoard.Grid[posCursor.Item1 - 1, posCursor.Item2 + 1].Piece.PieceImage, Program.mainBoard.Grid[posCursor.Item1 - 1, posCursor.Item2 + 1].PosOfImage);
                        }
                        if ((e.X % widthOfSquare < Program.sizeOfPieces / 2 & posCursor.Item2 > 0) & (e.Y % widthOfSquare > (widthOfSquare - Program.sizeOfPieces / 2) & posCursor.Item1 > 0) & (posCursor.Item1 > 0 & posCursor.Item2 > 0))//need to redraw bottom left square
                        {
                            graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[posCursor.Item1 - 1, posCursor.Item2 - 1].Color), new Rectangle(Program.mainBoard.Grid[posCursor.Item1 - 1, posCursor.Item2 - 1].Corner, sizeOfSquare));
                            if (Program.mainBoard.Grid[posCursor.Item1 - 1, posCursor.Item2 - 1].Piece != null & (posCursor.Item1 != currentPos.Item1 || posCursor.Item2 != currentPos.Item2))
                                graph.DrawImage(Program.mainBoard.Grid[posCursor.Item1 - 1, posCursor.Item2 - 1].Piece.PieceImage, Program.mainBoard.Grid[posCursor.Item1 - 1, posCursor.Item2 - 1].PosOfImage);
                        }
                        if ((e.X % widthOfSquare > (widthOfSquare - Program.sizeOfPieces / 2) & posCursor.Item2 < 7) & (e.Y % widthOfSquare < Program.sizeOfPieces / 2 & posCursor.Item1 < 7) & (posCursor.Item1 < 7 & posCursor.Item2 < 7))//need to redraw top right square
                        {
                            graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[posCursor.Item1 + 1, posCursor.Item2 + 1].Color), new Rectangle(Program.mainBoard.Grid[posCursor.Item1 + 1, posCursor.Item2 + 1].Corner, sizeOfSquare));
                            if (Program.mainBoard.Grid[posCursor.Item1 + 1, posCursor.Item2 + 1].Piece != null & (posCursor.Item1 != currentPos.Item1 || posCursor.Item2 != currentPos.Item2))
                                graph.DrawImage(Program.mainBoard.Grid[posCursor.Item1 + 1, posCursor.Item2 + 1].Piece.PieceImage, Program.mainBoard.Grid[posCursor.Item1 + 1, posCursor.Item2 + 1].PosOfImage);
                        }
                        if ((e.X % widthOfSquare < Program.sizeOfPieces / 2 & posCursor.Item2 > 0) & (e.Y % widthOfSquare < Program.sizeOfPieces / 2 & posCursor.Item1 > 0) & (posCursor.Item1 < 7 & posCursor.Item2 > 0))//need to redraw top left square
                        {
                            graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[posCursor.Item1 + 1, posCursor.Item2 - 1].Color), new Rectangle(Program.mainBoard.Grid[posCursor.Item1 + 1, posCursor.Item2 - 1].Corner, sizeOfSquare));
                            if (Program.mainBoard.Grid[posCursor.Item1 + 1, posCursor.Item2 - 1].Piece != null & (posCursor.Item1 != currentPos.Item1 || posCursor.Item2 != currentPos.Item2))
                                graph.DrawImage(Program.mainBoard.Grid[posCursor.Item1 + 1, posCursor.Item2 - 1].Piece.PieceImage, Program.mainBoard.Grid[posCursor.Item1 + 1, posCursor.Item2 - 1].PosOfImage);
                        }

                    }
                    if (currentPos.Item1 != -1)
                        if (Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece != null)
                            graph.DrawImage(Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PieceImage, new Point(e.X-Program.sizeOfPieces/2, e.Y- Program.sizeOfPieces / 2));
                    previousMousePos = e;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Show();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Show();
        }
    }
}
