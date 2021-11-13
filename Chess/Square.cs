using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
    class Square
    {
        static readonly string[] fileIntToString = { "a", "b", "c", "d", "e", "f", "g", "h" };
        public int Rank { get; set; } //represented on the board as Rank +1 (standard chess notation)
        public int File { get; set; } //a=0,b=1c=2,d=3,e=4,f=5,g=6,h=7 (standard chess notation)
        public Piece Piece { get; set; }
        public bool LegalMove { get; set; }
        public Color Color { get; set; }
        public string Name { get; set; }

        public Point Corner { get; set; }

        public Point PosOfImage { get; set; }

        public Square(int rank, int file)
        {
            File = file;
            Rank = rank;
            if ((File + Rank) % 2 == 0)
                Color = Color.DarkGreen;
            else
                Color = Color.Beige;
            Name = $"{fileIntToString[file]}{rank+1}";
            Corner = new Point(Form1.widthOfSquare * File,Form1.widthOfSquare*7-Form1.widthOfSquare*rank);
            PosOfImage = new Point(Corner.X + 10, Corner.Y + 10);
        
        }


    }
}
