using System;
using System.Collections.Generic;
using System.Text;

namespace ChessBoardModel
{
    /*
    [0,0][0,1]...[0,7]
    [1,0][1,1]...[1,7]
    .         ....
    .         ....                  
    .         ....
    [7,0]     ...[7,7]

        ==

        PoV of black player
    */

    class Board
    {
        static readonly int Size = 8;
        public Square[,] Grid { get; set; }

        public Board()
        {
            Grid = new Square[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Grid[i, j] = new Square(i, j);
                }
            }
        }
    }
}
