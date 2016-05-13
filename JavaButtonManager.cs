using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingJavabo
{
    class JavaButtonManager
    {
        private bool isInit = false;

        public int ButtonNumber { get; }
        private System.Drawing.Point[] buttons;
        private int[] depthes;

        public JavaButtonManager(int buttonNumber)
        {
            ButtonNumber = buttonNumber;
            buttons = new System.Drawing.Point[buttonNumber];
            depthes = new int[buttonNumber];
        }

        public System.Drawing.Point GetButtonPoint(int buttonIndex)
        {
            return buttons[buttonIndex];
        }

        public int GetButtonDepth(int buttonIndex)
        {
            return depthes[buttonIndex];
        }

        public void Move(System.Drawing.Rectangle moveSpace, int depth)
        {
            if (!isInit)
            {
                isInit = true;
                InitButtons(moveSpace, depth);
            }
            int speed = depth / 400;
            for (int i = 0; i < ButtonNumber; i++)
            {
                depthes[i] -= speed;
                depthes[i] = (depthes[i] % depth + depth) % depth; 
            }
        }

        private void InitButtons(System.Drawing.Rectangle moveSpace, int depth)
        {
            System.Random rand = new Random();
            for (int i = 0; i < ButtonNumber; i++)
            {
                buttons[i].X = rand.Next(moveSpace.Width) + moveSpace.X;
                buttons[i].Y = rand.Next(moveSpace.Height) + moveSpace.Y;
                depthes[i] = rand.Next(depth);
            }
        }
    }
}
