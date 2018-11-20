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
        } //all the used blocks

        byte[,] Board, GhostBoard; //Values in board determine the block type, one board for regular blocks, one for ghostblocks

        (int x, int y) LastGhost; //keeps track of last ghostblock

        public (int Width, int Height) Size; //determines board size

        public int Blocksize, Blocksize_; //blocksize and blocksize + 1 (because I needed it surprisingly often)

        //needed for drawing
        public Bitmap image;
        Graphics graphics;
        SolidBrush brush;
        Pen pen;
        Color BackgroundColor = Color.DimGray;

        #endregion

        public Tetris(int Width = 10, int Height = 21, int Blocksize = 20) //constructor
        {
            //initializing custom board settings and boards
            Size = (Width, Height);
            Board = new byte[Size.Width, Size.Height];
            GhostBoard = new byte[Size.Width, Size.Height];
            this.Blocksize = Blocksize;
            Blocksize_ = Blocksize + 1;

            //initializing stuff needed for drawing the board
            image = new Bitmap(Size.Width * Blocksize_ + 1, Size.Height * Blocksize_ + 1); //additional space for the lines inbetween and the border lines
            graphics = Graphics.FromImage(image);
            pen = new Pen(Color.Black);
            brush = new SolidBrush(BackgroundColor);

            //draws background, separators and outline for the board (in that order)
            graphics.FillRectangle(brush, 0, 0, image.Width, image.Height);
            for (int x = 0; x < Size.Width; x++)
            {
                graphics.DrawLine(pen, x * Blocksize_, 0, x * Blocksize_, image.Height);
            }
            for (int y = 0; y < Size.Height; y++)
            {
                graphics.DrawLine(pen, 0, y * Blocksize_, image.Width, y * Blocksize_);
            }
            graphics.DrawRectangle(pen, 0, 0, image.Width - 1, image.Height - 1);
        }

        #region functions

        /*public Bitmap PlaceBlock((int x, int y) Pos, byte blockType, bool Real = true, bool Overwrite = false)
        {

            for (int x = 0; x < GhostBoard.GetLength(0); x++)
            {
                for (int y = 0; y < GhostBoard.GetLength(1); y++)
                {
                    if (GhostBoard[x, y] == (byte)Blocks.empty) Board[x, y] = (byte)Blocks.empty;
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
        }*/

        /*public Bitmap PlaceBlocks(List<(int x, int y, byte)> BlockList, bool Real = true, bool Overwrite = false)
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
        }*/

        public void UpdateBlock((int x, int y) Pos, byte blockType, bool Real, bool Overwrite = true) //updates a single block on the board and then draws it to the image Bitmap, might edit the parameters
        {
            //clears the ghostblock
            GhostBoard[LastGhost.x, LastGhost.y] = (byte)Blocks.empty;
            DrawBlock(LastGhost);

            try
            {
                //translates mouse position (origin point top left corner of the board) to the corresponding block on the board
                Pos.x = Pos.x / Blocksize_;
                Pos.y = Pos.y / Blocksize_;

                if (Overwrite) //can ovewrite already existing blocks
                {
                    if (Real) //real block
                    {
                        Board[Pos.x, Pos.y] = blockType;
                    }
                    else //ghostblock
                    {
                        GhostBoard[Pos.x, Pos.y] = blockType;
                        LastGhost = (Pos.x, Pos.y);
                    }
                    DrawBlock(Pos); //draws the changed block on the bitmap
                }
                else
                {
                    if (!Real || Board[Pos.x, Pos.y] == ((byte)Blocks.empty)) //ghostblocks can be shown on top of real blocks but real blocks cannot be overwritten
                    {
                        if (Real) //real block
                        {
                            Board[Pos.x, Pos.y] = blockType;
                        }
                        else //ghostblock
                        {
                            GhostBoard[Pos.x, Pos.y] = blockType;
                            LastGhost = (Pos.x, Pos.y);
                        }
                        DrawBlock(Pos); //draws the changed block on the bitmap
                    }
                }
            }
            catch
            {

            }
        }

        public void UpdateBlocks(List<(int x, int y, byte)> BlockList, bool Real, bool Overwrite = true) //this might get used once we actually draw tetrominoes, not commented yet because currently unused
        {
            //will get updated once it needs to be used
            for (int x = 0; x < GhostBoard.GetLength(0); x++)
            {
                for (int y = 0; y < GhostBoard.GetLength(1); y++)
                {
                    GhostBoard[x, y] = (byte)Blocks.empty;
                }
            }

            try
            {
                if (!Overwrite)
                {
                    foreach ((int x, int y, byte b) in BlockList)
                    {
                        if (Real)
                        {
                            Board[x, y] = b;
                        }
                        else
                        {
                            GhostBoard[x, y] = b;
                        }
                        DrawBlock((x, y));
                    }
                }
                else
                {
                    foreach ((int x, int y, byte b) in BlockList)
                    {
                        if (!Real || Board[x, y] == ((byte)Blocks.empty))
                        {
                            if (Real)
                            {
                                Board[x, y] = b;
                            }
                            else
                            {
                                GhostBoard[x, y] = b;
                            }
                            DrawBlock((x, y));
                        }
                    }
                }
            }
            catch
            {

            }
        }

        public void DrawBlock((int x, int y) Pos)
        {
            //draw real block
            switch (Board[Pos.x, Pos.y])
            {
                case (byte)Blocks.empty:
                    brush.Color = BackgroundColor;
                    break;
                case (byte)Blocks.garbage:
                    brush.Color = Color.LightGray;
                    break;
                case (byte)Blocks.I:
                    brush.Color = Color.FromArgb(0x00, 0x9F, 0xDA);
                    break;
                case (byte)Blocks.J:
                    brush.Color = Color.FromArgb(0x00, 0x65, 0xBD);
                    break;
                case (byte)Blocks.L:
                    brush.Color = Color.FromArgb(0xFF, 0x79, 0x00);
                    break;
                case (byte)Blocks.O:
                    brush.Color = Color.FromArgb(0xFE, 0xCB, 0x00);
                    break;
                case (byte)Blocks.S:
                    brush.Color = Color.FromArgb(0x69, 0xBE, 0x28);
                    break;
                case (byte)Blocks.T:
                    brush.Color = Color.FromArgb(0x95, 0x2D, 0x98);
                    break;
                case (byte)Blocks.Z:
                    brush.Color = Color.FromArgb(0xED, 0x29, 0x39);
                    break;
            }
            graphics.FillRectangle(brush, Pos.x * Blocksize_ + 1, Pos.y * Blocksize_ + 1, Blocksize, Blocksize);

            //draw ghostblock with transparency
            switch (GhostBoard[Pos.x, Pos.y])
            {
                case (byte)Blocks.empty:
                    brush.Color = Color.FromArgb(0x40, BackgroundColor);
                    break;
                case (byte)Blocks.garbage:
                    brush.Color = Color.FromArgb(0x40, Color.LightGray);
                    break;
                case (byte)Blocks.I:
                    brush.Color = Color.FromArgb(0x40, 0x00, 0x9F, 0xDA);
                    break;
                case (byte)Blocks.J:
                    brush.Color = Color.FromArgb(0x40, 0x00, 0x65, 0xBD);
                    break;
                case (byte)Blocks.L:
                    brush.Color = Color.FromArgb(0x40, 0xFF, 0x79, 0x00);
                    break;
                case (byte)Blocks.O:
                    brush.Color = Color.FromArgb(0x40, 0xFE, 0xCB, 0x00);
                    break;
                case (byte)Blocks.S:
                    brush.Color = Color.FromArgb(0x40, 0x69, 0xBE, 0x28);
                    break;
                case (byte)Blocks.T:
                    brush.Color = Color.FromArgb(0x40, 0x95, 0x2D, 0x98);
                    break;
                case (byte)Blocks.Z:
                    brush.Color = Color.FromArgb(0x40, 0xED, 0x29, 0x39);
                    break;
            }
            graphics.FillRectangle(brush, Pos.x * Blocksize_ + 1, Pos.y * Blocksize_ + 1, Blocksize, Blocksize);
        }

        /*public Bitmap Draw()
        {
            brush.Color = Color.Black;
            for (int y = 0; y < Board.GetLength(1); y++)
            {
                for (int x = 0; x < Board.GetLength(0); x++)
                {
                    if (Board[x, y] != (byte)Blocks.empty)
                    {
                        switch (Board[x, y])
                        {
                            case (byte)Blocks.garbage:
                                brush.Color = Color.LightGray;
                                break;
                            case (byte)Blocks.I:
                                brush.Color = Color.DeepSkyBlue;
                                break;
                            case (byte)Blocks.J:
                                brush.Color = Color.Blue;
                                break;
                            case (byte)Blocks.L:
                                brush.Color = Color.Orange;
                                break;
                            case (byte)Blocks.O:
                                brush.Color = Color.Yellow;
                                break;
                            case (byte)Blocks.S:
                                brush.Color = Color.LimeGreen;
                                break;
                            case (byte)Blocks.T:
                                brush.Color = Color.Purple;
                                break;
                            case (byte)Blocks.Z:
                                brush.Color = Color.Red;
                                break;
                        }
                        if (!GhostBoard[x, y]) brush.Color = Color.FromArgb(128, brush.Color);
                        graphics.FillRectangle(brush, x * Blocksize_ + 1, y * Blocksize_ + 1, Blocksize, Blocksize);
                    }
                }
            }
            return image;
        }*/

        #endregion

    }
}
