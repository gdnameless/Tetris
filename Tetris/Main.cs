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

        #endregion

        #region Functions

        (int, int) GetMouseRelative()
        {
            return (MousePosition.X - Location.X - 20, MousePosition.Y - Location.Y - 43);
        }

        #endregion

        #region Form Events

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            drawing = true;
            g.DrawImage(Game.PlaceBlock(GetMouseRelative(), (byte)i, true, true), 0, 0);
            DrawTimer.Interval = 100;
            DrawTimer.Start();
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            drawing = false;
            DrawTimer.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawTimer.Interval = 16;
            g.DrawImage(Game.PlaceBlock(GetMouseRelative(), (byte)i, true, true), 0, 0);
        }

        private void Canvas_MouseEnter(object sender, EventArgs e)
        {
            if (drawing)
            {
                GhostTimer.Stop();
            }
            else
            {
                GhostTimer.Start();
            }
        }

        private void Canvas_MouseLeave(object sender, EventArgs e)
        {
            GhostTimer.Stop();
        }

        private void GhostTimer_Tick(object sender, EventArgs e)
        {
            g.DrawImage(Game.PlaceBlock(GetMouseRelative(), (byte)i, false), 0, 0);
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                i++;
                i %= 9;
            }
            else
            {
                i--;
                if (i < 0) i = 8;
            }
        }

        #endregion

        public Form1() //Constructor
        {
            InitializeComponent();
            MouseWheel += Form1_MouseWheel;
            Canvas.Size = new Size(Game.Size.Width * Game.Blocksize_ + 1, Game.Size.Height * Game.Blocksize_ + 1);
            Size = new Size(Canvas.Size.Width + 39, Canvas.Size.Height + 62);
        }

        #region unimportant

        private void Form1_Shown(object sender, EventArgs e)
        {
            g = Canvas.CreateGraphics();
            //Game.Draw();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            //g.DrawImage(Game.PlaceBlock(GetMouseRelative(), (byte)(i++ % 8 + 1)), 0, 0);
        }

        private void Canvas_Click(object sender, EventArgs e)
        {
            //g.DrawImage(Game.PlaceBlock(GetMouseRelative(), (byte)(i++ % 8 + 1)), 0, 0);
        }

        private void Canvas_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //g = Canvas.CreateGraphics();
            //Game.Draw();
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            //Game.Draw();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        #endregion
    }
}
