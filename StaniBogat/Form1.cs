using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using System.Windows.Forms.Integration;

namespace StaniBogat
{
    public partial class Form1 : Form
    {
        public int X;
        public int Y;
        public int Time;
        public int Level;
        public int Count;
        public bool LoadGif;
        public Player Player;
        public int PlayedTime;
        public bool IsStarted;
        public bool IsFinished;
        public bool IsUsedBtn1;
        public bool IsUsedBtn2;
        public bool IsUsedBtn3;
        public int WaitSeconds;
        public string TimeText;
        public string Directory;
        public Bitmap BackScene;
        public Question Question;
        public int QuestionIndex;
        public int QuestionCount;
        public bool IsClickedBtn6;
        public bool IsClickedBtn7;
        public bool IsClickedBtn8;
        public bool IsClickedBtn9;
        public ElementHost Element;
        public Form PlayerNameForm;
        public TextBlock AnimationText;
        public System.Windows.Forms.TextBox PlayerName;
        public System.Windows.Media.MediaPlayer StartAudio;
        public System.Windows.Media.MediaPlayer AudioSound;
        public System.Windows.Media.MediaPlayer QuestionSound;
        public System.Windows.Media.MediaPlayer WrongAnswerSound;
        public System.Windows.Media.MediaPlayer CorrectAnswerSound;

        public Form1()
        {
            InitializeComponent();

            Level = 0;
            Time = 60;
            PlayedTime = 0;
            WaitSeconds = 5;
            LoadGif = false;
            IsStarted = false;
            IsFinished = false;
            TimeText = "01:00";
            IsUsedBtn1 = false;
            IsUsedBtn2 = false;
            IsUsedBtn3 = false;
            Player = new Player();
            timer1.Interval = 1000;
            timer2.Interval = 1000;
            Question = new Question();
            Element = new ElementHost();
            PlayerNameForm = new Form();
            AnimationText = new TextBlock();
            Count = Controls.GetChildIndex(label1);
            PlayerName = new System.Windows.Forms.TextBox();
            StartAudio = new System.Windows.Media.MediaPlayer();
            AudioSound = new System.Windows.Media.MediaPlayer();
            QuestionSound = new System.Windows.Media.MediaPlayer();
            WrongAnswerSound = new System.Windows.Media.MediaPlayer();
            CorrectAnswerSound = new System.Windows.Media.MediaPlayer();
            Directory = Path.GetDirectoryName(Environment.CurrentDirectory);

            var path = Directory.Replace("bin", "") + "Studio.jpg";
            BackScene = new Bitmap(path);
            path = Directory.Replace("bin", "") + "start audio.mp3";
            StartAudio.Open(new Uri(path));
            path = Directory.Replace("bin", "") + "audio sound.mp3";
            AudioSound.Open(new Uri(path));
            path = Directory.Replace("bin", "") + "wrong answer.mp3";
            WrongAnswerSound.Open(new Uri(path));
            path = Directory.Replace("bin", "") + "correct answer.mp3";
            CorrectAnswerSound.Open(new Uri(path));
            path = Directory.Replace("bin", "") + "question sound.mp3";
            QuestionSound.Open(new Uri(path));
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
            PlayedTime++;

            if (Time <= 0)
            {
                FinishTheGame();
                return;
            }
            else if (Time == 120)
            {
                TimeText = "02:00";
            }
            else if (Time < 120 && Time >= 60)
            {
                if (Time == 117)
                {
                    AudioSound.Play();
                }

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
                if (Time == 57)
                {
                    AudioSound.Play();
                }

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
                        PlayerNameForm.Show();
                        StartAudio.Stop();
                    };
                    StartAudio.Play();
                    gif.Play();
                }

                LoadGif = false;
            }
            else if (IsStarted == true)
            {
                Pen pen = new Pen(Color.MidnightBlue, 8);
                Brush brush = new SolidBrush(Color.Black);

                e.Graphics.DrawImage(BackScene, 0, 0, label10.Location.X - 20, Height);
                e.Graphics.DrawRectangle(pen, label10.Location.X
                    - (Height / 17), 4, Width - label10.Location.X + 50, Height - 55);
                e.Graphics.DrawRectangle(pen, button5.Location.X,
                    button5.Location.Y, button5.Width, button5.Height);
                e.Graphics.DrawRectangle(pen, button10.Location.X,
                    button10.Location.Y, button10.Width, button10.Height);
                e.Graphics.FillRectangle(brush, label10.Location.X
                    - (Height / 17), 4, Width - label10.Location.X + 50, Height - 55);
                e.Graphics.DrawEllipse(pen, ((label10.Location.X - 50) / 2) - 5
                    * (Height / 30) / 2 - (Height / 35), (Height - Height / 7 - (Height / 17))
                    / 2 - (Height / 7), (int)(Height / 4.3), (int)(Height / 8.6));
                e.Graphics.FillEllipse(brush, ((label10.Location.X - 50) / 2) - 5 
                    * (Height / 30) / 2 - (Height / 35), (Height - Height / 7 - (Height / 17))
                    / 2 - (Height / 7), (int)(Height / 4.3), (int)(Height / 8.6));
                brush = new SolidBrush(Color.White);
                e.Graphics.DrawString(TimeText, new Font("Arial", Height / 27),
                    brush, new PointF(((label10.Location.X - 50) / 2) - TimeText.Length
                    * (Height / 29) / 2, (Height - Height / 7 - (Height / 18)) / 2 - (Height / 8)));
                brush = new SolidBrush(Color.Black);
                e.Graphics.DrawRectangle(pen, Width / 25, (Height - Height / 7 - (Height / 15)) / 2,
                    Width - (Width - label10.Location.X + 20) - (Height / 6), Height / 6 + (Height / 17));
                e.Graphics.FillRectangle(brush, Width / 25, (Height - Height / 7 - (Height / 15)) / 2,
                    Width - (Width - label10.Location.X + 20) - (Height / 6), Height / 6 + (Height / 17));
                brush = new SolidBrush(Color.White);
                e.Graphics.DrawString(FormatText(Question.question),
                    new Font("Arial", Height / 65), brush, new System.Drawing.Point(
                        90, (Height - Height / 7 - 35) / 2));

                pen.Dispose();
                brush.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AnimationText = null;
            Controls.Remove(button1);
            Controls.Remove(Element);

            PlayerNameForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            PlayerNameForm.MaximizeBox = false;
            PlayerNameForm.MinimizeBox = false;
            PlayerNameForm.Size = new System.Drawing.Size(550, 375);
            PlayerNameForm.Controls.Add(PlayerName);
            PlayerName.Font = new Font("Italic", 14);
            PlayerName.Width = PlayerNameForm.Width / 2;
            PlayerNameForm.StartPosition = FormStartPosition.CenterScreen;
            PlayerName.Location = new System.Drawing.Point((PlayerNameForm.Width
                - PlayerName.Width) / 2, (PlayerNameForm.Height - PlayerName.Height) / 2 - 25);

            System.Windows.Forms.Button button = new System.Windows.Forms.Button();
            PlayerNameForm.Controls.Add(button);
            button.Click += Button_Click;
            button.Size = new System.Drawing.Size(200, 75);
            button.Font = new Font("Arial", 20);
            button.Text = "Start";
            button.Location = new System.Drawing.Point((PlayerNameForm.Width
                - button.Width) / 2, PlayerName.Location.Y + PlayerName.Height + 20);

            var label = new System.Windows.Forms.Label();
            PlayerNameForm.Controls.Add(label);
            label.AutoSize = true;
            label.Font = new Font("Italic", 24);
            label.Text = "Please, enter player name!";
            label.Location = new System.Drawing.Point((PlayerNameForm.Width - label.Width) / 2, 80);

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

            Player.Name = PlayerName.Text;

            PlayerNameForm.Close();
            PlayerNameForm.Dispose();
            timer1.Start();
            IsStarted = true;
            Controls.Remove(Element);
            Controls.Remove(button1);
            Y = Height / 7 + Height / 43;

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
            label1.BackColor = Color.Goldenrod;

            button3.Size = button4.Size = button11.Size = 
                new System.Drawing.Size((int)(Height / 8.5), Height / 10);
            button4.ForeColor = Color.White;
            button4.BackColor = Color.MidnightBlue;
            button4.Font = new Font("Arial", Height / 65);
            button4.Location = new System.Drawing.Point(
                label10.Location.X - (Height / 17) + (Height / 86), Height / 43);

            button3.Text = "Bonus\ntime";
            button3.ForeColor = Color.White;
            button3.BackColor = Color.MidnightBlue;
            button3.Font = new Font("Arial", Height / 65);
            button3.Location = new System.Drawing.Point(
                button4.Location.X + button4.Width + 5, Height / 43);

            button11.Text = "Change\nquestion";
            button11.ForeColor = Color.White;
            button11.BackColor = Color.MidnightBlue;
            button11.Font = new Font("Arial", Height / 65);
            button11.Location = new System.Drawing.Point(
                button3.Location.X + button3.Width + 5, Height / 43);

            button2.ForeColor = Color.Black;
            button2.Font = new Font("Italic", Height / 43);
            button2.BackColor = Color.Goldenrod;
            button2.Height += Height / 86;
            button2.Location = new System.Drawing.Point(X - 5 + (label1.Width - button2.Width) / 2,
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
            button6.Font = new Font("Arial", Height / 70);
            button6.TextAlign = ContentAlignment.MiddleLeft;
            button6.Size = new System.Drawing.Size((Width - (Width
                - label10.Location.X + 5) - (Height / 5)) / 2, Height / 7);
            button6.Location = new System.Drawing.Point(Width / 25,
                (Height - Height / 6) / 2 + Height / 7 + Height / 13);

            button7.Text = "Б - ";
            button7.BackColor = Color.Black;
            button7.ForeColor = Color.White;
            button7.Font = new Font("Arial", Height / 70);
            button7.TextAlign = ContentAlignment.MiddleLeft;
            button7.Size = button6.Size;
            button7.Location = new System.Drawing.Point(Width / 25 + button6.Width +
                (Height / 45), (Height - Height / 6) / 2 + Height / 7 + Height / 13);

            button8.Text = "В - ";
            button8.BackColor = Color.Black;
            button8.ForeColor = Color.White;
            button8.Font = new Font("Arial", Height / 70);
            button8.TextAlign = ContentAlignment.MiddleLeft;
            button8.Size = button6.Size;
            button8.Location = new System.Drawing.Point(Width / 25,
                button6.Location.Y + button6.Height + (Height / 50));

            button9.Text = "Г - ";
            button9.BackColor = Color.Black;
            button9.ForeColor = Color.White;
            button9.Font = new Font("Arial", Height / 70);
            button9.TextAlign = ContentAlignment.MiddleLeft;
            button9.Size = button6.Size;
            button9.Location = new System.Drawing.Point(Width / 25 +
                button8.Width + (Height / 50), button7.Location.Y + button7.Height + (Height / 50));

            button10.Size = button5.Size;
            button10.ForeColor = Color.White;
            button10.BackColor = Color.Black;
            button10.Font = new Font("Arial", Height / 54);
            button10.Location = new System.Drawing.Point(
                label10.Location.X - (Height / 12) - button10.Width, 10);

            SelectQuestion(-1);
            Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string messageBoxText = "Are you sure you want to end the game?";
            string caption = "Surrender!";
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxButton button = MessageBoxButton.YesNo;

            MessageBoxResult result;
            result = System.Windows.MessageBox.Show(messageBoxText,
                caption, button, icon, MessageBoxResult.Yes);

            if (result == MessageBoxResult.Yes)
            {
                timer1.Stop();
                timer2.Stop();
                WaitSeconds = 3;
                Controls[Count].ForeColor = Color.White;
                Controls[Count].BackColor = Color.Transparent;
                FinishTheGame();
            }

            Refresh();
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
                    buttons[index].Enabled = false;
                    buttons.RemoveAt(index);
                    index = random.Next(1, 2);
                    buttons[index].Text = "";
                    buttons[index].Enabled = false;
                }
                else if (button7.Text == "Б - " + Question.correct_answer)
                {
                    var buttons = new List<System.Windows.Forms.Button>()
                    { button7, button6, button8, button9 };
                    int index = random.Next(1, 3);
                    buttons[index].Text = "";
                    buttons[index].Enabled = false;
                    buttons.RemoveAt(index);
                    index = random.Next(1, 2);
                    buttons[index].Text = "";
                    buttons[index].Enabled = false;
                }
                else if (button8.Text == "В - " + Question.correct_answer)
                {
                    var buttons = new List<System.Windows.Forms.Button>()
                    { button8, button6, button7, button9 };
                    int index = random.Next(1, 3);
                    buttons[index].Text = "";
                    buttons[index].Enabled = false;
                    buttons.RemoveAt(index);
                    index = random.Next(1, 2);
                    buttons[index].Text = "";
                    buttons[index].Enabled = false;
                }
                else if (button9.Text == "Г - " + Question.correct_answer)
                {
                    var buttons = new List<System.Windows.Forms.Button>()
                    { button9, button6, button7, button8 };
                    int index = random.Next(1, 3);
                    buttons[index].Text = "";
                    buttons[index].Enabled = false;
                    buttons.RemoveAt(index);
                    index = random.Next(1, 2);
                    buttons[index].Text = "";
                    buttons[index].Enabled = false;
                }

                IsUsedBtn1 = true;
                Refresh();
            }
        }

        private void SelectQuestion(int index)
        {
            if (Level > 9)
            {
                FinishTheGame();
                return;
            }

            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i].GetType() == typeof(System.Windows.Forms.Button))
                {
                    Controls[i].Enabled = true;
                }
            }

            AudioSound.Stop();
            QuestionSound.Play();

            var path = Directory.Replace("bin", "") + "Questions.txt";
            var text = File.ReadAllText(path);

            var questionLevels = JArray.Parse(text);
            var questions = questionLevels[Level].ToString();
            var questionLevel = JsonConvert.DeserializeObject<QuestionLevel>(questions);

            Random random = new Random();
            QuestionCount = questionLevel.questions.Count;
            QuestionIndex = random.Next(0, QuestionCount);

            if (QuestionIndex == index)
            {
                while (QuestionIndex == index)
                {
                    QuestionIndex = random.Next(0, QuestionCount);
                }
            }

            Question = questionLevel.questions[QuestionIndex];

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
            EnabledButtons();
            QuestionSound.Stop();
            timer1.Stop();
            timer2.Start();
            IsClickedBtn6 = true;
            button6.ForeColor = Color.Black;
            button6.BackColor = Color.Goldenrod;

            Refresh();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            EnabledButtons();
            QuestionSound.Stop();
            timer1.Stop();
            timer2.Start();
            IsClickedBtn7 = true;
            button7.ForeColor = Color.Black;
            button7.BackColor = Color.Goldenrod;

            Refresh();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            EnabledButtons();
            QuestionSound.Stop();
            timer1.Stop();
            timer2.Start();
            IsClickedBtn8 = true;
            button8.ForeColor = Color.Black;
            button8.BackColor = Color.Goldenrod;

            Refresh();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            EnabledButtons();
            QuestionSound.Stop();
            timer1.Stop();
            timer2.Start();
            IsClickedBtn9 = true;
            button9.ForeColor = Color.Black;
            button9.BackColor = Color.Goldenrod;

            Refresh();
        }

        private void ClearAnswerButtons()
        {
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;

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
            string messageBoxText = "Do you really want to close the game?";
            string caption = "Closing the game!";
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxButton button = MessageBoxButton.YesNo;

            MessageBoxResult result;
            result = System.Windows.MessageBox.Show(messageBoxText,
                caption, button, icon, MessageBoxResult.Yes);

            if (result != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (WaitSeconds == 3)
            {
                if (IsClickedBtn6)
                {
                    IsClickedBtn6 = false;

                    if (button6.Text == "А - " + Question.correct_answer)
                    {
                        CorrectAnswerSound.Play();
                        button6.BackColor = Color.Green;
                        button6.ForeColor = Color.Black;
                    }
                    else
                    {
                        IsFinished = true;
                        WrongAnswerSound.Play();
                        button6.BackColor = Color.Red;
                        button6.ForeColor = Color.Black;
                    }
                }
                else if (IsClickedBtn7)
                {
                    IsClickedBtn7 = false;

                    if (button7.Text == "Б - " + Question.correct_answer)
                    {
                        CorrectAnswerSound.Play();
                        button7.BackColor = Color.Green;
                        button7.ForeColor = Color.Black;
                    }
                    else
                    {
                        IsFinished = true;
                        WrongAnswerSound.Play();
                        button7.BackColor = Color.Red;
                        button7.ForeColor = Color.Black;
                    }
                }
                else if (IsClickedBtn8)
                {
                    IsClickedBtn8 = false;

                    if (button8.Text == "В - " + Question.correct_answer)
                    {
                        CorrectAnswerSound.Play();
                        button8.BackColor = Color.Green;
                        button8.ForeColor = Color.Black;
                    }
                    else
                    {
                        IsFinished = true;
                        WrongAnswerSound.Play();
                        button8.BackColor = Color.Red;
                        button8.ForeColor = Color.Black;
                    }
                }
                else if (IsClickedBtn9)
                {
                    IsClickedBtn9 = false;

                    if (button9.Text == "Г - " + Question.correct_answer)
                    {
                        CorrectAnswerSound.Play();
                        button9.BackColor = Color.Green;
                        button9.ForeColor = Color.Black;
                    }
                    else
                    {
                        IsFinished = true;
                        WrongAnswerSound.Play();
                        button9.BackColor = Color.Red;
                        button9.ForeColor = Color.Black;
                    }
                }
            }
            else if (WaitSeconds == -1)
            {
                Time = 60;
                timer2.Stop();
                timer1.Start();
                WaitSeconds = 5;
                TimeText = "01:00";
                ClearAnswerButtons();
                WrongAnswerSound.Stop();
                CorrectAnswerSound.Stop();
                Controls[Count].ForeColor = Color.White;
                Controls[Count].BackColor = Color.Transparent;

                if (IsFinished == false)
                {
                    Count--;
                    Level++;
                    AudioSound.Stop();
                    Controls[Count].ForeColor = Color.Black;
                    Controls[Count].BackColor = Color.Goldenrod;
                    SelectQuestion(-1);
                }
                else
                {
                    FinishTheGame();
                    AudioSound.Stop();
                }
            }

            WaitSeconds--;
            Refresh();
        }

        private void FinishTheGame()
        {
            timer1.Stop();
            TimeText = "";
            ClearAnswerButtons();
            Question.question = "";
            Player.PlayedTime = PlayedTime;
            Refresh();

            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i] == button10 ||
                    Controls[i] == button5)
                {
                    Controls[i].Enabled = true;
                    continue;
                }

                Controls[i].Enabled = false;
            }

            if (Controls[Count] == label1)
            {
                Player.Win = 0;
                System.Windows.Forms.MessageBox.Show(
                $"The game is over!\n Your win is {Player.Win} GBP !");
            }
            else if (Level <= 9)
            {
                var sum = Controls[++Count].Text.Split(' ');
                Player.Win = int.Parse(sum[sum.Length - 3] +
                    sum[sum.Length - 2] + sum[sum.Length - 1]);

                System.Windows.Forms.MessageBox.Show(
                $"The game is over!\n Your win is {Player.Win} GBP !");
            }
            else
            {
                Player.Win = 1000000;
                ShowWinAnimation();
            }

            CheckLeaderboard();
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
                Time = 60;
                timer1.Start();
                WaitSeconds = 5;
                TimeText = "01:00";
                IsUsedBtn1 = false;
                IsUsedBtn2 = false;
                IsUsedBtn3 = false;
                IsFinished = false;
                ClearAnswerButtons();
                IsClickedBtn6 = false;
                IsClickedBtn7 = false;
                IsClickedBtn8 = false;
                IsClickedBtn9 = false;
                var name = Player.Name;
                Player = new Player();
                Player.Name = name;

                AudioSound.Stop();
                QuestionSound.Stop();
                WrongAnswerSound.Stop();
                CorrectAnswerSound.Stop();

                if (Controls[Count] != button2)
                {
                    Controls[Count].ForeColor = Color.White;
                    Controls[Count].BackColor = Color.Transparent;
                }

                Count = Controls.GetChildIndex(label1);

                for (int i = 0; i < Controls.Count; i++)
                {
                    Controls[i].Enabled = true;
                }

                label1.ForeColor = Color.Black;
                label1.BackColor = Color.Goldenrod;
                SelectQuestion(-1);
            }

            Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var file = Directory.Replace("bin", "") + "Leaderboard.txt";
            var content = File.ReadAllText(file);

            if (content != "")
            {
                var result = "";
                List<Player> players = JsonConvert.DeserializeObject<List<Player>>(content);
                var rank = players.OrderByDescending(p => p.Win).ThenBy(p => p.PlayedTime).ToList();

                if (rank.Count > 0)
                {
                    for (int i = 0; i < players.Count; i++)
                    {
                        TimeSpan t = TimeSpan.FromSeconds(rank[i].PlayedTime);
                        string time = string.Format("{0:D2}:{1:D2}",
                            t.Minutes,
                            t.Seconds);

                        result += (i + 1).ToString() + '.' + ' ' + "Player name: " + rank[i].Name
                            + $" Win: {rank[i].Win} Played time: {time}" + '\n';
                    }
                }

                System.Windows.Forms.MessageBox.Show(result);
            }
        }

        private void CheckLeaderboard()
        {
            var file = Directory.Replace("bin", "") + "Leaderboard.txt";
            var content = File.ReadAllText(file);
            var players = new List<Player>();

            if (content != "")
            {
                players = JsonConvert.DeserializeObject<List<Player>>(content)
                .OrderByDescending(p => p.Win).ThenBy(p => p.PlayedTime).ToList();
            }

            if (players.Count < 10 && !players.Select(p
                => p.Name).ToList().Contains(Player.Name))
            {
                players.Add(Player);
            }
            else
            {
                var names = players.Select(p => p.Name).ToList();

                if (names.Contains(Player.Name))
                {
                    Player player = players.Where(p => p.Name == Player.Name).FirstOrDefault();

                    if ((Player.Win == player.Win && Player.PlayedTime
                        < player.PlayedTime) || Player.Win > player.Win)
                    {
                        players.Remove(player);
                        players.Add(Player);
                    }
                }
                else if (Player.Win > players.Last().Win || (Player.Win ==
                    players.Last().Win && Player.PlayedTime < players.Last().PlayedTime))
                {
                    players.RemoveAt(players.Count - 1);
                    players.Add(Player);
                }
            }

            var rankList = players.OrderByDescending(p =>
                p.Win).ThenBy(p => p.PlayedTime).ToList();
            File.WriteAllText(file, JsonConvert.SerializeObject(rankList));
        }

        private string FormatText(string text)
        {
            if (text == null)
            {
                return null;
            }

            var chars = text.ToCharArray();

            int i;
            int j = 0;

            for (i = 0; i < chars.Length; i++)
            {
                if ((i - j) * (Height / 65) + 100 >= label10.Location.X - 20)
                {
                    for (j = i; j > 0; j--)
                    {
                        if (chars[j] == ' ')
                        {
                            chars[j] = '\n';
                            break;
                        }
                    }
                }
            }

            return new string(chars);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (IsUsedBtn3 == false)
            {
                Time = 60;
                IsUsedBtn3 = true;
                ClearAnswerButtons();
                SelectQuestion(QuestionIndex);

                Refresh();
            }
        }

        private void button11_Paint(object sender, PaintEventArgs e)
        {
            if (IsUsedBtn3)
            {
                Pen pen = new Pen(Color.Red, 8);
                e.Graphics.DrawLine(pen, 0, button11.Height, button11.Width, 0);
                e.Graphics.DrawLine(pen, 0, 0, button11.Width, button11.Height);
            }
        }

        private void ShowWinAnimation()
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                Controls[i].Visible = false;
            }

            ElementHost element = new ElementHost();
            element.Dock = DockStyle.None;
            var file = Directory.Replace("bin", "") + "giphy.gif";

            element.Width = Width;
            element.Height = Height;
            element.Location = new System.Drawing.Point(0, 0);

            MediaElement gif = new MediaElement();
            gif.LoadedBehavior = MediaState.Manual;
            gif.UnloadedBehavior = MediaState.Manual;

            if (File.Exists(file))
            {
                gif.Source = new Uri(file);
                element.Child = gif;
                Controls.Add(element);
                gif.MediaEnded += (sender, args) =>
                {
                    Controls.Remove(element);

                    for (int i = 0; i < Controls.Count; i++)
                    {
                        Controls[i].Visible = true;
                    }
                    gif = null;
                    element.Dispose();
                };             
                gif.Play();
            }
        }

        private void EnabledButtons()
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i].GetType() == typeof(System.Windows.Forms.Button))
                {
                    Controls[i].Enabled = false;
                }
            }
        }
    }
}
