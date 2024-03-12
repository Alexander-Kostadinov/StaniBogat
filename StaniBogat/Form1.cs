using System;
using System.IO;
using System.Windows;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Forms.Integration;
using System.Collections.Generic;

namespace StaniBogat
{
    public partial class Form1 : Form
    {
        public int X;
        public int Y;
        public int Time;
        public int Level;
        public int Count;
        public Form Form;
        public bool LoadGif;
        public Player Player;
        public bool IsStarted;
        public bool IsFinished;
        public bool IsUsedBtn1;
        public bool IsUsedBtn2;
        public int WaitSeconds;
        public string TimeText;
        public string Directory;
        public Bitmap BackScene;
        public Question Question;
        public bool IsClickedBtn6;
        public bool IsClickedBtn7;
        public bool IsClickedBtn8;
        public bool IsClickedBtn9;
        public ElementHost Element;
        public TextBlock AnimationText;
        public System.Windows.Forms.TextBox PlayerName;

        public Form1()
        {
            InitializeComponent();

            Level = 0;
            Time = 120;
            WaitSeconds = 3;
            LoadGif = false;
            Form = new Form();
            IsStarted = false;
            IsFinished = false;
            TimeText = "02:00";
            IsUsedBtn1 = false;
            IsUsedBtn2 = false;
            Player = new Player();
            timer1.Interval = 1000;
            timer2.Interval = 1000;
            Question = new Question();
            Element = new ElementHost();
            AnimationText = new TextBlock();
            Count = Controls.GetChildIndex(label1);
            PlayerName = new System.Windows.Forms.TextBox();

            Directory = Path.GetDirectoryName(Environment.CurrentDirectory);
            var path = Directory.Replace("bin", "") + "Studio.bmp";
            BackScene = new Bitmap(path);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;

            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i] == button1)
                {
                    continue;
                }

                Controls[i].Visible = false;
            }

            var animation = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(2),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
            };

            Element.Height = this.Height / 2;
            Element.Width = this.Width / 2;
            Element.Child = AnimationText;
            Element.Dock = DockStyle.Top;

            AnimationText.FontSize = 50;
            AnimationText.FontStyle = FontStyles.Italic;
            AnimationText.TextAlignment = TextAlignment.Center;
            AnimationText.VerticalAlignment = VerticalAlignment.Center;
            AnimationText.Text = "Press the button to start the game !";
            AnimationText.Foreground = System.Windows.Media.Brushes.LightGoldenrodYellow;
            AnimationText.BeginAnimation(UIElement.OpacityProperty, animation);

            button1.Location = new System.Drawing.Point((Width -
                button1.Width) / 2, (Height - button1.Height) / 2);

            Controls.Add(Element);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Time--;

            if (Time <= 0)
            {
                string win;

                if (Controls[Count] == label1)
                {
                    win = "0";
                }
                else
                {
                    var sum = Controls[++Count].Text.Split(' ');
                    win = sum[sum.Length - 3] + sum[sum.Length - 2] + sum[sum.Length - 1];
                }

                System.Windows.Forms.MessageBox.Show($"Sorry! " +
                    $"Answer time is over!\n Your win is {win} GBP !");

                FinishTheGame();
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
            if (LoadGif == true)
            {
                Element.Width = Width;
                Element.Height = Height;
                Element.Dock = DockStyle.None;
                Element.Location = new System.Drawing.Point(0, 0);

                var path = Directory.Replace("bin", "") + "logo.gif";
                MediaElement gif = new MediaElement();
                gif.LoadedBehavior = MediaState.Manual;
                gif.UnloadedBehavior = MediaState.Manual;

                if (File.Exists(path))
                {
                    Element.Child = gif;
                    Controls.Add(Element);
                    gif.Source = new Uri(path);
                    gif.MediaEnded += (s, ev) =>
                    {
                        gif.Close();
                        gif = null;
                        Element.Dispose();
                        Controls.Remove(Element);
                        Form.Show();
                    };
                    gif.Play();
                }

                LoadGif = false;
            }
            else if (IsStarted == true)
            {
                Pen pen = new Pen(Color.DarkBlue, 8);
                Brush brush = new SolidBrush(Color.Black);

                e.Graphics.DrawImage(BackScene, 0, 0, Width, Height);
                e.Graphics.DrawRectangle(pen, 10, 10, button5.Width, button5.Height);
                e.Graphics.DrawRectangle(pen, button10.Location.X, 10, button10.Width, button10.Height);
                e.Graphics.DrawRectangle(pen, label10.Location.X 
                    - 20, 4, Width - label10.Location.X - 2, Height - 55);
                e.Graphics.FillRectangle(brush, label10.Location.X 
                    - 20, 4, Width - label10.Location.X - 2, Height - 55);
                e.Graphics.DrawEllipse(pen, ((label10.Location.X - 20)
                    / 2) - TimeText.Length * (Height / 30) / 2 - (Height / 35),
                    10, (int)(Height / 4.3), (int)(Height / 8.6));
                e.Graphics.FillEllipse(brush, ((label10.Location.X - 20) 
                    / 2) - TimeText.Length * (Height / 30) / 2 - (Height / 35),
                    10, (int)(Height / 4.3), (int)(Height / 8.6));
                brush = new SolidBrush(Color.White);
                e.Graphics.DrawString(TimeText, new Font("Arial", Height / 27),
                    brush, new PointF(((label10.Location.X - 20) / 2) - TimeText.Length * (Height / 29) / 2, 25));
                brush = new SolidBrush(Color.Black);
                e.Graphics.DrawRectangle(pen, 100, (Height - Height / 7) / 2,
                    Width - (Width - label10.Location.X + 20) - 200, Height / 6);
                e.Graphics.FillRectangle(brush, 100, (Height - Height / 7) / 2,
                    Width - (Width - label10.Location.X + 20) - 200, Height / 6);
                brush = new SolidBrush(Color.White);

                if (Question.question.Length * (Height / 50) + 100 > label10.Location.X - 20)
                {
                    int idx = Question.question.Length / 2;

                    if (Question.question[idx] != ' ')
                    {
                        for (int i = idx; i < Question.question.Length; i++)
                        {
                            if (Question.question[i] == ' ')
                            {
                                idx = i;
                                break;
                            }
                        }
                    }

                    var text = Question.question.Insert(idx, "\n");
                    e.Graphics.DrawString(text, new Font("Arial", Height / 57),
                    brush, 120, (Height - Height / 7) / 2 + 20);

                }
                else
                {
                    e.Graphics.DrawString(Question.question, new Font("Arial", Height / 52),
                    brush, 120, (Height - Height / 7) / 2 + 20);
                }     

                pen.Dispose();
                brush.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AnimationText = null;
            Controls.Remove(button1);
            Controls.Remove(Element);

            Form.Size = new System.Drawing.Size(550, 375);
            Form.Controls.Add(PlayerName);
            PlayerName.Font = new Font("Italic", 14);
            PlayerName.Width = Form.Width / 2;
            Form.StartPosition = FormStartPosition.CenterScreen;
            PlayerName.Location = new System.Drawing.Point((Form.Width
                - PlayerName.Width) / 2, (Form.Height - PlayerName.Height) / 2 - 25);

            System.Windows.Forms.Button button = new System.Windows.Forms.Button();
            Form.Controls.Add(button);
            button.Click += Button_Click;
            button.Size = new System.Drawing.Size(200, 75);
            button.Font = new Font("Arial", 20);
            button.Text = "Start";
            button.Location = new System.Drawing.Point((Form.Width
                - button.Width) / 2, PlayerName.Location.Y + PlayerName.Height + 20);

            var label = new System.Windows.Forms.Label();
            Form.Controls.Add(label);
            label.AutoSize = true;
            label.Font = new Font("Italic", 24);
            label.Text = "Please, enter player name!";
            label.Location = new System.Drawing.Point((Form.Width - label.Width) / 2, 80);

            LoadGif = true;
            Refresh();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (PlayerName.Text == "")
            {
                System.Windows.MessageBox.Show(
                    "Please enter player name before start the game!");
                return;
            }

            Form.Close();
            Form.Dispose();
            timer1.Start();
            IsStarted = true;
            Controls.Remove(Element);
            Controls.Remove(button1);
            Player.Name = PlayerName.Text;
            Y = Height / 7 + 20;

            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i].GetType() == typeof(System.Windows.Forms.Label))
                {
                    Controls[i].ForeColor = Color.White;
                    Controls[i].Font = new Font("Arial", Height / 39);
                    Controls[i].BackColor = Color.Transparent;
                    Controls[i].Location = new System.Drawing.Point(
                        Width - (Controls[i].Width + 50), Y);
                    Y += Controls[i].Height + Height / 57;
                }

                Controls[i].Visible = true;
            }

            X = label1.Location.X;
            Y = label1.Location.Y;
            label1.ForeColor = Color.Black;
            label1.BackColor = Color.DarkGoldenrod;

            button4.ForeColor = Color.White;
            button4.BackColor = Color.DarkBlue;
            button4.Font = new Font("Arial", Height / 43);
            button4.Location = new System.Drawing.Point(label10.Location.X, Height / 43);

            button3.Text = "Bonus\nTime";
            button3.ForeColor = Color.White;
            button3.BackColor = Color.DarkBlue;
            button3.Font = new Font("Arial", Height / 43);
            button3.Location = new System.Drawing.Point(
                button4.Location.X + button4.Width + 25, Height / 43);
            button4.Size = button3.Size;

            button2.ForeColor = Color.Black;
            button2.Font = new Font("Italic", Height / 43);
            button2.BackColor = Color.DarkGoldenrod;
            button2.Height += Height / 86;
            button2.Location = new System.Drawing.Point(X + (label1.Width - button2.Width) / 2,
                label1.Location.Y + label1.Height + Height / 30);

            button5.Text = "Best\nplayers";
            button5.ForeColor = Color.White;
            button5.BackColor = Color.Black;
            button5.Font = new Font("Arial", Height / 54);
            button5.Width += 5;
            button5.Height += 15;
            button5.Location = new System.Drawing.Point(10, 10);

            button6.Text = "А - ";
            button6.BackColor = Color.Black;
            button6.ForeColor = Color.White;
            button6.Font = new Font("Arial", Height / 59);
            button6.TextAlign = ContentAlignment.MiddleLeft;
            button6.Size = new System.Drawing.Size((Width - (Width
                - label10.Location.X + 20) - 200) / 2 - 10, Height / 9);
            button6.Location = new System.Drawing.Point(100,
                (Height - Height / 6) / 2 + Height / 6 + Height / 30);

            button7.Text = "Б - ";
            button7.BackColor = Color.Black;
            button7.ForeColor = Color.White;
            button7.Font = new Font("Arial", Height / 59);
            button7.TextAlign = ContentAlignment.MiddleLeft;
            button7.Size = button6.Size;
            button7.Location = new System.Drawing.Point(100 + button6.Width +
                20, (Height - Height / 6) / 2 + Height / 6 + Height / 30);

            button8.Text = "В - ";
            button8.BackColor = Color.Black;
            button8.ForeColor = Color.White;
            button8.Font = new Font("Arial", Height / 59);
            button8.TextAlign = ContentAlignment.MiddleLeft;
            button8.Size = button6.Size;
            button8.Location = new System.Drawing.Point(100,
                button6.Location.Y + button6.Height + 20);

            button9.Text = "Г - ";
            button9.BackColor = Color.Black;
            button9.ForeColor = Color.White;
            button9.Font = new Font("Arial", Height / 59);
            button9.TextAlign = ContentAlignment.MiddleLeft;
            button9.Size = button6.Size;
            button9.Location = new System.Drawing.Point(100 +
                button8.Width + 20, button7.Location.Y + button7.Height + 20);

            button10.Size = button5.Size;
            button10.ForeColor = Color.White;
            button10.BackColor = Color.Black;
            button10.Font = new Font("Arial", Height / 54);
            button10.Location = new System.Drawing.Point(
                label10.Location.X - 50 - button10.Width, 10);

            SelectQuestion();
            Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string win;

            if (Controls[Count] == label1)
            {
                win = "0";
            }
            else
            {
                var sum = Controls[++Count].Text.Split(' ');
                win = sum[sum.Length - 3] + sum[sum.Length - 2] + sum[sum.Length - 1];
            }

            System.Windows.Forms.MessageBox.Show($"Congratulations," +
                $" the game is finished! \nYour win is {win} GBP !");

            FinishTheGame();
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
                Random random = new Random();

                if (button6.Text == "А - " + Question.correct_answer)
                {
                    var buttons = new List<System.Windows.Forms.Button>()
                    { button6, button7, button8, button9 };
                    int index = random.Next(1, 3);
                    buttons[index].Text = "";
                    buttons.RemoveAt(index);
                    index = random.Next(1, 2);
                    buttons[index].Text = "";
                }
                else if (button7.Text == "Б - " + Question.correct_answer)
                {
                    var buttons = new List<System.Windows.Forms.Button>()
                    { button7, button6, button8, button9 };
                    int index = random.Next(1, 3);
                    buttons[index].Text = "";
                    buttons.RemoveAt(index);
                    index = random.Next(1, 2);
                    buttons[index].Text = "";
                }
                else if (button8.Text == "В - " + Question.correct_answer)
                {
                    var buttons = new List<System.Windows.Forms.Button>()
                    { button8, button6, button7, button9 };
                    int index = random.Next(1, 3);
                    buttons[index].Text = "";
                    buttons.RemoveAt(index);
                    index = random.Next(1, 2);
                    buttons[index].Text = "";
                }
                else if (button9.Text == "Г - " + Question.correct_answer)
                {
                    var buttons = new List<System.Windows.Forms.Button>()
                    { button9, button6, button7, button8 };
                    int index = random.Next(1, 3);
                    buttons[index].Text = "";
                    buttons.RemoveAt(index);
                    index = random.Next(1, 2);
                    buttons[index].Text = "";
                }

                IsUsedBtn1 = true;
                Refresh();
            }
        }

        private void SelectQuestion()
        {
            if (Level > 9)
            {
                return;
            }

            var path = Directory.Replace("bin", "") + "Questions.txt";
            var text = File.ReadAllText(path);

            var questionLevels = JArray.Parse(text);
            var questions = questionLevels[Level].ToString();
            var questionLevel = JsonConvert.DeserializeObject<QuestionLevel>(questions);

            Random random = new Random();
            Question = questionLevel.questions[random.Next(0, questionLevel.questions.Count - 1)];

            var num = random.Next(1, 4);

            if (num == 1)
            {
                button6.Text += Question.correct_answer;
                button9.Text += Question.wrong_answers[0];
                button7.Text += Question.wrong_answers[1];
                button8.Text += Question.wrong_answers[2];
            }
            else if (num == 2)
            {
                button7.Text += Question.correct_answer;
                button6.Text += Question.wrong_answers[0];
                button9.Text += Question.wrong_answers[1];
                button8.Text += Question.wrong_answers[2];
            }
            else if (num == 3)
            {
                button8.Text += Question.correct_answer;
                button6.Text += Question.wrong_answers[0];
                button7.Text += Question.wrong_answers[1];
                button9.Text += Question.wrong_answers[2];
            }
            else
            {
                button9.Text += Question.correct_answer;
                button6.Text += Question.wrong_answers[0];
                button7.Text += Question.wrong_answers[1];
                button8.Text += Question.wrong_answers[2];
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Level > 9)
            {
                return;
            }

            timer1.Stop();
            timer2.Start();
            IsClickedBtn6 = true;
            button6.ForeColor = Color.Black;
            button6.BackColor = Color.DarkGoldenrod;

            Refresh();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (Level > 9)
            {
                return;
            }

            timer1.Stop();
            timer2.Start();
            IsClickedBtn7 = true;
            button7.ForeColor = Color.Black;
            button7.BackColor = Color.DarkGoldenrod;

            Refresh();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (Level > 9)
            {
                return;
            }

            timer1.Stop();
            timer2.Start();
            IsClickedBtn8 = true;
            button8.ForeColor = Color.Black;
            button8.BackColor = Color.DarkGoldenrod;

            Refresh();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (Level > 9)
            {
                return;
            }

            timer1.Stop();
            timer2.Start();
            IsClickedBtn9 = true;
            button9.ForeColor = Color.Black;
            button9.BackColor = Color.DarkGoldenrod;

            Refresh();
        }

        private void ClearAnswerButtons()
        {
            button6.Text = "А - ";
            button7.Text = "Б - ";
            button8.Text = "В - ";
            button9.Text = "Г - ";

            button6.ForeColor = Color.White;
            button6.BackColor = Color.Black;
            button7.ForeColor = Color.White;
            button7.BackColor = Color.Black;
            button8.ForeColor = Color.White;
            button8.BackColor = Color.Black;
            button9.ForeColor = Color.White;
            button9.BackColor = Color.Black;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsStarted == true)
            {
                string messageBoxText = "If you close the game, " +
                "\nall progress will be lost!";
                string caption = "Closing the game!";
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxButton button = MessageBoxButton.OKCancel;

                MessageBoxResult result;
                result = System.Windows.MessageBox.Show(messageBoxText,
                    caption, button, icon, MessageBoxResult.Yes);

                if (result != MessageBoxResult.OK)
                {
                    e.Cancel = true;
                }
            }
            else
            {
                Close();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            WaitSeconds--;

            if (WaitSeconds == 1)
            {
                Controls[Count].ForeColor = Color.White;
                Controls[Count].BackColor = Color.Transparent;

                if (IsClickedBtn6)
                {
                    IsClickedBtn6 = false;

                    if (button6.Text == "А - " + Question.correct_answer)
                    {
                        button6.BackColor = Color.Green;
                        button6.ForeColor = Color.Black;
                        Level++; Count--;
                        timer1.Start();
                        Time = 120;
                    }
                    else
                    {
                        IsFinished = true;
                        button6.BackColor = Color.Red;
                        button6.ForeColor = Color.Black;
                    }

                    Refresh();
                }
                else if (IsClickedBtn7)
                {
                    IsClickedBtn7 = false;

                    if (button7.Text == "Б - " + Question.correct_answer)
                    {
                        button7.BackColor = Color.Green;
                        button7.ForeColor = Color.Black;
                        Level++; Count--;
                        timer1.Start();
                        Time = 120;
                    }
                    else
                    {
                        IsFinished = true;
                        button7.BackColor = Color.Red;
                        button7.ForeColor = Color.Black;
                    }

                    Refresh();
                }
                else if (IsClickedBtn8)
                {
                    IsClickedBtn8 = false;

                    if (button8.Text == "В - " + Question.correct_answer)
                    {
                        button8.BackColor = Color.Green;
                        button8.ForeColor = Color.Black;
                        Level++; Count--;
                        timer1.Start();
                        Time = 120;
                    }
                    else
                    {
                        IsFinished = true;
                        button8.BackColor = Color.Red;
                        button8.ForeColor = Color.Black;
                    }

                    Refresh();
                }
                else if (IsClickedBtn9)
                {
                    IsClickedBtn9 = false;

                    if (button9.Text == "Г - " + Question.correct_answer)
                    {
                        button9.BackColor = Color.Green;
                        button9.ForeColor = Color.Black;
                        Level++; Count--;
                        timer1.Start();
                        Time = 120;
                    }
                    else
                    {
                        IsFinished = true;
                        button9.BackColor = Color.Red;
                        button9.ForeColor = Color.Black;
                    }

                    Refresh();
                }
            }
            else if (WaitSeconds == 0)
            {
                timer2.Stop();
                WaitSeconds = 3;
                ClearAnswerButtons();
                Controls[Count].ForeColor = Color.Black;
                Controls[Count].BackColor = Color.DarkGoldenrod;

                if (IsFinished == false)
                {
                    SelectQuestion();
                }
                else
                {
                    FinishTheGame();

                    var win = "";

                    if (Controls[Count] == label1)
                    {
                        win = "0";
                    }
                    else
                    {
                        var sum = Controls[++Count].Text.Split(' ');
                        win = sum[sum.Length - 3] + sum[sum.Length - 2] + sum[sum.Length - 1];
                    }

                    System.Windows.Forms.MessageBox.Show($"Wrong answer!\n" +
                        $" Your win is {win} GBP !");
                }             
            }

            Refresh();
        }

        private void FinishTheGame()
        {
            timer1.Stop();
            Question.question = "";

            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i] == button10)
                {
                    continue;
                }
                else if (i == Count)
                {
                    Controls[i].ForeColor = Color.White;
                    Controls[i].BackColor = Color.Transparent;
                }

                Controls[i].Enabled = false;
            }

            Refresh();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBoxResult result;
            string caption = "Start new game!";
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxButton button = MessageBoxButton.YesNo;
            string messageBoxText = "Do you want to start new game?";

            result = System.Windows.MessageBox.Show(messageBoxText,
                caption, button, icon, MessageBoxResult.Yes);

            if (result == MessageBoxResult.Yes)
            {
                Level = 0;
                Time = 120;
                timer1.Start();
                WaitSeconds = 3;
                IsUsedBtn1 = false;
                IsUsedBtn2 = false;
                IsFinished = false;
                IsClickedBtn6 = false;
                IsClickedBtn7 = false;
                IsClickedBtn8 = false;
                IsClickedBtn9 = false;
                label1.ForeColor = Color.Black;
                label1.BackColor = Color.DarkGoldenrod;
                Count = Controls.GetChildIndex(label1);

                for (int i = 0; i < Controls.Count; i++)
                {
                    Controls[i].Enabled = true;
                }

                SelectQuestion();
                Refresh();
            }
        }
    }
}
