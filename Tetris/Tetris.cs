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

        (int InitialDelay, int RepeatDelay) DAS; //not used yet

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

        (int Width, int Height) Size;

        int Blocksize, Blocksize_;

        #endregion

        public Tetris(int Width = 10, int Height = 21, int Blocksize = 20, int InitialDelay = 400, int RepeatDelay = 6)
        {
            Size = (Width, Height);
            Board = new byte[Size.Width, Size.Height];
            for (int i = 0; i < Board.Length; i++)
            {
                Board[i / Size.Height, i % Size.Width] = (byte)Blocks.empty;
            }
            this.Blocksize = Blocksize;
            Blocksize_ = Blocksize + 1;
            DAS = (InitialDelay, RepeatDelay);
        } //Constructor, probably move DAS somewhere else

        #region functions

        public Bitmap PlaceBlock((int x, int y) Pos, byte blockType, bool Overwrite = false)
        {
            try
            {
                Pos.x = Pos.x / Blocksize_;
                Pos.y = Pos.y / Blocksize_;
                if (!Overwrite)
                {
                    if (Board[Pos.x, Pos.y] == (byte)Blocks.empty)
                    {
                        Board[Pos.x, Pos.y] = blockType;
                    }
                }
                else
                {
                    Board[Pos.x, Pos.y] = blockType;
                }
            }
            catch
            {

            }
            return Draw();
        }

        public Bitmap PlaceBlocks(List<(int x, int y, byte)> BlockList, bool Overwrite = false)
        {
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
            Bitmap image = new Bitmap(Size.Width * Blocksize_ + 1, Size.Height * Blocksize_ + 1); //space for the lines inbetween and the border lines
            Graphics g = Graphics.FromImage(image);
            SolidBrush p = new SolidBrush(Color.Green);
            Pen pen = new Pen(Color.Black);
            for (int y = 0; y < Board.GetLength(1); y++)
            {
                for (int x = 0; x < Board.GetLength(0); x++)
                {
                    if (Board[x, y] != (byte)Blocks.empty)
                    {
                        switch (Board[x, y])
                        {
                            case (byte)Blocks.garbage:
                                p.Color = Color.SlateGray;
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
                                p.Color = Color.Green;
                                break;
                            case (byte)Blocks.T:
                                p.Color = Color.Purple;
                                break;
                            case (byte)Blocks.Z:
                                p.Color = Color.Red;
                                break;
                        }
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
