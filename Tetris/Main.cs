using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        #region variable declarations

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
            Nothing
        } //possible actions

        #endregion

        #region Functions

        (int, int) GetMouseRelative()
        {
            return (MousePosition.X - Location.X - 20, MousePosition.Y - Location.Y - 43); //gets mouse position relative to Canvas
        }

        #endregion

        #region Form Events

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            drawing = true;
            GhostTimer.Stop(); //don't draw ghostblocks if currently drawing real blocks
            Game.UpdateBlock(GetMouseRelative(), (byte)i, true); //sets block
            g.DrawImage(Game.image, 0, 0); //draws to Canvas
            DrawTimer.Interval = 100; //DAS for the mouse
            DrawTimer.Start(); //starts drawing
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            drawing = false;
            DrawTimer.Stop(); //stops drawing
            GhostTimer.Start(); //reenable ghostblocks
        }

        private void DrawTimer_Tick(object sender, EventArgs e)
        {
            DrawTimer.Interval = 8;
            Game.UpdateBlock(GetMouseRelative(), (byte)i, true); //sets block
            g.DrawImage(Game.image, 0, 0); //draws to Canvas
        }

        private void Canvas_MouseEnter(object sender, EventArgs e)
        {
            if (drawing) //don't draw ghostblocks if currently drawing real blocks
            {
                GhostTimer.Stop();
            }
            else
            {
                GhostTimer.Start();
            }
        }

        private void Canvas_MouseLeave(object sender, EventArgs e) //stops redrawing ghostblocks when mouse outside of Canvas
        {
            GhostTimer.Stop();
            Game.UpdateBlock((-1, -1), 0, false); //removes last ghostblock when leaving window
            g.DrawImage(Game.image, 0, 0);
        }

        private void GhostTimer_Tick(object sender, EventArgs e) //redraws ghostblocks
        {
            if (drawing) //don't draw ghostblocks if currently drawing real blocks
            {
                GhostTimer.Stop();
            }
            Game.UpdateBlock(GetMouseRelative(), (byte)i, false); //sets ghostblock at mouse position
            g.DrawImage(Game.image, 0, 0); //draws to Canvas
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e) //scroll
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
            Canvas.Size = new Size(Game.Size.Width * Game.Blocksize_ + 1, Game.Size.Height * Game.Blocksize_ + 1);
            Size = new Size(Canvas.Size.Width + 39, Canvas.Size.Height + 62);
            Game.NextPiece();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            int Action = Keys.EvaluateKey(e.KeyValue);
            switch (Action)
            {
                case (int)Actions.HardDrop:
                    Game.Harddrop(Piece.FromAnchorPoint(), Piece.Pos, Piece.Piece);
                    Piece.Pos = (5, 0);
                    Piece.SetBlock(Game.NextPiece());
                    Game.UpdateTetromino(Piece.FromAnchorPoint(), Piece.Pos, Piece.Piece);
                    break;
                case (int)Actions.Right:
                    Piece.Pos.x++;
                    Game.UpdateTetromino(Piece.FromAnchorPoint(), Piece.Pos, Piece.Piece);
                    break;
                case (int)Actions.Left:
                    Piece.Pos.x--;
                    Game.UpdateTetromino(Piece.FromAnchorPoint(), Piece.Pos, Piece.Piece);
                    break;
                case (int)Actions.RotateCCW:
                    Piece.RotateCounterClockwise();
                    Game.UpdateTetromino(Piece.FromAnchorPoint(), Piece.Pos, Piece.Piece);
                    break;
                case (int)Actions.RotateCW:
                    Piece.RotateClockwise();
                    Game.UpdateTetromino(Piece.FromAnchorPoint(), Piece.Pos, Piece.Piece);
                    break;
                case (int)Actions.SoftDrop:
                    Piece.Pos.y++;
                    Game.UpdateTetromino(Piece.FromAnchorPoint(), Piece.Pos, Piece.Piece);
                    break;
                case (int)Actions.Hold:
                    Piece.SetBlock((byte)i);
                    Game.UpdateTetromino(Piece.FromAnchorPoint(), Piece.Pos, Piece.Piece);
                    break;
            }
            g.DrawImage(Game.image, 0, 0);
        }
    }
}
