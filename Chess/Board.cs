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
        static readonly string NoMinorPiecesFen = "r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R w KQkq - 0 1";
        static readonly string EnPassantFen = "r3k2r/p1p1p1p1/8/1P1P1P1P/1p1p1p1p/8/P1P1P1P1/R3K2R w KQkq - 0 1";
        static readonly int Size = 8;

        public bool WhiteCanCastleK { get; set; } 
        public bool WhiteCanCastleQ { get; set; }
        public bool BlackCanCastleK { get; set; }
        public bool BlackCanCastleQ { get; set; }
        public bool WhiteTurn { get; set; }

        public string EnPassantPossible { get; set; }

        public int TurnCount { get; set; }
        public int MoveCounter_50rule { get; set; }
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
            bool isNumeric;

            int file = 0, rank = 7, counter = 0;
            while (FEN[counter].ToString() != " ")
            {
                //skip the right number of squares that are empty
                isNumeric = int.TryParse(FEN[counter].ToString(), out int step);
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
                    this.Grid[rank, file].Piece = new Piece(FEN[counter].ToString(), (rank, file));
                    file++;
                }
                counter++;
            }
            counter++;
            //check who's turn it is
            if (FEN[counter].ToString() == "w")
                WhiteTurn = true;
            else if (FEN[counter].ToString() == "b")
                WhiteTurn = false;
            //check what are the possible castles
            counter += 2;
                BlackCanCastleK = false;
                BlackCanCastleQ = false;
                WhiteCanCastleK = false;
                WhiteCanCastleQ = false;
            if (FEN[counter].ToString() == "-")
                counter++;
            else 
            {
                while (FEN[counter].ToString() != " ")
                {
                    if (FEN[counter].ToString() == "K")
                        WhiteCanCastleK = true;
                    else if (FEN[counter].ToString() == "Q")
                        WhiteCanCastleQ = true;
                    else if (FEN[counter].ToString() == "k")
                        BlackCanCastleK = true;
                    else if (FEN[counter].ToString() == "q")
                        BlackCanCastleQ = true;
                    counter++;
                }
            }
            counter++;
            // check if en passant is possible (possible pp crusher)
            if (FEN[counter].ToString() == "-")
                EnPassantPossible = null;
            else
            {
                EnPassantPossible = FEN[counter].ToString();
                counter++;
                EnPassantPossible += FEN[counter].ToString();
            }
            //check for 50 move rule
            counter += 2;
            isNumeric = int.TryParse(FEN[counter].ToString(), out int number);
            if (isNumeric)
                MoveCounter_50rule = number;
            else
                MoveCounter_50rule = 0;
            counter += 2;
            isNumeric = int.TryParse(FEN[counter].ToString(), out number);
            if (isNumeric)
                TurnCount = number;
            else
                TurnCount = 1;

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
            
            //player turn
            FEN += " ";
            if (WhiteTurn)
                FEN += "w";
            else
                FEN += "b";
            //possible castles
            FEN += " ";
            if (WhiteCanCastleK)
                FEN += "K";
            if (WhiteCanCastleQ)
                FEN += "Q";
            if (BlackCanCastleK)
                FEN += "k";
            if (BlackCanCastleQ)
                FEN += "q";
            if (WhiteCanCastleK || WhiteCanCastleQ || BlackCanCastleK || BlackCanCastleQ)//pas rajouter un white space si aucun castle possible
                FEN += " ";
            //TO DO : en passant
            if (EnPassantPossible != null)
                FEN += EnPassantPossible;
            else
                FEN += "-";
            //50 move rule and turn count
            FEN += $" {MoveCounter_50rule} {TurnCount}";
            

            return FEN;
        
        }

        //TO DO : board constructor from specific FEN
    }
}
