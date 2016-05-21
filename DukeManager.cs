using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingJavabo
{
    class DukeManager
    {
        private bool isInit = false;

        public int DukeNumber { get; }
        private System.Drawing.Point[] dukes;

        public DukeManager(int dukeNumber)
        {
            DukeNumber = dukeNumber;
            dukes = new System.Drawing.Point[dukeNumber];
        }

        public System.Drawing.Point GetDukePoint(int dukeIndex)
        {
            return dukes[dukeIndex];
        }

        public void Move(System.Drawing.Rectangle moveSpace)
        {
            if (!isInit)
            {
                isInit = true;
                InitDukes(moveSpace);
            }
            int speed = moveSpace.Width / 100;
            int height = moveSpace.Height;
            for (int i = 0; i < DukeNumber; i++)
            {
                dukes[i].Y -= speed;
                dukes[i].Y -= moveSpace.Y;
                dukes[i].Y = (dukes[i].Y % height + height) % height;
                dukes[i].Y += moveSpace.Y;
            }
        }

        private void InitDukes(System.Drawing.Rectangle moveSpace)
        {
            System.Random rand = new Random();
            for (int i = 0; i < DukeNumber; i++)
            {
                dukes[i].X = rand.Next(moveSpace.Width) + moveSpace.X;
                dukes[i].Y = rand.Next(moveSpace.Height) + moveSpace.Y;
                for(int j = 0; j < i; j++)
                {
                    if(dukes[j].Y < dukes[i].Y)
                    {
                        System.Drawing.Point temp = dukes[j];
                        dukes[j] = dukes[i];
                        dukes[i] = temp;
                    }
                }
            }
        }
    }
}
