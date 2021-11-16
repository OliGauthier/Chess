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
            currentPos = calculateRankAndFile(e.X, e.Y);
            if (Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece != null)
            {
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
            
            inMove = true;

        }


        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Graphics graph = panel1.CreateGraphics();
            this.Cursor = Cursors.Default;
            (int, int) nextPos = calculateRankAndFile(e.X, e.Y);
            if (currentPos.Item1 != -1 & currentPos.Item2 != -1)
            { 
                if (Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece != null)
                {
                    graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Color), new Rectangle(Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Corner, sizeOfSquare));
                    graph.DrawImage(Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece.PieceImage, Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].PosOfImage);

                    Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece = Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece;
                    Program.mainBoard.Grid[nextPos.Item1, nextPos.Item2].Piece.Position = nextPos;
                    if (currentPos.Item1!=nextPos.Item1 || currentPos.Item2!=nextPos.Item2)
                        Program.mainBoard.Grid[currentPos.Item1, currentPos.Item2].Piece = null;
                    if (highLightedSquares.Count > 0)
                    {
                        foreach ((int, int) pos in highLightedSquares)
                        {
                            graph.DrawRectangle(new Pen(Program.mainBoard.Grid[pos.Item1, pos.Item2].Color, widthOfHighlight), new Rectangle(Program.mainBoard.Grid[pos.Item1, pos.Item2].Corner.X + widthOfHighlight / 2, Program.mainBoard.Grid[pos.Item1, pos.Item2].Corner.Y + widthOfHighlight / 2, widthOfSquare- widthOfHighlight,widthOfSquare-widthOfHighlight));
                        }
                    }
                    highLightedSquares = null;
                }
            }
            inMove = false;
            textBox1.Text = Program.mainBoard.CreateFenFromBoard();
            previousMousePos = null;// static object used for the mouse move function. must then be reset to avoid fucking stuff with a past move
            currentPos = (-1, -1);
        }

       

        private (int,int) calculateRankAndFile(int posX, int posY)
        {
            int file = posX / 80;
            int rank = 7 - posY / 80;
            return (rank, file);
        }

        

        

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text += textBox2.Text;
            textBox2.Text = "";
            
        }

        
    }
}
