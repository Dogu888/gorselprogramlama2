using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp091025DKKO
{
    public partial class Form1 : Form
    {
        private Point btn1Start, btn2Start, btn3Start, btn4Start;
        private Point btn1Target, btn2Target, btn3Target, btn4Target;
        private Timer animationTimer;
        private int step = 0;
        private bool goingToCenter = true;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            btn1Start = button1.Location;
            btn2Start = button2.Location;
            btn3Start = button3.Location;
            btn4Start = button4.Location;

           
            int centerX = (this.ClientSize.Width - button1.Width) / 2;
            int centerY = (this.ClientSize.Height - button1.Height) / 2;

            int half = button1.Width / 2; 

            btn1Target = new Point(centerX - half, centerY - button1.Height - 2); 
            btn2Target = new Point(centerX - half, centerY + button1.Height + 2 - button1.Height); 
            btn3Target = new Point(centerX + half, centerY - button1.Height - 2); 
            btn4Target = new Point(centerX + half, centerY + button1.Height + 2 - button1.Height); 

            
            animationTimer = new Timer();
            animationTimer.Interval = 20;
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            step++;
            int totalSteps = 50;
            float t = (float)step / totalSteps;

            if (goingToCenter)
            {
                button1.Location = Lerp(btn1Start, btn1Target, t);
                button2.Location = Lerp(btn2Start, btn2Target, t);
                button3.Location = Lerp(btn3Start, btn3Target, t);
                button4.Location = Lerp(btn4Start, btn4Target, t);

                if (step >= totalSteps)
                {
                    step = 0;
                    goingToCenter = false;
                }
            }
            else
            {
                button1.Location = Lerp(btn1Target, btn1Start, t);
                button2.Location = Lerp(btn2Target, btn2Start, t);
                button3.Location = Lerp(btn3Target, btn3Start, t);
                button4.Location = Lerp(btn4Target, btn4Start, t);

                if (step >= totalSteps)
                {
                    step = 0;
                    goingToCenter = true;
                }
            }
        }

        private Point Lerp(Point start, Point end, float t)
        {
            if (t > 1f) t = 1f;
            int x = (int)(start.X + (end.X - start.X) * t);
            int y = (int)(start.Y + (end.Y - start.Y) * t);
            return new Point(x, y);
        }
    }
}
