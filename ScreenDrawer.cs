using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingJavabo
{
    class ScreenDrawer
    {
        public void Draw(System.Drawing.Graphics graphics, int width, int height)
        {
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(width, height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);

            g.FillRectangle(System.Drawing.Brushes.White, 0, 0, width, height);
            DrawFloor(g, width, height);
            DrawSky(g, width, height);

            graphics.DrawImage(image, 0, 0);
        }

        #region Draw Background

        private void DrawFloor(System.Drawing.Graphics graphics, int width, int height)
        {
            int SIZE_X = width / 10;
            int SIZE_Y = height / 10;
            double COSA = Math.Cos(0.0026);
            double SINA = Math.Sin(0.0026);

            bool isWidth = false;
            for(int i = 0; !isWidth; i++)
            {
                bool isHeight = false;
                for(int j = 0; !isHeight; j++)
                {
                    System.Drawing.Point[] points = {
                        new System.Drawing.Point(SIZE_X*i, SIZE_Y*j),
                        new System.Drawing.Point(SIZE_X*(i+1), SIZE_Y*j),
                        new System.Drawing.Point(SIZE_X*(i+1), SIZE_Y*(j+1)),
                        new System.Drawing.Point(SIZE_X*i, SIZE_Y*(j+1))
                    };
                    System.Drawing.Point[] pointsa = new System.Drawing.Point[4];
                    System.Drawing.Point[] pointsb = new System.Drawing.Point[4];
                    for(int k = 0; k < 4; k++)
                    {
                        points[k] = rotateX(points[k], COSA, SINA);
                        pointsa[k] = translate(scale(points[k], -1, -1), width / 2, height);
                        pointsb[k] = translate(scale(points[k], 1, -1), width / 2, height);
                    }
                    bool isLimeA = (i + j) % 2 == 0;
                    System.Drawing.Brush brusha = isLimeA ? System.Drawing.Brushes.Lime : System.Drawing.Brushes.Honeydew;
                    System.Drawing.Brush brushb = !isLimeA ? System.Drawing.Brushes.Lime : System.Drawing.Brushes.Honeydew;
                    graphics.FillPolygon(brusha, pointsa);
                    graphics.FillPolygon(brushb, pointsb);

                    isWidth = points[0].X > width / 2;
                    isHeight = points[0].Y > height / 2 || points[2].Y == points[0].Y;
                }
            }
        }

        private void DrawSky(System.Drawing.Graphics graphics, int width, int height)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, width, height * 15 / 32);
            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(rect, System.Drawing.Color.LightCoral, System.Drawing.Color.White, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            graphics.FillRectangle(brush, rect);
        }

        #endregion

        #region Point Transformation

        private System.Drawing.Point translate(System.Drawing.Point point, int x, int y)
        {
            return new System.Drawing.Point(point.X + x, point.Y + y);
        }

        private System.Drawing.Point scale(System.Drawing.Point point, double x, double y)
        {
            return new System.Drawing.Point((int)(point.X * x), (int)(point.Y * y));
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
