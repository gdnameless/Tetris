using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Tetromino
    {
        public byte Piece, rotation = (byte)Rotation.north;
        public (int x, int y) Pos;

        public byte[,] CurrentPiece = new byte[4, 4];

        public Tetromino(byte Piece)
        {
            this.Piece = Piece;
            Pos = (4, 0);
            SetBlock(Piece);
        }

        public enum Blocks
        {
            empty,
            T,
            I,
            S,
            Z,
            O,
            L,
            J
        } //all the used blocks

        public enum Rotation
        {
            north,
            east,
            south,
            west
        } //rotations

        public byte[,] SetBlock(byte Tetromino)
        {
            switch (Tetromino)
            {
                case (byte)Blocks.I:
                    Piece = (byte)Blocks.I;
                    CurrentPiece = new byte[,] {
                        { 0, 0, 0, 0 },
                        { 1, 1, 1, 1 },
                        { 0, 0, 0, 0 },
                        { 0, 0, 0, 0 }
                    };
                    break;
                case (byte)Blocks.J:
                    Piece = (byte)Blocks.J;
                    CurrentPiece = new byte[,] {
                        { 1, 0, 0, 0 },
                        { 1, 1, 1, 0 },
                        { 0, 0, 0, 0 },
                        { 0, 0, 0, 0 }
                    };
                    break;
                case (byte)Blocks.L:
                    Piece = (byte)Blocks.L;
                    CurrentPiece = new byte[,] {
                        { 0, 0, 1, 0 },
                        { 1, 1, 1, 0 },
                        { 0, 0, 0, 0 },
                        { 0, 0, 0, 0 }
                    };
                    break;
                case (byte)Blocks.O:
                    Piece = (byte)Blocks.O;
                    CurrentPiece = new byte[,] {
                        { 1, 1, 0, 0 },
                        { 1, 1, 0, 0 },
                        { 0, 0, 0, 0 },
                        { 0, 0, 0, 0 }
                    };
                    break;
                case (byte)Blocks.S:
                    Piece = (byte)Blocks.S;
                    CurrentPiece = new byte[,] {
                        { 0, 1, 1, 0 },
                        { 1, 1, 0, 0 },
                        { 0, 0, 0, 0 },
                        { 0, 0, 0, 0 }
                    };
                    break;
                case (byte)Blocks.T:
                    Piece = (byte)Blocks.T;
                    CurrentPiece = new byte[,] {
                        { 0, 1, 0, 0 },
                        { 1, 1, 1, 0 },
                        { 0, 0, 0, 0 },
                        { 0, 0, 0, 0 }
                    };
                    break;
                case (byte)Blocks.Z:
                    Piece = (byte)Blocks.Z;
                    CurrentPiece = new byte[,] {
                        { 1, 1, 0, 0 },
                        { 0, 1, 1, 0 },
                        { 0, 0, 0, 0 },
                        { 0, 0, 0, 0 }
                    };
                    break;
                default:
                    Piece = (byte)Blocks.empty;
                    CurrentPiece = new byte[,] {
                        { 0, 0, 0, 0 },
                        { 0, 0, 0, 0 },
                        { 0, 0, 0, 0 },
                        { 0, 0, 0, 0 }
                    };
                    break;
            }
            byte[,] temp = new byte[4, 4];
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    temp[x, y] = CurrentPiece[y, x];
                }
            }
            CurrentPiece = temp;
            return CurrentPiece;
        }

        public (byte, byte)[] FromAnchorPoint()
        {
            (byte, byte)[] temp = new (byte, byte)[4];
            int i = 0;
            for (byte y = 0; y < 4; y++)
            {
                for (byte x = 0; x < 4; x++)
                {
                    if (CurrentPiece[x, y] == 1) temp[i++] = (x, y);
                }
            }
            return temp;
        }

        public void RotateClockwise()
        {
            if (Piece == (byte)Blocks.I)
            {
                byte[,] temp = new byte[4, 4];
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        temp[x, y] = CurrentPiece[y, 3 - x];
                    }
                }
                CurrentPiece = temp;
            }
            else if (Piece != (byte)Blocks.O)
            {
                byte[,] temp = new byte[4, 4];
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        temp[x, y] = CurrentPiece[y, 2 - x];
                    }
                }
                CurrentPiece = temp;
            }

            //O rotation has no effect

        }

        public void RotateCounterClockwise()
        {
            if (Piece == (byte)Blocks.I)
            {
                byte[,] temp = new byte[4, 4];
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        temp[x, y] = CurrentPiece[3 - y, x];
                    }
                }
                CurrentPiece = temp;
            }
            else if (Piece != (byte)Blocks.O)
            {
                byte[,] temp = new byte[4, 4];
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        temp[x, y] = CurrentPiece[2 - y, x];
                    }
                }
                CurrentPiece = temp;
            }

            //O rotation has no effect

        }
    }
}
