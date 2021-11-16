using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Media;

namespace Chess
{
    static class Program
    {
        public static Board mainBoard;
        public static Image blackB, blackK, blackQ, blackN, blackP, blackR, whiteB, whiteK, whiteQ, whiteN, whiteP, whiteR;
        public static Cursor cursorBlackB, cursorblackK, cursorblackQ, cursorblackN, cursorblackP, cursorblackR, cursorwhiteB, cursorwhiteK, cursorwhiteQ, cursorwhiteN, cursorwhiteP, cursorwhiteR;
        public static int sizeOfPieces = 60, sizeOfIcons=76;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            InitializePiecesImages();
            InitializePieceCursors();
            mainBoard = new Board();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 game = new Form1();
            Application.Run(game);
               
        }


        /*
         Function that intializes the images for each piece
             */
        static void InitializePiecesImages()
        {
            blackB = resizeImage(Image.FromFile("bB.png"), new Size(sizeOfPieces, sizeOfPieces));
            blackK = resizeImage(Image.FromFile("bK.png"), new Size(sizeOfPieces, sizeOfPieces));
            blackQ = resizeImage(Image.FromFile("bQ.png"), new Size(sizeOfPieces, sizeOfPieces)); 
            blackN = resizeImage(Image.FromFile("bN.png"), new Size(sizeOfPieces, sizeOfPieces));
            blackP = resizeImage(Image.FromFile("bP.png"), new Size(sizeOfPieces, sizeOfPieces));
            blackR = resizeImage(Image.FromFile("bR.png"), new Size(sizeOfPieces, sizeOfPieces));
            whiteB = resizeImage(Image.FromFile("wB.png"), new Size(sizeOfPieces, sizeOfPieces));
            whiteK = resizeImage(Image.FromFile("wK.png"), new Size(sizeOfPieces, sizeOfPieces));
            whiteQ = resizeImage(Image.FromFile("wQ.png"), new Size(sizeOfPieces, sizeOfPieces));
            whiteN = resizeImage(Image.FromFile("wN.png"), new Size(sizeOfPieces, sizeOfPieces));
            whiteP = resizeImage(Image.FromFile("wP.png"), new Size(sizeOfPieces, sizeOfPieces));
            whiteR = resizeImage(Image.FromFile("wR.png"), new Size(sizeOfPieces, sizeOfPieces));
        }
        /*
         Function that intializes the cursors for each piece
             */
        static void InitializePieceCursors()
        {
            cursorBlackB = createCursorFromImage(blackB, new Size(sizeOfIcons, sizeOfIcons));
            cursorblackK = createCursorFromImage(blackK, new Size(sizeOfIcons, sizeOfIcons));
            cursorblackQ = createCursorFromImage(blackQ, new Size(sizeOfIcons, sizeOfIcons));
            cursorblackN = createCursorFromImage(blackN, new Size(sizeOfIcons, sizeOfIcons));
            cursorblackP = createCursorFromImage(blackP, new Size(sizeOfIcons, sizeOfIcons));
            cursorblackR = createCursorFromImage(blackR, new Size(sizeOfIcons, sizeOfIcons));
            cursorwhiteB = createCursorFromImage(whiteB, new Size(sizeOfIcons, sizeOfIcons));
            cursorwhiteK = createCursorFromImage(whiteK, new Size(sizeOfIcons, sizeOfIcons));
            cursorwhiteQ = createCursorFromImage(whiteQ, new Size(sizeOfIcons, sizeOfIcons));
            cursorwhiteN = createCursorFromImage(whiteN, new Size(sizeOfIcons, sizeOfIcons));
            cursorwhiteP = createCursorFromImage(whiteP, new Size(sizeOfIcons, sizeOfIcons));
            cursorwhiteR = createCursorFromImage(whiteR, new Size(sizeOfIcons, sizeOfIcons));
        }

        /*
         * Function that resizes any image to a bigger or smaller size 
         * credits : https://www.c-sharpcorner.com/UploadFile/ishbandhu2009/resize-an-image-in-C-Sharp/
        */
        private static Image resizeImage(Image imgToResize, Size size)
        {
            //Get the image current width  
            int sourceWidth = imgToResize.Width;
            //Get the image current height  
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calulate  width with new desired size  
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //Calculate height with new desired size  
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //New Width  
            int destWidth = (int)(sourceWidth * nPercent);
            //New Height  
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height  
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }

        /*
         Function that creates a cursor object from any image to a specified size
             */
        private static Cursor createCursorFromImage(Image img, Size size)
        {
            Bitmap bm = new Bitmap(img);
            return new Cursor(bm.GetHicon());
        }
    }
}



///credits for chess pieces : https://github.com/ornicar/lila/tree/master/public/piece/merida
///credits for chess pieces : https://github.com/ornicar/lila/tree/master/public/piece/merida
