using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Tetris
    {

        #region variable declarations

        public (int InitialDelay, int RepeatDelay) DAS = (400, 6); //not used yet

        public enum Blocks
        {
            empty,
            T,
            I,
            S,
            Z,
            O,
            L,
            J,
            garbage
        }

        byte[,] Board;
        bool[,] GhostBoard;

        public (int Width, int Height) Size;

        public int Blocksize, Blocksize_;

        Bitmap image;
        Graphics g;
        SolidBrush p;
        Pen pen;

        #endregion

        public Tetris(int Width = 10, int Height = 21, int Blocksize = 20) //Constructor
        {
            Size = (Width, Height);
            Board = new byte[Size.Width, Size.Height];
            GhostBoard = new bool[Size.Width, Size.Height];
            this.Blocksize = Blocksize;
            Blocksize_ = Blocksize + 1;
            image = new Bitmap(Size.Width * Blocksize_ + 1, Size.Height * Blocksize_ + 1); //space for the lines inbetween and the border lines
            g = Graphics.FromImage(image);
            pen = new Pen(Color.Black);
            p = new SolidBrush(Color.DimGray);
        }

        #region functions

        public Bitmap PlaceBlock((int x, int y) Pos, byte blockType, bool Real = true, bool Overwrite = false)
        {
            for (int x = 0; x < GhostBoard.GetLength(0); x++)
            {
                for (int y = 0; y < GhostBoard.GetLength(1); y++)
                {
                    if (GhostBoard[x, y] == false) Board[x, y] = (byte)Blocks.empty;
                    GhostBoard[x, y] = true;
                }
            }
            try
            {
                Pos.x = Pos.x / Blocksize_;
                Pos.y = Pos.y / Blocksize_;
                if (!Overwrite)
                {
                    if (Board[Pos.x, Pos.y] == ((byte)Blocks.empty))
                    {
                        Board[Pos.x, Pos.y] = blockType;
                        GhostBoard[Pos.x, Pos.y] = Real;
                    }
                }
                else
                {
                    Board[Pos.x, Pos.y] = blockType;
                    GhostBoard[Pos.x, Pos.y] = Real;
                }
            }
            catch
            {

            }
            return Draw();
        }

        public Bitmap PlaceBlocks(List<(int x, int y, byte)> BlockList, bool Real = true, bool Overwrite = false)
        {
            for (int x = 0; x < GhostBoard.GetLength(0); x++)
            {
                for (int y = 0; y < GhostBoard.GetLength(1); y++)
                {
                    if (GhostBoard[x, y] == false) Board[x, y] = (byte)Blocks.empty;
                    GhostBoard[x, y] = true;
                }
            }
            try
            {
                if (!Overwrite)
                {
                    foreach ((int x, int y, byte b) in BlockList)
                    {
                        int X = x / Blocksize_;
                        int Y = y / Blocksize_;
                        if (Board[X, Y] == (byte)Blocks.empty)
                        {
                            Board[X, Y] = b;
                            GhostBoard[X, Y] = Real;
                        }
                    }
                }
                else
                {
                    foreach ((int x, int y, byte b) in BlockList)
                    {
                        int X = x / Blocksize_;
                        int Y = y / Blocksize_;
                        Board[X, Y] = b;
                        GhostBoard[X, Y] = Real;
                    }
                }
            }
            catch
            {

            }
            return Draw();
        }

        public Bitmap Draw()
        {
            p.Color = Color.DimGray;
            g.FillRectangle(p, 0, 0, image.Width, image.Height);
            p.Color = Color.Black;
            for (int y = 0; y < Board.GetLength(1); y++)
            {
                for (int x = 0; x < Board.GetLength(0); x++)
                {
                    if (Board[x, y] != (byte)Blocks.empty)
                    {
                        switch (Board[x, y])
                        {
                            case (byte)Blocks.garbage:
                                p.Color = Color.LightGray;
                                break;
                            case (byte)Blocks.I:
                                p.Color = Color.DeepSkyBlue;
                                break;
                            case (byte)Blocks.J:
                                p.Color = Color.Blue;
                                break;
                            case (byte)Blocks.L:
                                p.Color = Color.Orange;
                                break;
                            case (byte)Blocks.O:
                                p.Color = Color.Yellow;
                                break;
                            case (byte)Blocks.S:
                                p.Color = Color.LimeGreen;
                                break;
                            case (byte)Blocks.T:
                                p.Color = Color.Purple;
                                break;
                            case (byte)Blocks.Z:
                                p.Color = Color.Red;
                                break;
                        }
                        if (!GhostBoard[x, y]) p.Color = Color.FromArgb(128, p.Color);
                        g.FillRectangle(p, x * Blocksize_ + 1, y * Blocksize_ + 1, Blocksize, Blocksize);
                    }
                }
            }
            for (int x = 0; x < Size.Width; x++)
            {
                g.DrawLine(pen, x * Blocksize_, 0, x * Blocksize_, image.Height);
            }
            for (int y = 0; y < Size.Height; y++)
            {
                g.DrawLine(pen, 0, y * Blocksize_, image.Width, y * Blocksize_);
            }
            g.DrawRectangle(pen, 0, 0, image.Width - 1, image.Height - 1);
            return image;
        }

        #endregion

    }
}
