using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Andrew
{
    public partial class Form1 : Form
    {
        private Pen pen;
        List<Point> points;
        List<Point> convex_hull;

        public Form1()
        {
            InitializeComponent();
        }

        // знаковая площадь треугольника oab, 
        // взятая со знаком плюс или минус в зависимости от типа поворота, образуемого точками
        private int cross(Point o, Point a, Point b)
        {
            return (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);
        }

        private void Andrew()
        {
            if (points.Count < 3) return;
            // сортируем список точек по X (если X равны, то по Y)
            var sorted_points = points.OrderBy(p => p.X).ThenBy(p => p.Y);
            List<Point> up = new List<Point>(); // верхняя оболочка
            List<Point> down = new List<Point>(); // нижняя оболочка
            // самая левая и самая правая точки
            Point p1 = sorted_points.First(), p2 = sorted_points.Last();
            up.Add(p1);
            down.Add(p1);
            for (int i = 1; i < sorted_points.Count(); ++i)
            {
                if (i == sorted_points.Count() - 1 || cross(p1, sorted_points.ElementAt(i), p2) < 0) //поворот по часовой стрелке
                {
                    // если текущая тройка точек образует не правый поворот, 
                    while (up.Count >= 2 && cross(up[up.Count - 2], up[up.Count - 1], sorted_points.ElementAt(i)) >= 0)
                        // то ближайшего соседа нужно удалить
                        up.RemoveAt(up.Count - 1);
                    // добавляем точку в оболочку
                    up.Add(sorted_points.ElementAt(i));
                }
                if (i == sorted_points.Count() - 1 || cross(p1, sorted_points.ElementAt(i), p2) > 0) //поворот против часовой стрелки
                {
                    while (down.Count >= 2 && cross(down[down.Count - 2], down[down.Count - 1], sorted_points.ElementAt(i)) <= 0)
                        down.RemoveAt(down.Count - 1);
                    down.Add(sorted_points.ElementAt(i));
                }
            }
            //удаляем последнюю точку верхней оболочки и первую нижней (каждая есть в другом листе)
            up.RemoveAt(up.Count - 1);
            down.RemoveAt(0);            
            // сливаем оболочки
            for (int i = 0; i < up.Count; ++i)
                //if(!convex_hull.Contains(up[i]))
                    convex_hull.Add(up[i]);
            for (int i = down.Count - 1; i >= 0; --i)
                //if (!convex_hull.Contains(down[i]))
                    convex_hull.Add(down[i]);
        }

        private void construct_Click(object sender, EventArgs e)
        {
            if (points.Count < 3) return;
            Andrew();
            Graphics g = Graphics.FromImage(pictureBox.Image);
            for (int i = 0; i < convex_hull.Count - 1; ++i)
                g.DrawLine(pen, convex_hull[i], convex_hull[i + 1]);
            g.DrawLine(pen, convex_hull[0], convex_hull[convex_hull.Count - 1]);
            pictureBox.Invalidate();
            convex_hull.Clear();
        }

        private void clear_Click(object sender, EventArgs e)
        {
            if (points.Count > 0)
                points.Clear();
            if(convex_hull.Count > 0)
                convex_hull.Clear();
            Graphics g = Graphics.FromImage(pictureBox.Image);
            g.Clear(Color.White);
            g.Dispose();
            pictureBox.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pen = new Pen(Color.Black, 1);
            pen.StartCap = pen.EndCap = LineCap.Round;

            Image bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.Dispose();
            if (pictureBox.Image != null)
                pictureBox.Image.Dispose();
            pictureBox.Image = bmp;
            points = new List<Point>();
            convex_hull = new List<Point>();
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            Graphics g = Graphics.FromImage(pictureBox.Image);
            g.DrawRectangle(pen, new Rectangle(e.X, e.Y, 1, 1));
            g.Dispose();
            //Bitmap bmp = pictureBox.Image as Bitmap;
            //bmp.SetPixel(e.X, e.Y, Color.Black);
            points.Add(new Point(e.X, e.Y));
            pictureBox.Invalidate();
        }
    }
}
