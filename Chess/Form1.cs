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
        public static int sizeOfSquare = 80;

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

                    graph.FillRectangle(new SolidBrush(Program.mainBoard.Grid[i, j].Color), Program.mainBoard.Grid[i, j].Corner.X, Program.mainBoard.Grid[i, j].Corner.Y, sizeOfSquare, sizeOfSquare);
                    //graph.DrawString(Program.mainBoard.Grid[i, j].Name, SystemFonts.DefaultFont, new SolidBrush(Color.Red), new Point(Program.mainBoard.Grid[i,j].Corner.X+sizeOfSquare/2, Program.mainBoard.Grid[i, j].Corner.Y + sizeOfSquare / 2));
                    //graph.DrawString($"{Program.mainBoard.Grid[i, j].File}&{Program.mainBoard.Grid[i, j].Rank}", SystemFonts.DefaultFont, new SolidBrush(Color.Red), new Point(Program.mainBoard.Grid[i, j].Corner.X + sizeOfSquare / 2, Program.mainBoard.Grid[i, j].Corner.Y + 3 * sizeOfSquare / 4));
                    if(Program.mainBoard.Grid[i,j].Piece!=null)
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
                graph.DrawString(Convert.ToString(Convert.ToChar(97+i)), SystemFonts.DefaultFont, new SolidBrush(Color.Black), new Point(i* sizeOfSquare + sizeOfSquare/2, 0));
        }

        /*
         Draws the numbers on left side of board
             */
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

            Graphics graph = panel3.CreateGraphics();
            for (int i = 0; i < 8; i++)
                graph.DrawString($"{8-i}", SystemFonts.DefaultFont, new SolidBrush(Color.Black), new Point(0,i * sizeOfSquare + sizeOfSquare/2));
        }


        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Graphics graph = panel1.CreateGraphics();

            Point pos = calculateFileAndRankFromClick(e.X, e.Y);
            graph.DrawImage(Program.blackB, pos);
        }

       
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Graphics graph = panel1.CreateGraphics();
            //graph.DrawImage(Program.blackB, new Point(100, 100));
            
        }

        private Point calculateFileAndRankFromClick(int posX, int posY)
        {
            int file = posX / 80;
            int rank = 7-posY / 80;
            return Program.mainBoard.Grid[rank, file].PosOfImage;
        }

    }
}
