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
            if (pieceType == "R")
                Console.WriteLine("L");
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
            if (PieceType == "R" || PieceType == "r")
            {
                for (int i = Position.Item1 - 1; i >= 0; i--)//check if can go down the ranks
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
                for (int i = Position.Item1 + 1; i < 8; i++)//check if can go up the ranks
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
                for (int i = Position.Item2 - 1; i >= 0; i--)//check if can go left
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
            if (PieceType == "B" || PieceType == "b")
            {
                //for ()
                //{ }
            }
            shitcompeut++;
        }
    }
}
