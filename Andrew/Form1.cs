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

        public Form1()
        {
            InitializeComponent();
        }

        private void construct_Click(object sender, EventArgs e)
        {
            
        }

        private void clear_Click(object sender, EventArgs e)
        {
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
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            Graphics g = Graphics.FromImage(pictureBox.Image);
            g.DrawEllipse(pen, new Rectangle(e.X, e.Y, 1, 1));
            g.Dispose();
            pictureBox.Invalidate();
        }
    }
}
