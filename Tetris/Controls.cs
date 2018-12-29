using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Controls
    {
        public enum Actions
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
        }

        public int[] Keys = new int[] { 23, 25, 26, 18, 67, 68, 46, 24 };

        public int EvaluateKey(int Key)
        {
            for (int i = 0; i < Keys.Length; i++)
            {
                if (Keys[i] == Key)
                {
                    return i;
                }
            }
            return (int)Actions.Nothing;
        }
    }
}
