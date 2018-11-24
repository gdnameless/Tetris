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
            Nothing
        }

        int[] Keys = new int[] { 37, 39, 40, 32, 88, 89, 67 };

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
