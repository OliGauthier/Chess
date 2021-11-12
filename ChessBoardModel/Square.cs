using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ChessBoardModel
{
    class Square
    {
        public int Rank { get; set; } //represented on the board as Rank +1 (standard chess notation)
        public int File { get; set; } //a=0,b=1c=2,d=3,e=4,f=5,g=6,h=7 (standard chess notation)
        public bool Occupied { get; set; }
        public bool LegalMove { get; set; }
        public Color Color { get; set; }

        public Square(int file, int rank)
        {
            File = file;
            Rank = rank;
            if ((File + Rank) % 2 == 1)
                Color = Color.DarkSlateGray;
            else
                Color = Color.LightSlateGray;
        }
    }
}
