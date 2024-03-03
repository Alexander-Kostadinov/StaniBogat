using System;
using System.IO;
using System.Windows;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Forms.Integration;

namespace StaniBogat
{
    public partial class Form1 : Form
    {
        public int X;
        public int Y;
        public int Time;
        public int Count;
        public bool IsStarted;
        public bool IsUsedBtn1;
        public bool IsUsedBtn2;
        public string TimeText;
        public Bitmap BackScene;
        public bool IsTimeRunnig;
        public ElementHost Element;
        public TextBlock AnimationText;

        public Form1()
        {
            InitializeComponent();

            Count = 1;
            Time = 120;
            IsStarted = false;
            TimeText = "02:00";
            IsUsedBtn1 = false;
            IsUsedBtn2 = false;
            IsTimeRunnig = false;
            timer1.Interval = 1000;
            Element = new ElementHost();
            AnimationText = new TextBlock();

            var directory = Path.GetDirectoryName(Environment.CurrentDirectory);
            var path = directory.Replace("bin", "") + "Studio.bmp";
            BackScene = new Bitmap(path);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Width = (int)(Screen.PrimaryScreen.Bounds.Width / 1.25);
            this.Height = (int)(Screen.PrimaryScreen.Bounds.Height / 1.25);

            for (int i = 0; i < Controls.Count; i++)
            {
                Controls[i].Visible = false;
            }

            //ElementHost elementHost = new ElementHost();
            //elementHost.Width = this.Width;
            //elementHost.Height = this.Height;
            //elementHost.Location = new System.Drawing.Point(0, 0);

            //var directory = Path.GetDirectoryName(Environment.CurrentDirectory);
            //var path = directory.Replace("bin", "") + "logo.gif";
            //MediaElement gif = new MediaElement();
            //gif.LoadedBehavior = MediaState.Manual;
            //gif.UnloadedBehavior = MediaState.Manual;

            //if (File.Exists(path))
            //{
            //    gif.Source = new Uri(path);
            //    elementHost.Child = gif;
            //    Controls.Add(elementHost);
            //    gif.Play();

            //    gif.MediaEnded += (s, ev) =>
            //    {
            //        gif.Close();
            //        gif = null;
            //        elementHost.Dispose();
            //        Controls.Remove(elementHost);
            //        elementHost = null;
            //    };
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Time--;

            if (Time <= 0)
            {
                timer1.Stop();
            }

            if (Time == 180)
            {
                TimeText = "03:00";
            }
            else if (Time >= 120 && Time < 180)
            {
                if (Time < 130)
                {
                    TimeText = "02:" + '0' + (Time - 120);
                }
                else
                {
                    TimeText = "02:" + (Time - 120);
                }
            }
            else if (Time < 120 && Time >= 60)
            {
                if (Time < 70)
                {
                    TimeText = "01:" + '0' + (Time - 60);
                }
                else
                {
                    TimeText = "01:" + (Time - 60);
                }
            }
            else if (Time < 60)
            {
                if (Time < 10)
                {
                    TimeText = "00:" + '0' + Time;
                }
                else
                {
                    TimeText = "00:" + Time;
                }
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (IsStarted == false)
            {
                button1.Visible = true;

                //var animation = new DoubleAnimation
                //{
                //    From = 0.0,
                //    To = 1.0,
                //    Duration = TimeSpan.FromSeconds(2),
                //    AutoReverse = true,
                //    RepeatBehavior = RepeatBehavior.Forever,
                //};

                //Element.Dock = DockStyle.Top;
                //Element.Child = AnimationText;
                //Element.Width = this.Width / 2;
                //Element.Height = this.Height / 2;

                //AnimationText.FontSize = 42;
                //AnimationText.FontStyle = FontStyles.Italic;
                //AnimationText.TextAlignment = TextAlignment.Center;
                //AnimationText.VerticalAlignment = VerticalAlignment.Center;
                //AnimationText.Text = "Press the button to start the game !";
                //AnimationText.Foreground = System.Windows.Media.Brushes.LightGoldenrodYellow;
                //AnimationText.BeginAnimation(UIElement.OpacityProperty, animation);

                //button1.Location = new System.Drawing.Point((Width -
                //    button1.Width) / 2, (Height - button1.Height) / 2);

                //Controls.Add(Element);
            }
            if (IsStarted == true)
            {
                Pen pen = new Pen(Color.DarkBlue, 8);
                Brush brush = new SolidBrush(Color.Black);

                e.Graphics.DrawImage(BackScene, 0, 0, Width, Height);
                e.Graphics.DrawRectangle(pen, 10, 10, button5.Width, button5.Height);
                e.Graphics.DrawRectangle(pen, label10.Location.X 
                    - 20, 4, Width - label10.Location.X - 2, Height - 55);
                e.Graphics.FillRectangle(brush, label10.Location.X 
                    - 20, 4, Width - label10.Location.X - 2, Height - 55);
                e.Graphics.DrawEllipse(pen, ((label10.Location.X - 20)
                    / 2) - (TimeText.Length * 30) / 2 - 25, 10, 200, 100);
                e.Graphics.FillEllipse(brush, ((label10.Location.X - 20) 
                    / 2) - (TimeText.Length * 30) / 2 - 25, 10, 200, 100);
                brush = new SolidBrush(Color.White);
                e.Graphics.DrawString(TimeText, new Font("Arial", 32),
                    brush, new PointF(((label10.Location.X - 20) / 2) - (TimeText.Length * 30) / 2, 25));
                brush = new SolidBrush(Color.DarkGoldenrod);
                e.Graphics.FillRectangle(brush, X - 10, Y, label10.Width + 5, label10.Height);

                pen.Dispose();
                brush.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            IsStarted = true;
            Element.Dispose();
            Controls.Remove(Element);
            Controls.Remove(button1);
            Y = 125;

            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i].GetType() == typeof(System.Windows.Forms.Label))
                {
                    Controls[i].ForeColor = Color.White;
                    Controls[i].Font = new Font("Arial", 24);
                    Controls[i].BackColor = Color.Transparent;
                    Controls[i].Location = new System.Drawing.Point(
                        Width - (Controls[i].Width + 50), Y);
                    Y += Controls[i].Height + 15;
                }

                Controls[i].Visible = true;
            }

            X = label1.Location.X;
            Y = label1.Location.Y;
            label1.ForeColor = Color.Black;

            button4.ForeColor = Color.White;
            button4.BackColor = Color.DarkBlue;
            button4.Font = new Font("Arial", 20);
            button4.Location = new System.Drawing.Point(label10.Location.X + 10, 20);

            button3.Text = "Bonus\nTime";
            button3.ForeColor = Color.White;
            button3.BackColor = Color.DarkBlue;
            button3.Font = new Font("Arial", 20);
            button3.Location = new System.Drawing.Point(
                button4.Location.X + button4.Width + 25, 20);
            button4.Size = button3.Size;

            button2.ForeColor = Color.Black;
            button2.Font = new Font("Italic", 20);
            button2.BackColor = Color.DarkGoldenrod;
            button2.Height += 10;
            button2.Location = new System.Drawing.Point(X - 10, Y + 30 + label1.Height);

            button5.Text = "Best\nplayers";
            button5.ForeColor = Color.White;
            button5.BackColor = Color.Black;
            button5.Font = new Font("Arial", 16);
            button5.Width += 5;
            button5.Height += 15;
            button5.Location = new System.Drawing.Point(10, 10);

            Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            System.Windows.MessageBox.Show("Congratulations!\nYour game finished!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (IsUsedBtn2 == false)
            {
                IsUsedBtn2 = true;
                Time += 60;
                Refresh();
            }
        }

        private void button3_Paint(object sender, PaintEventArgs e)
        {
            if (IsUsedBtn2)
            {
                Pen pen = new Pen(Color.Red, 8);
                e.Graphics.DrawLine(pen, 0, button3.Height, button3.Width, 0);
                e.Graphics.DrawLine(pen, 0, 0, button3.Width, button3.Height);
            }
        }

        private void button4_Paint(object sender, PaintEventArgs e)
        {
            if (IsUsedBtn1)
            {
                Pen pen = new Pen(Color.Red, 8);
                e.Graphics.DrawLine(pen, 0, button4.Height, button4.Width, 0);
                e.Graphics.DrawLine(pen, 0, 0, button4.Width, button4.Height);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (IsUsedBtn1 == false)
            {
                IsUsedBtn1 = true;
                Refresh();
            }
        }
    }
}
