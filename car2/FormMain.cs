using System;
using System.Drawing;
using System.Media;
using System.Timers;
using System.Windows.Forms;

namespace car2
{
    public partial class FormMain : Form
    {
        private float CarX = 0;

        private float CarEndX = 750;

        private float CarSpeedX = 1;

        private float CarAccelerationX = 0.1f;

        private SoundPlayer soundPlayerEngine;

        private SoundPlayer soundPlayerBreaks;
        private float StopDistance = 3 / 4f;

        public FormMain()
        {
            InitializeComponent();
            timerUpdater.Elapsed += TimerUpdaterOnElapsed;
            timerUpdater.Interval = 16.6;

            soundPlayerEngine = new SoundPlayer();

            soundPlayerEngine.SoundLocation = @"D:\Samples\Au5_arise_sample_pack\Au5_vowel_synths\Au5_vowel_synth_gulp_D#.wav";

            soundPlayerEngine.Load();

            soundPlayerBreaks = new SoundPlayer();

            soundPlayerBreaks.SoundLocation = @"C:\Users\Admin\Downloads\ott_doing_ott_things.wav";

            soundPlayerBreaks.Load();
        }

        private bool isStopping = false;
        
        private void TimerUpdaterOnElapsed(object sender, ElapsedEventArgs e)
        {
            CarX += CarSpeedX;

            if (CarX >  CarEndX * StopDistance )
            {
                if (!isStopping)
                {
                    soundPlayerEngine.Stop();
                    soundPlayerBreaks.Play();
                }

                isStopping = true;
                CarSpeedX -= StopDistance / (1 - StopDistance) * CarAccelerationX;
                if (CarSpeedX < 0)
                {
                    CarX = CarEndX;
                }
            }
            else
            {
                CarSpeedX += CarAccelerationX;
            }

            if (CarX >= CarEndX)
            {
                CarX = CarEndX;
                timerUpdater.Stop();
            }

            pictureBoxCanvas.Refresh();
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Pen blackPen = new Pen(Brushes.Black, 5);

            DrawBackground(g, blackPen);
            DrawCar(g);
            DrawBrackets(g, 65, 50, 125, 50);

            Font font = new Font(DefaultFont.FontFamily, 16, FontStyle.Regular);
            g.DrawString("Artmed", font, Brushes.Black, new PointF(70, 45));

            DrawTree(g, pictureBoxCanvas.Width - 55, pictureBoxCanvas.Height / 2);
            DrawInfinitySign(g, blackPen, 650, 0, 100, 100);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public void DrawInfinitySign(Graphics g, Pen pen, float x, float y, float width, float height)
        {
            PointF pt1 = new PointF(x, y + height / 2);
            PointF pt2 = new PointF(x + width / 2, y);
            PointF pt3 = new PointF(x + width / 2, y + height);
            PointF pt4 = new PointF(x + width, y + height / 2);

            g.DrawBezier(pen, pt1, pt2, pt3, pt4);
            g.DrawBezier(pen, pt1, pt3, pt2, pt4);
        }

        private void DrawBackground(Graphics g, Pen pen)
        {
            //sky
            g.FillRectangle(Brushes.LightBlue, 0, 0, pictureBoxCanvas.Width, pictureBoxCanvas.Height / 2);

            //grass
            g.FillRectangle(Brushes.Green, 0, pictureBoxCanvas.Height / 2, pictureBoxCanvas.Width, pictureBoxCanvas.Height / 2);

            //road
            g.DrawLine(pen, 0, pictureBoxCanvas.Height / 2, pictureBoxCanvas.Width, pictureBoxCanvas.Height / 2);
        }

        public void DrawCar(Graphics g)
        {
            g.FillRectangle(Brushes.Red, CarX, pictureBoxCanvas.Height / 2 - 65, 50, 55);
            g.FillRectangle(Brushes.Brown, CarX + 50, pictureBoxCanvas.Height / 2 - 50, 20, 40);
            g.FillRectangle(Brushes.Black, CarX, pictureBoxCanvas.Height / 2 - 10, 70, 10);
        }


        public void DrawBrackets(Graphics g, float leftX, float leftY, float rightX, float rightY)
        {
            Pen pen = new Pen(Brushes.Black, 3);
            g.DrawArc(pen, leftX, leftY, 25, 25, 90, 180);
            g.DrawArc(pen, rightX, rightY, 25, 25, 270, 180);
        }

        private void DrawTree(Graphics g, int startX, int startY)
        {
            int x = startX;
            int y = startY - 15;

            Pen darkgreenPen = new Pen(Brushes.DarkGreen, 2);

            g.FillRectangle(Brushes.Brown, x - 4, y, 8, 15);

            //right
            g.DrawLine(darkgreenPen, x, y, x + 50, y);
            g.DrawLine(darkgreenPen, x + 50, y, x, y - 25);

            g.DrawLine(darkgreenPen, x, y - 25, x + 40, y - 25);
            g.DrawLine(darkgreenPen, x + 40, y - 25, x, y - 45);

            g.DrawLine(darkgreenPen, x, y - 45, x + 30, y - 45);
            g.DrawLine(darkgreenPen, x + 30, y - 45, x, y - 55);

            //left
            g.DrawLine(darkgreenPen, x, y, x - 50, y);
            g.DrawLine(darkgreenPen, x - 50, y, x, y - 25);
            g.DrawLine(darkgreenPen, x, y - 25, x - 40, y - 25);
            g.DrawLine(darkgreenPen, x - 40, y - 25, x, y - 45);
            g.DrawLine(darkgreenPen, x, y - 45, x - 30, y - 45);
            g.DrawLine(darkgreenPen, x - 30, y - 45, x, y - 55);

            //Lowest triangle
            g.FillPolygon(Brushes.DarkGreen, GenerateTrianglePoints(x, y - 25, 50, 25));
            g.FillPolygon(Brushes.DarkGreen, GenerateTrianglePoints(x, y - 45, 40, 20));
            g.FillPolygon(Brushes.DarkGreen, GenerateTrianglePoints(x, y - 55, 30, 10));
        }

        private PointF[] GenerateTrianglePoints(int topX, int topY, int width, int high)
        {
            PointF dotTop = new PointF(topX, topY);
            PointF dotLeft = new PointF(topX - width, topY + high);
            PointF dotRight = new PointF(topX + width, topY + high);

            return new[] {dotTop, dotLeft, dotRight};
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CarX = 0;
            CarAccelerationX = 0.1f;
            CarSpeedX = 1;
            isStopping = false;
            
            
            soundPlayerEngine.PlayLooping();

            timerUpdater.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CarX = 0;
            pictureBoxCanvas.Refresh();

            // float double int bool string char
            // uint, long, short, byte
        }
    }
}