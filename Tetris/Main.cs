using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace Tetris
{
    public partial class Form1 : Form
    {

        #region variable declarations

        Font font = new Font(FontFamily.Families[3], 10);
        SolidBrush solidbrush = new SolidBrush(Color.Black);
        string text, lasttext = "";
        bool run = true;
        bool[] PreviousKeys = new bool[8];
        int LeftStartHold = 0, RightStartHold = 0, UpStartHold = 0, DownStartHold = 0;
        int DAS = 7, ARR = 1, FRAMECOUNT = 0;

        int i = (int)Blocks.S; //only for testing

        bool drawing = false;

        Graphics g;

        Tetris Game = new Tetris();
        Tetromino Piece = new Tetromino((byte)Blocks.T);
        Controls Keys = new Controls();

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

        enum Actions
        {
            Left,
            Right,
            SoftDrop,
            HardDrop,
            RotateCW,
            RotateCCW,
            Hold,
            Up,
            Nothing
        } //possible actions

        #endregion

        #region Functions

        (int, int) GetMouseRelative()
        {
            return (MousePosition.X - Location.X - 20, MousePosition.Y - Location.Y - 43); //gets mouse position relative to Canvas
        }

        void MainLoop()
        {
            text = "Inputs detected: ";

            //get input

            bool temp;
            /*
            //this can later be used to get custom controls
            for (byte i = 1; i != 0; i++)
            {
                if (Keyboard.IsKeyDown((Key)i))
                {
                    MessageBox.Show(i.ToString());
                }
            }
            */
            if (Keyboard.IsKeyDown((Key)Keys.Keys[(int)Actions.Left]))
            {
                if (!PreviousKeys[(int)Actions.Left])
                {
                    LeftStartHold = Environment.TickCount;
                }
                else
                {
                    //uhh redo with one main game timer and cleaner code btw edit:doing lul
                }
                Piece.TempPos.x--;
                if (Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece))
                {
                    Piece.Pos.x--;
                }
                else
                {
                    Piece.TempPos.x++;
                }
                Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece);
                PreviousKeys[(int)Actions.Left] = true;
                text += "L; ";
            }
            else PreviousKeys[(int)Actions.Left] = false;

            if (Keyboard.IsKeyDown((Key)Keys.Keys[(int)Actions.Right]))
            {
                Piece.TempPos.x++;
                if (Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece))
                {
                    Piece.Pos.x++;
                }
                else
                {
                    Piece.TempPos.x--;
                }
                Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece);
                PreviousKeys[(int)Actions.Right] = true;
                text += "R; ";
            }
            else PreviousKeys[(int)Actions.Right] = false;

            if (Keyboard.IsKeyDown((Key)Keys.Keys[(int)Actions.SoftDrop]))
            {
                Piece.TempPos.y++;
                if (Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece))
                {
                    Piece.Pos.y++;
                }
                else
                {
                    Piece.TempPos.y--;
                }
                Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece);
                PreviousKeys[(int)Actions.SoftDrop] = true;
                text += "D; ";
            }
            else PreviousKeys[(int)Actions.SoftDrop] = false;

            if (Keyboard.IsKeyDown((Key)Keys.Keys[(int)Actions.Up]))
            {
                Piece.TempPos.y--;
                if (Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece))
                {
                    Piece.Pos.y--;
                }
                else
                {
                    Piece.TempPos.y++;
                }
                Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece);
                PreviousKeys[(int)Actions.Up] = true;
                text += "U; ";
            }
            else PreviousKeys[(int)Actions.Up] = false;

            if (Keyboard.IsKeyDown((Key)Keys.Keys[(int)Actions.RotateCCW]))
            {
                if (!PreviousKeys[(int)Actions.RotateCCW])
                {
                    Piece.RotateCounterClockwise();
                    temp = false;
                    while (!temp)
                    {
                        if (Piece.PerformSRS())
                        {
                            if (Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece))
                            {
                                Piece.UpdatePiece(true);
                                temp = true;
                            }
                        }
                        else
                        {
                            Piece.UpdatePiece(false);
                            temp = true;
                        }
                    }
                    Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece);
                }
                PreviousKeys[(int)Actions.RotateCCW] = true;
                text += "RCCW; ";
            }
            else PreviousKeys[(int)Actions.RotateCCW] = false;

            if (Keyboard.IsKeyDown((Key)Keys.Keys[(int)Actions.RotateCW]))
            {
                if (!PreviousKeys[(int)Actions.RotateCW])
                {
                    Piece.RotateClockwise();
                    temp = false;
                    while (!temp)
                    {
                        if (Piece.PerformSRS())
                        {
                            if (Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece))
                            {
                                Piece.UpdatePiece(true);
                                temp = true;
                            }
                        }
                        else
                        {
                            Piece.UpdatePiece(false);
                            temp = true;
                        }
                    }
                    Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece);
                }
                text += "RCW; ";
                PreviousKeys[(int)Actions.RotateCW] = true;
            }
            else PreviousKeys[(int)Actions.RotateCW] = false;

            if (Keyboard.IsKeyDown((Key)Keys.Keys[(int)Actions.Hold]))
            {
                if (!PreviousKeys[(int)Actions.Hold])
                {
                    Piece.SetBlock((byte)i);
                    Piece.Pos = (5, 0);
                    Piece.TempPos = (5, 0);
                    Piece.currentRotation = 0;
                    Piece.previousRotation = 0;
                    Piece.UpdatePiece(Game.CheckCollision(Piece.FromAnchorPoint(), Piece.Pos, Piece.Piece));
                }
                PreviousKeys[(int)Actions.Hold] = true;
                text += "H; ";
            }
            else PreviousKeys[(int)Actions.Hold] = false;

            if (Keyboard.IsKeyDown((Key)Keys.Keys[(int)Actions.HardDrop]))
            {
                if (!PreviousKeys[(int)Actions.HardDrop])
                {
                    Game.Harddrop(Piece.FromAnchorPoint(), Piece.Pos, Piece.Piece);
                    Piece.Pos = (5, 0);
                    Piece.TempPos = (5, 0);
                    Piece.currentRotation = 0;
                    Piece.previousRotation = 0;
                    Piece.SetBlock(Game.NextPiece());
                    Piece.UpdatePiece(Game.CheckCollision(Piece.FromAnchorPoint(), Piece.Pos, Piece.Piece));
                }
                PreviousKeys[(int)Actions.HardDrop] = true;
                text += "HD;";
            }
            else PreviousKeys[(int)Actions.HardDrop] = false;

            text += "\n";

            /*for (int y = 0; y < Game.Size.Height; y++)
            {
                for (int x = 0; x < Game.Size.Width; x++)
                {
                    text += (Game.VisibleBoard[x, y].ToString());
                }
                text += "\n";
            }*/

            Game.UpdateBlock(GetMouseRelative(), (byte)i, false); //ghostblock
            if (drawing) Game.UpdateBlock(GetMouseRelative(), (byte)i, true); //actual block

            //draw

            Game.RedrawBoard();
            g.DrawImage(Game.image, 0, 0);

            if (lasttext != text)
            {
                solidbrush.Color = Color.DimGray;
                g.FillRectangle(solidbrush, Game.image.Width, 0, Game.image.Width, Game.image.Height);
                
                solidbrush.Color = Color.Black;
                g.DrawString(text, font, solidbrush, new Point(Game.image.Width, 0));
            }

            FRAMECOUNT++;
            lasttext = text;
        }

        #endregion

        #region Form Events

        private void Canvas_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            drawing = true;
            //GhostTimer.Stop(); //don't draw ghostblocks if currently drawing real blocks
            //Game.UpdateBlock(GetMouseRelative(), (byte)i, true); //sets block
            //g.DrawImage(Game.image, 0, 0); //draws to Canvas
            //DrawTimer.Interval = 100; //DAS for the mouse
            //DrawTimer.Start(); //starts drawing
        }

        private void Canvas_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            drawing = false;
            //DrawTimer.Stop(); //stops drawing
            //GhostTimer.Start(); //reenable ghostblocks
        }

        private void DrawTimer_Tick(object sender, EventArgs e)
        {
            //DrawTimer.Interval = 8;
            //Game.UpdateBlock(GetMouseRelative(), (byte)i, true); //sets block
            //g.DrawImage(Game.image, 0, 0); //draws to Canvas
        }

        private void Canvas_MouseEnter(object sender, EventArgs e)
        {
            if (drawing) //don't draw ghostblocks if currently drawing real blocks
            {
                //GhostTimer.Stop();
            }
            else
            {
                //GhostTimer.Start();
            }
        }

        private void Canvas_MouseLeave(object sender, EventArgs e) //stops redrawing ghostblocks when mouse outside of Canvas
        {
            //GhostTimer.Stop();
            Game.UpdateBlock((-1, -1), 0, false); //removes last ghostblock when leaving window
            //g.DrawImage(Game.image, 0, 0);
        }

        private void GhostTimer_Tick(object sender, EventArgs e) //redraws ghostblocks
        {
            if (drawing) //don't draw ghostblocks if currently drawing real blocks
            {
                //GhostTimer.Stop();
            }
            //Game.UpdateBlock(GetMouseRelative(), (byte)i, false); //sets ghostblock at mouse position
            //g.DrawImage(Game.image, 0, 0); //draws to Canvas
        }

        private void MainGameTimer_Tick(object sender, EventArgs e) //Main loop ~60 times a second
        {
            if (run)
            {
                MainLoop();
            }
            else
            {
                Close();
            }
        }

        private void Form1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e) //scroll
        {
            if (e.Delta > 0) //scroll up
            {
                i++;
                i %= 9;
            }
            else //scroll down
            {
                i--;
                if (i < 0) i = 8;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            g = Canvas.CreateGraphics(); //makes drawing on Canvas (Picturebox) possible
        }

        #endregion

        public Form1() //Constructor
        {
            InitializeComponent();

            MouseWheel += Form1_MouseWheel; //adds a MouseEventHandler to handle scrolling

            //Cursor.Hide();

            //set size according to board size
            //Canvas.Size = new Size(Game.Size.Width * Game.Blocksize_ + 1, Game.Size.Height * Game.Blocksize_ + 1);
            Canvas.Size = new Size((Game.Size.Width * Game.Blocksize_ + 1) * 2, Game.Size.Height * Game.Blocksize_ + 1);
            Size = new Size(Canvas.Size.Width + 39, Canvas.Size.Height + 62);
            Game.NextPiece();
            MainGameTimer.Start();
        }

        private void KeyPressedTimer_Tick(object sender, EventArgs e)
        {
            /*bool temp;
            
            //this can later be used to get custom controls
            for (byte i = 1; i != 0; i++)
            {
                if (Keyboard.IsKeyDown((Key)i))
                {
                    MessageBox.Show(i.ToString());
                }
            }
            
            if (Keyboard.IsKeyDown((Key)Keys.Keys[(int)Actions.Left]))
            {
                if (!PreviousKeys[(int)Actions.Left])
                {
                    LeftStartHold = Environment.TickCount;
                }
                else
                {
                    //uhh redo with one main game timer and cleaner code btw
                }
                Piece.TempPos.x--;
                if (Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece))
                {
                    Piece.Pos.x--;
                }
                else
                {
                    Piece.TempPos.x++;
                }
                Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece);
                PreviousKeys[(int)Actions.Left] = true;
            }
            else PreviousKeys[(int)Actions.Left] = false;

            if (Keyboard.IsKeyDown((Key)Keys.Keys[(int)Actions.Right]))
            {
                Piece.TempPos.x++;
                if (Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece))
                {
                    Piece.Pos.x++;
                }
                else
                {
                    Piece.TempPos.x--;
                }
                Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece);
                PreviousKeys[(int)Actions.Right] = true;
            }
            else PreviousKeys[(int)Actions.Right] = false;

            if (Keyboard.IsKeyDown((Key)Keys.Keys[(int)Actions.RotateCCW]))
            {
                if (!PreviousKeys[(int)Actions.RotateCCW])
                {
                    Piece.RotateCounterClockwise();
                    temp = false;
                    while (!temp)
                    {
                        if (Piece.PerformSRS())
                        {
                            if (Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece))
                            {
                                Piece.UpdatePiece(true);
                                temp = true;
                            }
                        }
                        else
                        {
                            Piece.UpdatePiece(false);
                            temp = true;
                        }
                    }
                    Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece);
                }
                PreviousKeys[(int)Actions.RotateCCW] = true;
            }
            else PreviousKeys[(int)Actions.RotateCCW] = false;

            if (Keyboard.IsKeyDown((Key)Keys.Keys[(int)Actions.RotateCW]))
            {
                if (!PreviousKeys[(int)Actions.RotateCW])
                {
                    Piece.RotateClockwise();
                    temp = false;
                    while (!temp)
                    {
                        if (Piece.PerformSRS())
                        {
                            if (Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece))
                            {
                                Piece.UpdatePiece(true);
                                temp = true;
                            }
                        }
                        else
                        {
                            Piece.UpdatePiece(false);
                            temp = true;
                        }
                    }
                    Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece);
                }
                PreviousKeys[(int)Actions.RotateCW] = true;
            }
            else PreviousKeys[(int)Actions.RotateCW] = false;

            if (Keyboard.IsKeyDown((Key)Keys.Keys[(int)Actions.SoftDrop]))
            {
                Piece.TempPos.y++;
                if (Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece))
                {
                    Piece.Pos.y++;
                }
                else
                {
                    Piece.TempPos.y--;
                }
                PreviousKeys[(int)Actions.SoftDrop] = true;
            }
            else PreviousKeys[(int)Actions.SoftDrop] = false;

            if (Keyboard.IsKeyDown((Key)Keys.Keys[(int)Actions.Up]))
            {
                Piece.TempPos.y--;
                if (Game.CheckCollision(Piece.FromAnchorPoint(), Piece.TempPos, Piece.Piece))
                {
                    Piece.Pos.y--;
                }
                else
                {
                    Piece.TempPos.y++;
                }
                PreviousKeys[(int)Actions.Up] = true;
            }
            else PreviousKeys[(int)Actions.Up] = false;

            if (Keyboard.IsKeyDown((Key)Keys.Keys[(int)Actions.Hold]))
            {
                if (!PreviousKeys[(int)Actions.Hold])
                {
                    Piece.SetBlock((byte)i);
                    Piece.Pos = (5, 0);
                    Piece.TempPos = (5, 0);
                    Piece.currentRotation = 0;
                    Piece.previousRotation = 0;
                    Piece.UpdatePiece(Game.CheckCollision(Piece.FromAnchorPoint(), Piece.Pos, Piece.Piece));
                }
                PreviousKeys[(int)Actions.Hold] = true;
            }
            else PreviousKeys[(int)Actions.Hold] = false;

            if (Keyboard.IsKeyDown((Key)Keys.Keys[(int)Actions.HardDrop]))
            {
                if (!PreviousKeys[(int)Actions.HardDrop])
                {
                    Game.Harddrop(Piece.FromAnchorPoint(), Piece.Pos, Piece.Piece);
                    Piece.Pos = (5, 0);
                    Piece.TempPos = (5, 0);
                    Piece.currentRotation = 0;
                    Piece.previousRotation = 0;
                    Piece.SetBlock(Game.NextPiece());
                    Piece.UpdatePiece(Game.CheckCollision(Piece.FromAnchorPoint(), Piece.Pos, Piece.Piece));
                }
                PreviousKeys[(int)Actions.HardDrop] = true;
            }
            else PreviousKeys[(int)Actions.HardDrop] = false;

            g.DrawImage(Game.image, 0, 0);*/
        }
    }
}
