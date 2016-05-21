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

        public void Move(System.Drawing.Size moveSpace)
        {
            if (!isInit)
            {
                isInit = true;
                InitDukes(moveSpace);
            }
            int speed = System.Math.Max(1, moveSpace.Width / 128);
            int height = moveSpace.Height;
            for (int i = 0; i < DukeNumber; i++)
            {
                dukes[i].Y -= speed;
                dukes[i].Y = (dukes[i].Y % height + height) % height;
            }
        }

        private void InitDukes(System.Drawing.Size moveSpace)
        {
            System.Random rand = new Random();
            for (int i = 0; i < DukeNumber; i++)
            {
                dukes[i].X = rand.Next(moveSpace.Width);
                dukes[i].Y = rand.Next(moveSpace.Height);
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
