using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
    class Piece
    {

        

        public Image PieceImage { get; set; }
        public System.Windows.Forms.Cursor PieceCursor { get; set; }
        public (int,int) Position { get; set; }//(rank,file)
        public (int,int) StartingPosition { get; set; }//(rank,file)
        public string PieceType { get; set; } //defined by one of the following letters with accordance to FEN notation: p, P, r, R, b, B, n, N, q, Q, k, K
        public bool White { get; set; }
        public List<(int, int)> PossibleMoves { get; set; }

        public int shitcompeut { get; set; }

        public Piece(string pieceType, (int,int) startPos)
        {
            PieceType = pieceType;
            switch (pieceType)
            {
                case "p":
                    PieceImage = Program.blackP;
                    PieceCursor = Program.cursorblackP;
                    break;
                case "P":
                    PieceImage = Program.whiteP;
                    PieceCursor = Program.cursorwhiteP;
                    White = true;
                    break;
                case "r":
                    PieceImage = Program.blackR;
                    PieceCursor = Program.cursorblackR;
                    break;
                case "R":
                    this.PieceImage = Program.whiteR;
                    PieceCursor = Program.cursorwhiteR;
                    White = true;
                    break;
                case "b":
                    PieceImage = Program.blackB;
                    PieceCursor = Program.cursorBlackB;
                    break;
                case "B":
                    PieceImage = Program.whiteB;
                    PieceCursor = Program.cursorwhiteB;
                    White = true;
                    break;
                case "n":
                    PieceImage = Program.blackN;
                    PieceCursor = Program.cursorblackN;
                    break;
                case "N":
                    PieceImage = Program.whiteN;
                    PieceCursor = Program.cursorwhiteN;
                    White = true;
                    break;
                case "q":
                    PieceImage = Program.blackQ;
                    PieceCursor = Program.cursorblackQ;
                    break;
                case "Q":
                    PieceImage = Program.whiteQ;
                    PieceCursor = Program.cursorwhiteQ;
                    White = true;
                    break;
                case "k":
                    PieceImage = Program.blackK;
                    PieceCursor = Program.cursorblackK;
                    break;
                case "K":
                    PieceImage = Program.whiteK;
                    PieceCursor = Program.cursorwhiteK;
                    White = true;
                    break;
                default:
                    //gérer l'erreur si une pièce est crée qui ne devrait pas exister
                    break;
            }
            StartingPosition = startPos;
            Position = StartingPosition;
            PossibleMoves = new List<(int, int)>();
            shitcompeut = 0;
        }

        public void CalculateAvaibleMoves(Board board)
        {
            PossibleMoves = new List<(int, int)>();
            if (shitcompeut == 1)
                Console.WriteLine("");
            //the qeen moves like a rook and a bishop so it uses both if statements
            if (PieceType == "R" || PieceType == "r" || PieceType == "Q" || PieceType == "q" )
            {
                //check if can go down the ranks
                for (int i = Position.Item1 - 1; i >= 0; i--)
                {
                    if (board.Grid[i, Position.Item2].Piece == null)
                        PossibleMoves.Add((i, Position.Item2));
                    else if (White == board.Grid[i, Position.Item2].Piece.White)//piece is of same color as piece on the square it wants to move to
                        break;
                    else
                    {
                        PossibleMoves.Add((i, Position.Item2));
                        break;
                    }
                }
                //check if can go up the ranks
                for (int i = Position.Item1 + 1; i < 8; i++)
                {
                    if(board.Grid[i, Position.Item2].Piece==null)
                        PossibleMoves.Add((i, Position.Item2));
                    else if (White == board.Grid[i, Position.Item2].Piece.White)//piece is of same color as piece on the square it wants to move to
                        break;
                    else
                    {
                        PossibleMoves.Add((i, Position.Item2));
                        break;
                    }
                }
                //check if can towards a file
                for (int i = Position.Item2 - 1; i >= 0; i--)
                {
                    if (board.Grid[Position.Item1, i].Piece == null)
                        PossibleMoves.Add((Position.Item1, i));
                    else if (White == board.Grid[Position.Item1, i].Piece.White)//piece is of same color as piece on the square it wants to move to
                        break;
                    else
                    {
                        PossibleMoves.Add((Position.Item1, i));
                        break;
                    }
                }
                //check if can go towards h file
                for (int i = Position.Item2 + 1; i < 8; i++)//check if can go right
                {
                    if (board.Grid[Position.Item1, i].Piece == null)
                        PossibleMoves.Add((Position.Item1, i));
                    else if (White == board.Grid[Position.Item1, i].Piece.White)//piece is of same color as piece on the square it wants to move to
                        break;
                    else
                    {
                        PossibleMoves.Add((Position.Item1, i));
                        break;
                    }
                }
            }
            if (PieceType == "B" || PieceType == "b" || PieceType == "Q" || PieceType== "q")
            {
                //check if can go down the ranks & towards a file
                int movement = -1;
                while (true)
                {
                    //square is inside the board
                    if (Position.Item1 + movement >= 0 & Position.Item2 + movement >= 0)
                    {
                        if (board.Grid[Position.Item1 + movement, Position.Item2 + movement].Piece == null)
                            PossibleMoves.Add((Position.Item1 + movement, Position.Item2 + movement));
                        else if (White == board.Grid[Position.Item1 + movement, Position.Item2 + movement].Piece.White)//piece is of same color as piece on the square it wants to move to
                            break;
                        else
                        {
                            PossibleMoves.Add((Position.Item1 + movement, Position.Item2 + movement));
                            break;
                        }
                    }
                    else
                        break;
                    movement--;
                }
                //check if can go up the ranks & towards h file
                movement = 1;
                while (true)
                {
                    //square is inside the board
                    if (Position.Item1 + movement <= 7 & Position.Item2 + movement <= 7)
                    {
                        if (board.Grid[Position.Item1 + movement, Position.Item2 + movement].Piece == null)
                            PossibleMoves.Add((Position.Item1 + movement, Position.Item2 + movement));
                        else if (White == board.Grid[Position.Item1 + movement, Position.Item2 + movement].Piece.White)//piece is of same color as piece on the square it wants to move to
                            break;
                        else
                        {
                            PossibleMoves.Add((Position.Item1 + movement, Position.Item2 + movement));
                            break;
                        }
                    }
                    else
                        break;
                    movement++;
                }

                //check if can go down the ranks & towards h file
                movement = -1;
                int movement2 = 1;
                while (true)
                {
                    //square is inside the board
                    if (Position.Item1 + movement >= 0 & Position.Item2 + movement2 <= 7)
                    {
                        if (board.Grid[Position.Item1 + movement, Position.Item2 + movement2].Piece == null)
                            PossibleMoves.Add((Position.Item1 + movement, Position.Item2 + movement2));
                        else if (White == board.Grid[Position.Item1 + movement, Position.Item2 + movement2].Piece.White)//piece is of same color as piece on the square it wants to move to
                            break;
                        else
                        {
                            PossibleMoves.Add((Position.Item1 + movement, Position.Item2 + movement2));
                            break;
                        }
                    }
                    else
                        break;
                    movement--;
                    movement2++;
                }
                //check if can go up the ranks & towards a file
                movement = 1;
                movement2 = -1;
                while (true)
                {
                    //square is inside the board
                    if (Position.Item1 + movement <= 7 & Position.Item2 + movement2 >= 0)
                    {
                        if (board.Grid[Position.Item1 + movement, Position.Item2 + movement2].Piece == null)
                            PossibleMoves.Add((Position.Item1 + movement, Position.Item2 + movement2));
                        else if (White == board.Grid[Position.Item1 + movement, Position.Item2 + movement2].Piece.White)//piece is of same color as piece on the square it wants to move to
                            break;
                        else
                        {
                            PossibleMoves.Add((Position.Item1 + movement, Position.Item2 + movement2));
                            break;
                        }
                    }
                    else
                        break;
                    movement++;
                    movement2--;
                }


            }

            if (PieceType == "N" || PieceType == "n")
            {
                (int, int)[] moves = { (2,1), (2,-1), (-2,1), (-2,-1), (1,2), (1,-2), (-1,2), (-1,-2) };
                int rankDestination;
                int fileDestination;
                for (int i = 0; i < 8; i++)
                {
                    rankDestination = Position.Item1 + moves[i].Item1;
                    fileDestination = Position.Item2 + moves[i].Item2;
                    //square is inside the board
                    if (rankDestination >= 0 && rankDestination <= 7 && fileDestination >= 0 && fileDestination <= 7)
                    {
                        if (board.Grid[rankDestination, fileDestination].Piece == null)
                            PossibleMoves.Add((rankDestination, fileDestination));
                        else if (White != board.Grid[rankDestination, fileDestination].Piece.White)//can move to square with piece of different color
                            PossibleMoves.Add((rankDestination, fileDestination));
                    }
                }
            }
            if (PieceType == "K" || PieceType == "k")
            {
                (int, int)[] moves = { (1, 1), (1, -1), (-1, 1), (-1, -1), (1, 0), (0, -1), (0, 1), (-1, 0) };
                int rankDestination;
                int fileDestination;
                for (int i = 0; i < 8; i++)
                {
                    rankDestination = Position.Item1 + moves[i].Item1;
                    fileDestination = Position.Item2 + moves[i].Item2;
                    //square is inside the board
                    if (rankDestination >= 0 && rankDestination <= 7 && fileDestination >= 0 && fileDestination <= 7)
                    {
                        if (board.Grid[rankDestination, fileDestination].Piece == null)
                            PossibleMoves.Add((rankDestination, fileDestination));
                        else if (White != board.Grid[rankDestination, fileDestination].Piece.White)//can move to square with piece of different color
                            PossibleMoves.Add((rankDestination, fileDestination));
                    }
                }
                //TO DO : Handle castling using current FEN
                if (PieceType == "K")
                {
                    if (board.WhiteCanCastleK & board.Grid[Position.Item1, Position.Item2 + 1].Piece == null & board.Grid[Position.Item1, Position.Item2 + 2].Piece == null)
                        PossibleMoves.Add((Position.Item1, Position.Item2 + 2));
                    if (board.WhiteCanCastleQ & board.Grid[Position.Item1, Position.Item2 - 1].Piece == null & board.Grid[Position.Item1, Position.Item2 - 2].Piece == null & board.Grid[Position.Item1, Position.Item2 - 3].Piece == null)
                        PossibleMoves.Add((Position.Item1, Position.Item2 -2));
                }
                else if (PieceType == "k")
                {
                    if (board.BlackCanCastleK & board.Grid[Position.Item1, Position.Item2 + 1].Piece == null & board.Grid[Position.Item1, Position.Item2 + 2].Piece == null)
                        PossibleMoves.Add((Position.Item1, Position.Item2 + 2));
                    if (board.BlackCanCastleQ & board.Grid[Position.Item1, Position.Item2 - 1].Piece == null & board.Grid[Position.Item1, Position.Item2 - 2].Piece == null & board.Grid[Position.Item1, Position.Item2 - 3].Piece == null)
                        PossibleMoves.Add((Position.Item1, Position.Item2 - 2));
                }
            }

            if (PieceType == "P" || PieceType == "p")
            {
                
                int forward1, forward2, startRank;
                if (PieceType == "P")
                {
                    startRank = 1;
                    forward1 = 1;
                    forward2 = 2;
                }
                else
                {
                    startRank = 6;
                    forward1 = -1;
                    forward2 = -2;
                }
                //capturable piece
                if(Position.Item2 + 1 <= 7)
                    if (board.Grid[Position.Item1 + forward1, Position.Item2 + 1].Piece != null)
                        if (White != board.Grid[Position.Item1 + forward1, Position.Item2 + 1].Piece.White)
                            PossibleMoves.Add((Position.Item1 + forward1, Position.Item2 + 1));
                if(Position.Item2 -1 >=0)    
                    if (board.Grid[Position.Item1 + forward1, Position.Item2 - 1].Piece != null)
                        if (White != board.Grid[Position.Item1 + forward1, Position.Item2 - 1].Piece.White)
                            PossibleMoves.Add((Position.Item1 + forward1, Position.Item2 - 1));

                //one move ahead
                if (board.Grid[Position.Item1 + forward1, Position.Item2].Piece == null)
                {
                    PossibleMoves.Add((Position.Item1 + forward1, Position.Item2));
                    //if we are on the recond second rank we could move two squares ahead
                    if (Position.Item1 == startRank & board.Grid[Position.Item1 + forward2, Position.Item2].Piece == null)
                        PossibleMoves.Add((Position.Item1 + forward2, Position.Item2));
                }
                //en passant
                if (board.EnPassantPossible != null)
                {
                    (int, int) enPassantSquare = Square.GetIndexesBasedOnName(board.EnPassantPossible);
                    if (PieceType == "P" & ((Position.Item1 + 1, Position.Item2 + 1) == enPassantSquare || (Position.Item1 + 1, Position.Item2 - 1) == enPassantSquare))
                        PossibleMoves.Add(enPassantSquare);
                    else if(PieceType == "p" & ((Position.Item1 - 1, Position.Item2 + 1) == enPassantSquare || (Position.Item1 - 1, Position.Item2 - 1) == enPassantSquare))
                        PossibleMoves.Add(enPassantSquare);

                }
                

            }
        }
    }
}
