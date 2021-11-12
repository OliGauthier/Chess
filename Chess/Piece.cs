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
        public int[,] Position { get; set; }
        public int[,] StartingPosition { get; set; }
        public string PieceType { get; set; } //defined by one of the following letters with accordance to FEN notation: p, P, r, R, b, B, n, N, q, Q, k, K

        public Piece(string pieceType, int[,] startPos)
        {
            if (pieceType == "R")
                Console.WriteLine("L");
            PieceType = pieceType;
            switch (pieceType)
            {
                case "p":
                    PieceImage = Program.blackP;
                    break;
                case "P":
                    PieceImage = Program.whiteP;
                    break;
                case "r":
                    PieceImage = Program.blackR;
                    break;
                case "R":
                    this.PieceImage = Program.whiteR;
                    break;
                case "b":
                    PieceImage = Program.blackB;
                    break;
                case "B":
                    PieceImage = Program.whiteB;
                    break;
                case "n":
                    PieceImage = Program.blackN;
                    break;
                case "N":
                    PieceImage = Program.whiteN;
                    break;
                case "q":
                    PieceImage = Program.blackQ;
                    break;
                case "Q":
                    PieceImage = Program.whiteQ;
                    break;
                case "k":
                    PieceImage = Program.blackK;
                    break;
                case "K":
                    PieceImage = Program.whiteK;
                    break;
                default:
                    //gérer l'erreur si une pièce est crée qui ne devrait pas exister
                    break;
            }
            StartingPosition = startPos;
            Position = StartingPosition;
        }
    }
}
