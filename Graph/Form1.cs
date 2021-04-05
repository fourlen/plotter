using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graph
{
    public partial class Form1 : Form
    {
        private List<PointF> points = new List<PointF>();
        private List<float[]> pointsf = new List<float[]>();
        private IExpression ixp = null;
        float zoom = 1;
        private Color[] colors = new Color[] { Color.Black, Color.Red, Color.Green, Color.Blue };
        public Form1()
        {
            InitializeComponent();
        }

        public void GetIexp(string func)
        {
            Parser parser = new Parser(func);
            ixp = parser.Parse();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        public float FromNumToPixX(double x)
        {
            return (float)(ClientSize.Width / 2 + (float)ClientSize.Width / 20 * x);
        }
        public float FromNumToPixY(double y)
        {
            return (float)(ClientSize.Height / 2 + 100 - (float)(ClientSize.Height - 200) / 20 * y);
        }

        public void MouseScrollEvent(object sender, MouseEventArgs e)
        {
            zoom += (float)(e.Delta) / 1200;
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var p = new Pen(colors[0], 3);
            var g = e.Graphics;
            g.DrawLine(p, ClientSize.Width / 2, 200, ClientSize.Width / 2, ClientSize.Height + 200);
            g.DrawLine(p, 0, (ClientSize.Height + 200) / 2, ClientSize.Width, (ClientSize.Height + 200) / 2);
            g.DrawLine(p, ClientSize.Width / 2, 200, ClientSize.Width / 2 - 10, 20 + 200);
            g.DrawLine(p, ClientSize.Width / 2, 200, ClientSize.Width / 2 + 10, 20 + 200);
            g.DrawLine(p, ClientSize.Width, (ClientSize.Height + 200) / 2, ClientSize.Width - 20, (ClientSize.Height + 200) / 2 - 10);
            g.DrawLine(p, ClientSize.Width, (ClientSize.Height + 200) / 2, ClientSize.Width - 20, (ClientSize.Height + 200)/ 2 + 10);
            var pSetka = new Pen(colors[0], 1);
            for (float i = 0; i < 10 / zoom; i++)
            {
                //g.DrawLine(pSetka, 0, 200 + (ClientSize.Height - 200) / 20 * i * zoom, ClientSize.Width, 200 + (ClientSize.Height - 200) / 20 * i * zoom);
                //g.DrawLine(pSetka, ClientSize.Width / 20 * i * zoom, 200, ClientSize.Width / 20 * i * zoom, ClientSize.Height);
                g.DrawLine(pSetka, 0, FromNumToPixY((i) * zoom), ClientSize.Width, FromNumToPixY((i) * zoom));
                g.DrawLine(pSetka, FromNumToPixX((i) * zoom), 200, FromNumToPixX((i) * zoom), ClientSize.Height);
                g.DrawLine(pSetka, 0, FromNumToPixY(-(i) * zoom), ClientSize.Width, FromNumToPixY(-(i) * zoom));
                g.DrawLine(pSetka, FromNumToPixX(-(i) * zoom), 200, FromNumToPixX(-(i) * zoom), ClientSize.Height);
            }
            //string function = "x";
            //Parser parser = new Parser(function);
            //IExpression ixp = parser.Parse();
            /*if (ixp != null)
            {
                for (float i = -10f; i < 10; i += 0.01f)
                {
                    double f1 = ixp.Calculate();
                    NumbersSingleton.GetInstance().Shift();
                    double f2 = ixp.Calculate();
                    if (f1 <= 10 && f2 <= 10)
                    {
                        g.DrawLine(p, FromNumToPixX(i), FromNumToPixY(f1), FromNumToPixX(i + 0.01), FromNumToPixY(f2));
                    }
                }
            }*/
            var pGraph = new Pen(colors[comboBox1.SelectedIndex], 3);
            if (ixp != null && pointsf.Count != 0)
            {
                for (int i = 1; i < pointsf.Count; i++)
                {
                    if (FromNumToPixY(pointsf[i - 1][1] * zoom) >= 225f) 
                    {
                        g.DrawLine(pGraph, FromNumToPixX(pointsf[i - 1][0] * zoom), FromNumToPixY(pointsf[i - 1][1] * zoom), FromNumToPixX(pointsf[i][0] * zoom), FromNumToPixY(pointsf[i - 1][1] * zoom));
                    }
                    
                }
            }
            NumbersSingleton.GetInstance().Reload();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MouseWheel += MouseScrollEvent;
            comboBox1.SelectedIndex = 0;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                pointsf.Clear();
                GetIexp(textBox2.Text);
                CalulatePoints();
                Refresh();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Expression makes no sense");
            }
        }

        private void CalulatePoints()
        {
            for (float i = -10f; i < 10; i += 0.01f)
            {
                double f1 = ixp.Calculate();
                NumbersSingleton.GetInstance().Shift();
                double f2 = ixp.Calculate();
                if (f1 <= 10 && f2 <= 10)
                {
                    float[] point1 = { i, (float)(f1) };
                    float[] point2 = { (float)(i + 0.01), (float)(f2) };
                    //PointF p1 = new PointF(FromNumToPixX(i), FromNumToPixY(f1));
                    //PointF p2 = new PointF(FromNumToPixX(i + 0.01), FromNumToPixY(f2));
                    //points.Add(p1);
                    //points.Add(p2);
                    pointsf.Add(point1);
                    pointsf.Add(point2);
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        public void GeneratePicture()
        {
            /*plot = new Bitmap(ClientSize.Width, ClientSize.Height);
            var g = Graphics.FromImage(plot);
            var p = new Pen(Color.Black, 3);
            g.DrawLine(p, ClientSize.Width / 2, 200, ClientSize.Width / 2, ClientSize.Height + 200);
            g.DrawLine(p, 0, (ClientSize.Height + 200) / 2, ClientSize.Width, (ClientSize.Height + 200) / 2);
            g.DrawLine(p, ClientSize.Width / 2, 200, ClientSize.Width / 2 - 10, 20 + 200);
            g.DrawLine(p, ClientSize.Width / 2, 200, ClientSize.Width / 2 + 10, 20 + 200);
            g.DrawLine(p, ClientSize.Width, (ClientSize.Height + 200) / 2, ClientSize.Width - 20, (ClientSize.Height + 200) / 2 - 10);
            g.DrawLine(p, ClientSize.Width, (ClientSize.Height + 200) / 2, ClientSize.Width - 20, (ClientSize.Height + 200) / 2 + 10);
            var pSetka = new Pen(Color.Black, 1);
            for (int i = 0; i < 20; i++)
            {
                g.DrawLine(pSetka, 0, 200 + (ClientSize.Height - 200) / 20 * i, ClientSize.Width, 200 + (ClientSize.Height - 200) / 20 * i);
                g.DrawLine(pSetka, ClientSize.Width / 20 * i, 200, ClientSize.Width / 20 * i, ClientSize.Height);
            }
            //string function = "x";
            //Parser parser = new Parser(function);
            //IExpression ixp = parser.Parse();
            if (ixp != null)
            {
                for (float i = -10f; i < 10; i += 0.01f)
                {
                    double f1 = ixp.Calculate();
                    NumbersSingleton.GetInstance().Shift();
                    double f2 = ixp.Calculate();
                    if (f1 <= 10 && f2 <= 10)
                    {
                        g.DrawLine(p, FromNumToPixX(i), FromNumToPixY(f1), FromNumToPixX(i + 0.01), FromNumToPixY(f2));
                    }
                }
            }
            NumbersSingleton.GetInstance().Reload();
            pictureBox1.Image = plot;*/
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
