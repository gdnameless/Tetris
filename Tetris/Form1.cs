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

        int i = 0; //only for testing

        Graphics g;

        Tetris Game = new Tetris();

        enum Blocks
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
            g.DrawImage(Game.PlaceBlock(GetMouseRelative(), (byte)(i++ % 8 + 1)), 0, 0);
            timer1.Interval = 400;
            timer1.Start();
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 1000 / 60;
            g.DrawImage(Game.PlaceBlock(GetMouseRelative(), (byte)(i++ % 8 + 1)), 0, 0);
        }

        #endregion

        #region unimportant

        public Form1()
        {
            InitializeComponent();
        }

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
