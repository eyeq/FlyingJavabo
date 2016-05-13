using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingJavabo
{
    class ScreenDrawer
    {
        private const double radian = 0.0032;

        private DukeManager dukeManager;
        private JavaButtonManager buttonManager;
        private System.Drawing.Image dukeImage = Properties.Resources.duke;
        private System.Drawing.Image buttonImage = Properties.Resources.javabutton;

        private System.Drawing.Size graphicSize;
        private System.Drawing.Size tileSize;
        private System.Drawing.Size dukeSize;
        private System.Drawing.Size buttonSize;
        private System.Drawing.Size moveSpaceSize;
        private System.Drawing.Rectangle moveRect;
        private System.Drawing.Rectangle skyRect;

        private System.Drawing.Bitmap backgroundImage;

        public ScreenDrawer(int width, int height)
        {
            graphicSize = new System.Drawing.Size(width, height);
            tileSize = new System.Drawing.Size(width / 10, height / 10);
            dukeSize = new System.Drawing.Size(width / 10, width / 10);
            buttonSize = new System.Drawing.Size(width / 5, width / 25);

            backgroundImage = new System.Drawing.Bitmap(graphicSize.Width, graphicSize.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(backgroundImage);

            g.FillRectangle(System.Drawing.Brushes.White, 0, 0, width, height);
            DrawSky(g, graphicSize, System.Drawing.Color.LightCoral, System.Drawing.Color.White);
            System.Drawing.Point point = DrawFloor(g, graphicSize, tileSize, radian, System.Drawing.Brushes.Lime, System.Drawing.Brushes.Honeydew);

            moveRect = new System.Drawing.Rectangle(-point.X, 0, point.X * 2, point.Y);
            point = rotateX(point, radian);
            moveSpaceSize = new System.Drawing.Size(graphicSize.Width / 2 - point.X, graphicSize.Height / 2 - point.Y);

            skyRect = new System.Drawing.Rectangle(0, 0, graphicSize.Width, graphicSize.Height / 2);

            dukeManager = new DukeManager(moveSpaceSize.Height / 4);
            buttonManager = new JavaButtonManager(moveSpaceSize.Height / 3);
        }

        public void Draw(System.Drawing.Graphics graphics)
        {
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(graphicSize.Width, graphicSize.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(backgroundImage, 0, 0);

            dukeManager.Move(moveRect);
            DrawDuke(g, dukeSize, dukeManager, radian, dukeImage);

            buttonManager.Move(skyRect, moveRect.Height);
            DrawButton(g, buttonSize, buttonManager, buttonImage);

            graphics.DrawImage(image, 0, 0);
        }

        #region Draw Background

        private System.Drawing.Point DrawFloor(System.Drawing.Graphics graphics, System.Drawing.Size graphicSize, System.Drawing.Size tileSize, double radian, System.Drawing.Brush brushA, System.Drawing.Brush brushB)
        {
            double COSA = Math.Cos(radian);
            double SINA = Math.Sin(radian);

            System.Drawing.Point point = new System.Drawing.Point();
            bool isWidth = false;
            for(int i = 0; !isWidth; i++)
            {
                bool isHeight = false;
                for(int j = 0; !isHeight; j++)
                {
                    System.Drawing.Point[] points = new System.Drawing.Point[] {
                        new System.Drawing.Point(tileSize.Width*i, tileSize.Height*j),
                        new System.Drawing.Point(tileSize.Width*(i+1), tileSize.Height*j),
                        new System.Drawing.Point(tileSize.Width*(i+1), tileSize.Height*(j+1)),
                        new System.Drawing.Point(tileSize.Width*i, tileSize.Height*(j+1))
                    };
                    System.Drawing.Point temp = points[0];

                    System.Drawing.Point[] pointsa = new System.Drawing.Point[4];
                    System.Drawing.Point[] pointsb = new System.Drawing.Point[4];
                    for(int k = 0; k < 4; k++)
                    {
                        points[k] = rotateX(points[k], COSA, SINA);
                        pointsa[k] = translate(scale(points[k], -1, -1), graphicSize.Width / 2, graphicSize.Height);
                        pointsb[k] = translate(scale(points[k], 1, -1), graphicSize.Width / 2, graphicSize.Height);
                    }
                    bool isHalf = (i + j) % 2 == 0;
                    System.Drawing.Brush brusha = isHalf ? brushA : brushB;
                    System.Drawing.Brush brushb = !isHalf ? brushA : brushB;
                    graphics.FillPolygon(brusha, pointsa);
                    graphics.FillPolygon(brushb, pointsb);

                    isWidth = points[0].X > graphicSize.Width / 2;
                    isHeight = points[0].Y > graphicSize.Height / 2 || points[2].Y == points[0].Y;
                    if(j == 0 && !isWidth)
                    {
                        point.X = temp.X;
                    }
                    if(i == 0 && points[2].Y > points[0].Y + tileSize.Height / 10)
                    {
                        point.Y = temp.Y;
                    }
                }
            }
            return point;
        }

        private void DrawSky(System.Drawing.Graphics graphics, System.Drawing.Size graphicSize, System.Drawing.Color colorA, System.Drawing.Color colorB)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, graphicSize.Width, graphicSize.Height * 15 / 32);
            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(rect, colorA, colorB, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            graphics.FillRectangle(brush, rect);
        }

        #endregion

        private void DrawDuke(System.Drawing.Graphics graphics, System.Drawing.Size dukeSize, DukeManager manager, double radian, System.Drawing.Image image)
        {
            double COSA = Math.Cos(radian);
            double SINA = Math.Sin(radian);
            
            for (int i = 0; i < manager.DukeNumber; i++)
            {
                System.Drawing.Point duke = manager.GetDukePoint(i);
                System.Drawing.Point size = new System.Drawing.Point(duke.X, duke.Y - dukeSize.Height);

                duke = rotateX(duke, COSA, SINA);
                duke = scale(duke, 1, -1);
                duke = translate(duke, graphicSize.Width / 2, - moveSpaceSize.Height + graphicSize.Height);

                size = rotateX(size, COSA, SINA);
                size = scale(size, 1, -1);
                size = translate(size, graphicSize.Width / 2, -moveSpaceSize.Height + graphicSize.Height);

                int sizeInt = size.Y - duke.Y;
                if(sizeInt * 4 < dukeSize.Height)
                {
                    continue;
                }
                duke.X -= sizeInt / 2;
                duke.Y += sizeInt;
                graphics.DrawImage(image, new System.Drawing.Rectangle(duke, new System.Drawing.Size(sizeInt, sizeInt)));
            }
        }

        private void DrawButton(System.Drawing.Graphics graphics, System.Drawing.Size buttonSize, JavaButtonManager manager, System.Drawing.Image image)
        {
            for (int i = 0; i < manager.ButtonNumber; i++)
            {
                System.Drawing.Point button = manager.GetButtonPoint(i);
                int depth = manager.GetButtonDepth(i);
                if (depth == 0)
                {
                    continue;
                }
                int width = buttonSize.Width / depth;
                int height = buttonSize.Height / depth;
                graphics.DrawImage(image, new System.Drawing.Rectangle(button.X - width, button.Y - height, width, height));
            }
        }

        #region Point Transformation

        private System.Drawing.Point translate(System.Drawing.Point point, int x, int y)
        {
            return new System.Drawing.Point(point.X + x, point.Y + y);
        }

        private System.Drawing.Point scale(System.Drawing.Point point, double x, double y)
        {
            return new System.Drawing.Point((int)(point.X * x), (int)(point.Y * y));
        }

        private System.Drawing.Point rotateX(System.Drawing.Point point, double radian)
        {
            return rotateX(point, Math.Cos(radian), Math.Sin(radian));
        }

        private System.Drawing.Point rotateX(System.Drawing.Point point, double cosa, double sina)
        {
            double ya = point.Y * cosa - sina;
            double za = point.Y * sina + cosa;
            return new System.Drawing.Point((int)(point.X / za), (int)(ya / za));
        }

        #endregion
    }
}
