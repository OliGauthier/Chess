using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    
    class Board
    {
        static readonly string StartFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        static readonly int Size = 8;
        public Square[,] Grid { get; set; }//1st param is the rank, second param is the file

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

            //initialize board from basic FEN
            this.FillBoardFromFEN(StartFen);
        }


        public void FillBoardFromFEN(string FEN)
        {
            //TO DO : check for valid FEN


            int file = 0, rank = 7, counter = 0;
            while (FEN[counter].ToString() != " ")
            {
                //skip the right number of squares that are empty
                bool isNumeric = int.TryParse(FEN[counter].ToString(), out int step);
                if (isNumeric)
                    file += step;
                //start the next rank
                else if (FEN[counter].ToString() == "/")
                {
                    rank--;
                    file = 0;
                }
                else
                {
                    this.Grid[rank, file].Piece = new Piece(FEN[counter].ToString(), new int[rank, file]);
                    file++;
                }
                counter++;
            }

            //TO DO : implementer lecture des 5 pamètres restants

        }

        public string CreateFenFromBoard()
        {
            string FEN = "";
            int emptySpaceCounter = 0;
            //read piece positions
            for (int rank = 7; rank >= 0; rank--)
            {
                for (int file = 0; file < 8; file++)
                {
                    if (this.Grid[rank, file].Piece == null)
                        emptySpaceCounter++;
                    else
                    {
                        if(emptySpaceCounter!=0)
                            FEN += emptySpaceCounter.ToString();
                        emptySpaceCounter = 0;
                        FEN+=this.Grid[rank, file].Piece.PieceType;
                    }
                }
                if (emptySpaceCounter != 0)
                    FEN += emptySpaceCounter.ToString();
                emptySpaceCounter = 0;
                if (rank!=0)
                    FEN += "/";
            }

            return FEN;
        
        }

        //TO DO : board constructor from specific FEN
    }
}
