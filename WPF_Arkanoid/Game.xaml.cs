using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using System.Media;

namespace WPF_Arkanoid
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        // плеер для проигрыша музыкальных эффектов
        private MediaPlayer player = new MediaPlayer();

        // плеер для проигрыша музыки на фоне
        private MediaPlayer playa;
        public int count = 0;

        // переменные для игрового функционала
        bool gameStart = false;              // переменная для проверки на начало игры
        bool gameEnd = false;                // переменная для проверки на конец игры
        int dx = -1;                         // для отскока шарика по X координате
        int dy = -1;                         // для отскока шарика по Y координате

        DispatcherTimer timer1;              // ТАЙМЕР

        // листы
        List<Rectangle> rectangles = new List<Rectangle>();
        List<Brush> colorRectangle = new List<Brush>();
        List<Rectangle> tempRectangle = new List<Rectangle>();
        List<Brush> colorPlatforms = new List<Brush>();

        // переменная счёта
        int scor = 0;
        public Random rndm = new Random();
        public Scores scr = new Scores();

        public string path = @"D:\Lessons for C#\WPF\WPF_Arkanoid\WPF_Arkanoid\Saves\sv.txt";
        public string pathColor = @"D:\Lessons for C#\WPF\WPF_Arkanoid\WPF_Arkanoid\Saves\sv_color.txt";

        // скорость которая передаётся из настроек
        int tempSpeed;

        public Game()
        {
            this.Title = "Status - Passive";

            colorRectangle.Add(Brushes.Blue);
            colorRectangle.Add(Brushes.Red);
            colorRectangle.Add(Brushes.Pink);

            InitializeComponent();

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 24;i++)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Width = 250;

                    if (rndm.Next(1, 10) / 2 != 0)
                    {
                        rectangle.Fill = colorRectangle[rndm.Next(0, 2)];
                    }
                    else
                    {
                        rectangle.Fill = new SolidColorBrush((Color.FromRgb((byte)rndm.Next(1, 255), (byte)rndm.Next(0, 255), (byte)rndm.Next(0, 233))));
                    }

                    Grid.SetColumn(rectangle, i);
                    Grid.SetColumnSpan(rectangle, 1);
                    Grid.SetRow(rectangle, j);
                    Grid.SetRowSpan(rectangle, 1);
                    MyGrid.Children.Add(rectangle);
                    rectangles.Add(rectangle);
                }
            }

            laser.Visibility = Visibility.Hidden;

            tempRectangle = rectangles;
        }

        // конструктор с параметрами которые приходят от класса Settings
        public Game(string width, string height, string speed, Color ballColor, Color platformColor, string pathMusic)
        {
            colorRectangle.Add(Brushes.Blue);
            colorRectangle.Add(Brushes.Red);
            colorRectangle.Add(Brushes.Pink);
           
            InitializeComponent();

            SolidColorBrush brush;

            if (pathMusic != "")
            {
                playa = new MediaPlayer();

                playa.Open(new Uri(pathMusic, UriKind.RelativeOrAbsolute));

                playa.Play();
            }

            if (width.Length > 0 && height.Length > 0)
            {
                for (int j = 0; j < Convert.ToInt32(height); j++)
                {
                    for (int i = 0; i < Convert.ToInt32(width); i++)
                    {
                        Rectangle rectangle = new Rectangle();
                        rectangle.Width = 250;

                        if (rndm.Next(1, 10) / 2 != 0)
                        {
                            rectangle.Fill = colorRectangle[rndm.Next(0, 2)];
                        }

                        rectangle.Fill = new SolidColorBrush((Color.FromRgb((byte)rndm.Next(1, 255), (byte)rndm.Next(0, 255), (byte)rndm.Next(0, 233))));
                        Grid.SetColumn(rectangle, i);
                        Grid.SetColumnSpan(rectangle, 1);
                        Grid.SetRow(rectangle, j);
                        Grid.SetRowSpan(rectangle, 1);
                        MyGrid.Children.Add(rectangle);
                        rectangles.Add(rectangle);
                    }
                }
            }

            laser.Visibility = Visibility.Hidden;

            if (ballColor != null)
            {
                brush = new SolidColorBrush(ballColor);
                Ball.Fill = brush;
            }

            if(platformColor != null)
            {
                brush = new SolidColorBrush(platformColor);
                UserBlock.Fill = brush;
            }

            if(speed.Length >= 0)
            {
                tempSpeed = Convert.ToInt32(speed);
            }

            tempRectangle = rectangles;
        }

        public void Init()
        {
            InitializeComponent();

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 24; i++)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Width = 250;

                    if (rndm.Next(1, 10) / 2 != 0)
                    {
                        rectangle.Fill = colorRectangle[rndm.Next(0, 2)];
                    }

                    rectangle.Fill = new SolidColorBrush((Color.FromRgb((byte)rndm.Next(1, 255), (byte)rndm.Next(0, 255), (byte)rndm.Next(0, 233))));
                    Grid.SetColumn(rectangle, i);
                    Grid.SetColumnSpan(rectangle, 1);
                    Grid.SetRow(rectangle, j);
                    Grid.SetRowSpan(rectangle, 1);
                    MyGrid.Children.Add(rectangle);
                    rectangles.Add(rectangle);
                }
            }

            Grid.SetColumn(UserBlock, 0);
            Grid.SetColumnSpan(UserBlock, 3);
            Grid.SetRow(UserBlock, 22);

            Grid.SetColumn(Ball, 1);
            Grid.SetColumnSpan(Ball, 1);
            Grid.SetRow(Ball, 21);
            Grid.SetRowSpan(Ball, 1);
        }

        private void UserBlock_KeyDown(object sender, KeyEventArgs e)
        {
            // движение платформы вправо
            if (e.Key == Key.Right)
            {
                int currentCol = Grid.GetColumn(UserBlock);
                int currentCol_1 = Grid.GetColumn(laser);
                if (currentCol < 21)
                {
                    Grid.SetColumn(UserBlock, currentCol + 2);
                    Grid.SetColumn(laser, currentCol_1 + 2);
                    if (!gameStart)
                    {
                        int ballCol = Grid.GetColumn(Ball);
                        Grid.SetColumn(Ball, ballCol + 2);
                    }
                }
            }

            // движение платформы влево
            if (e.Key == Key.Left)
            {
                int currentCol = Grid.GetColumn(UserBlock);
                int currentCol_1 = Grid.GetColumn(laser);
                if (currentCol > 0)
                {
                    Grid.SetColumn(UserBlock, currentCol - 2);
                    Grid.SetColumn(laser, currentCol_1 - 2);

                    if (!gameStart)
                    {
                        int ballCol = Grid.GetColumn(Ball);
                        Grid.SetColumn(Ball, ballCol - 2);
                    } // if gamestart

                } // if current

            } // if key

            // Пауза
            if (e.Key == Key.Escape)
            {
                if (timer1.IsEnabled)
                {
                    timer1.Stop();
                    MessageBox.Show("Вы поставили паузу, чтобы убрать паузу нажмите R ");
                }
                else
                {
                    MessageBox.Show("Вы не начали игру, пауза не возможна!");
                }
            }// if escape

            // Возобновление игры
            if (e.Key == Key.R)
            {
                MessageBox.Show("Возобновляю вашу игру, удачно провести время!");
                gameStart = true;
                timer1 = new DispatcherTimer();

                if (tempSpeed != 0)
                {
                    timer1.Interval = TimeSpan.FromMilliseconds(tempSpeed);
                }
                else
                {
                    timer1.Interval = TimeSpan.FromMilliseconds(50);
                }

                timer1.Tick += Timer1_Tick;

                timer1.Start();
            } // if R


            // загрузка
            if(e.Key == Key.L)
            {
                if (timer1 != null)
                {
                    Load();
                }
            }

            // сохранение
            if(e.Key == Key.S)
            {
                Save();
            }

            if (e.Key == Key.M)
            {
                if (playa != null)
                {
                    playa.Stop();
                }
            }

            if (e.Key == Key.P)
            {
                if(playa != null)
                {
                    playa.Play();
                }
            }

            // ресурсы 1
            if (e.Key == Key.B)
            {
                if (rectangles.Count > 0)
                {
                    MessageBox.Show("Применяется стиль - 1 (Кисть)");

                    SolidColorBrush brWhite = new SolidColorBrush(Colors.White);
                    this.Resources.Add("Brush1", brWhite);
                    SolidColorBrush brBlack = new SolidColorBrush(Colors.Black);
                    this.Resources.Add("Brush2", brBlack);

                    foreach (Rectangle item in rectangles)
                    {
                        item.Fill = (Brush)this.Resources["Brush1"];
                    }

                    MyGrid.Background = (Brush)this.Resources["Brush2"];
                    UserBlock.Fill = (Brush)this.Resources["Brush1"];
                    Ball.Fill = (Brush)this.Resources["Brush1"];

                    this.Resources.Remove("Brush1");
                    this.Resources.Remove("Brush2");
                }
            }

            // ресурсы 2
            if (e.Key == Key.G) 
            {
                if(rectangles.Count > 0)
                {
                    MessageBox.Show("Применяется стиль - 2 (Градиент)");

                    LinearGradientBrush lnrGradien = new LinearGradientBrush();
                    lnrGradien.GradientStops.Add(new GradientStop(Colors.LightBlue,1));
                    lnrGradien.GradientStops.Add(new GradientStop(Colors.LightPink,4));

                    this.Resources.Add("Gradient1", lnrGradien);

                    LinearGradientBrush lnrGradien1 = new LinearGradientBrush();
                    lnrGradien.GradientStops.Add(new GradientStop(Colors.Blue, 1));
                    lnrGradien.GradientStops.Add(new GradientStop(Colors.Yellow, 6));

                    this.Resources.Add("Gradient2", lnrGradien1);

                    foreach (Rectangle item in rectangles)
                    {
                        item.Fill = (Brush)this.Resources["Gradient1"];
                    }

                    UserBlock.Fill = (Brush)this.Resources["Gradient2"];
                    MyGrid.Background = Brushes.White;

                    this.Resources.Remove("Gradient1");
                    this.Resources.Remove("Gradient2");
                }
            }

            // начало игры
            if(!gameStart && e.Key == Key.Enter)
            {
                this.Title = "Status - Active";

                gameStart = true;

                timer1 = new DispatcherTimer();

                if(tempSpeed != 0)
                {
                    timer1.Interval = TimeSpan.FromMilliseconds(tempSpeed);
                }
                else
                {
                    timer1.Interval = TimeSpan.FromMilliseconds(50);
                }


                timer1.Tick += Timer1_Tick;
                timer1.Start();
            } // if gamestart and Enter
        }


        public void Load()
        {
            MessageBox.Show("Загружаю вашу игру! Для начала игры нажми на кнопку R !");

            foreach (var item in tempRectangle)
            {
                MyGrid.Children.Remove(item);
            } // foreach tempRectangle

            string curScore = File.ReadAllText(@"D:\Lessons for C#\WPF\WPF_Arkanoid\WPF_Arkanoid\Saves\sv_score.txt");

            string[] lines = File.ReadAllLines(path);
            string[] linesColor = File.ReadAllLines(pathColor);

            tempSpeed = Convert.ToInt32(File.ReadAllText(@"D:\Lessons for C#\WPF\WPF_Arkanoid\WPF_Arkanoid\Saves\sv_ballSpeed.txt"));

            string[] linesPlatform = File.ReadAllLines(@"D:\Lessons for C#\WPF\WPF_Arkanoid\WPF_Arkanoid\Saves\sv_platform.txt");
            string[] linesBall = File.ReadAllLines(@"D:\Lessons for C#\WPF\WPF_Arkanoid\WPF_Arkanoid\Saves\sv_ball.txt");

            string[] temp;

            int count = 0;

            foreach (string s in lines)
            {
                        temp = s.Split(' ');

                        Rectangle rectangle = new Rectangle();

                        rectangle.Width = 250;

                        if(count != linesColor.Length + 1)
                        {
                          Color color = (Color)ColorConverter.ConvertFromString(linesColor[count]);

                          rectangle.Fill = new SolidColorBrush(color);
                          count++;
                        }

                        Grid.SetColumn(rectangle, Convert.ToInt32(temp[0]));
                        Grid.SetColumnSpan(rectangle, Convert.ToInt32(temp[1]));
                        Grid.SetRow(rectangle, Convert.ToInt32(temp[2]));
                        Grid.SetRowSpan(rectangle, Convert.ToInt32(temp[3]));

                        MyGrid.Children.Add(rectangle);
                        rectangles.Add(rectangle);
            } // foreach lines
            

            foreach (var item in linesPlatform)
            {
                temp = item.Split(' ');

                Grid.SetColumn(UserBlock, Convert.ToInt32(temp[0]));
                Grid.SetColumnSpan(UserBlock, Convert.ToInt32(temp[1]));
                Grid.SetRow(UserBlock, Convert.ToInt32(temp[2]));
            } // foreach linesPlatform

            foreach (var item in linesBall)
            {
                timer1.Stop();
                temp = item.Split(' ');

                Grid.SetColumn(Ball, Convert.ToInt32(temp[0]));
                Grid.SetColumnSpan(Ball, Convert.ToInt32(temp[1]));
                Grid.SetRow(Ball, Convert.ToInt32(temp[2]));
                Grid.SetRowSpan(Ball, Convert.ToInt32(temp[3]));
            } // foreach linesBall

            scor = Convert.ToInt32(curScore);

            this.Title = $"Score - {scor}, Status - Active";

            tempRectangle = rectangles;
        }

        public void Save()
        {
            MessageBox.Show("Сохраняю вашу игру!");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            if(File.Exists(pathColor))
            {
                File.Delete(pathColor);
            }

            foreach (var item in tempRectangle)
            {
                File.AppendAllText(path, Grid.GetColumn(item).ToString() + " " + Grid.GetColumnSpan(item).ToString() + " " + Grid.GetRow(item).ToString() + " " + Grid.GetRowSpan(item).ToString() + Environment.NewLine);
                File.AppendAllText(pathColor, item.Fill.ToString() + Environment.NewLine);
            }

            File.WriteAllText(@"D:\Lessons for C#\WPF\WPF_Arkanoid\WPF_Arkanoid\Saves\sv_score.txt", scor.ToString());

            File.WriteAllText(@"D:\Lessons for C#\WPF\WPF_Arkanoid\WPF_Arkanoid\Saves\sv_platform.txt", Grid.GetColumn(UserBlock).ToString() + " " + Grid.GetColumnSpan(UserBlock).ToString() + " " + Grid.GetRow(UserBlock).ToString());

            File.WriteAllText(@"D:\Lessons for C#\WPF\WPF_Arkanoid\WPF_Arkanoid\Saves\sv_ball.txt", Grid.GetColumn(Ball).ToString() + " " + Grid.GetColumnSpan(Ball).ToString() + " " + Grid.GetRow(Ball).ToString() + " " + Grid.GetRowSpan(Ball));

            File.WriteAllText(@"D:\Lessons for C#\WPF\WPF_Arkanoid\WPF_Arkanoid\Saves\sv_ballSpeed.txt", tempSpeed.ToString());
        }


        // функция с бонусами :3
        public void ColorsTransform(UIElement curr)
        {
            int count = 0;

            Rectangle current = new Rectangle();

            current = (Rectangle)curr;

            foreach (var item in colorRectangle)
            {
                if (current.Fill == colorRectangle[0])
                {
                    if (scor != 0)
                    {
                        scor *= 2;
                    }
                }

                if (current.Fill == colorRectangle[1])
                {
                    //if (count <= 3)
                    //{
                        List<Rectangle> lst = new List<Rectangle>();

                    laser.Visibility = Visibility.Visible;

                    foreach (var item1 in tempRectangle)
                    {
                       
                            if (Grid.GetColumn(laser) == Grid.GetColumn(item1))
                            {
                                MyGrid.Children.Remove(item1);
                                lst.Add(item1);
                                count++;
                            }
                     
                    }

                    foreach (var item2 in lst)
                    {
                        if (tempRectangle.Contains(item2)) tempRectangle.Remove(item2);
                    }
                    }
                    else
                    {
                        laser.Visibility = Visibility.Hidden;
                    }

                 //   count++;
               // }

                    if (current.Fill == colorRectangle[2])
                    {
                    tempSpeed = 200;
                    }
            }
        }

        // непосредственно наш таймер
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if(gameEnd)
            {
                this.Title.Replace("Active", "Passive");

                MessageBoxResult result = MessageBox.Show("Вы хотите начать новую игру?", "Пластмассовый мир победил, а вы проиграли!", MessageBoxButton.YesNo, MessageBoxImage.Question);

                scor = 0;

                player.Open(new Uri(@"D:\Lessons for C#\WPF\WPF_Arkanoid\WPF_Arkanoid\Music\2.mp3", UriKind.RelativeOrAbsolute));

                player.Play();

                if (result == MessageBoxResult.Yes)
                {
                    ClearBlocks();

                    Init();
                    gameEnd = false;
                }
                else
                {
                    timer1.Stop();
                    return;
                }
                player.Stop();
            }

            if(MyGrid.Children.Count == 2)
            {
                this.Title.Replace("Active", "Passive");

                MessageBoxResult result = MessageBox.Show("Вы хотите начать новую игру?", "Вы выиграли!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                scor = 0;

                player.Open(new Uri(@"D:\Lessons for C#\WPF\WPF_Arkanoid\WPF_Arkanoid\Music\1.mp3", UriKind.RelativeOrAbsolute));

                player.Play();

                if (result == MessageBoxResult.Yes)
                {
                    ClearBlocks();
                    Init();
                }
                else
                {
                    player.Stop();
                    return;
                }
            }

            Check();

            int currentballColumn = Grid.GetColumn(Ball);
            Grid.SetColumn(Ball, currentballColumn + dx);
            int currentballRow = Grid.GetRow(Ball);
            Grid.SetRow(Ball, currentballRow + dy);

        } // timer


        // удаление всех блоков
        public void ClearBlocks()
        {
            if (MyGrid.Children.Count > 0)
            {
                    foreach (Rectangle rect in tempRectangle)
                    {
                             MyGrid.Children.Remove(rect);
                    }
            }
        } // ClearBlocks

        public void PlayerOtskok()
        {
            player.Open(new Uri(@"D:\Lessons for C#\WPF\WPF_Arkanoid\WPF_Arkanoid\Music\3.mp3", UriKind.RelativeOrAbsolute));

            player.Play();
        }
        
        public void PlayerOtskokPlatform()
        {
            player.Open(new Uri(@"D:\Lessons for C#\WPF\WPF_Arkanoid\WPF_Arkanoid\Music\4.mp3", UriKind.RelativeOrAbsolute));

            player.Play();
        }


        // проверка на отскок
        public void Check()
        {
            int getColumnUserBlock = Grid.GetColumn(UserBlock);
            int getColumnBall = Grid.GetColumn(Ball);
            int getRowBall = Grid.GetRow(Ball);


            if(getRowBall == 21 && dy > 0)
            {
                    if (getColumnUserBlock >= (getColumnBall - 2) && getColumnUserBlock <= getColumnBall)
                    {
                        dy = -dy;
                        PlayerOtskok();
                    }
                    else if ((getColumnUserBlock == (getColumnBall - 3) && dx < 0) || (getColumnUserBlock == (getColumnBall + 1) && dx > 0))
                    {
                        dy = -dy;
                        dx = -dx;
                       PlayerOtskok();
                    }
                else
                    {
                        gameEnd = true;
                        scr.AddScore(scor.ToString());
                        scr.Show();
                    }
                }

                if ((getColumnBall == 0 && dx < 0) || (getColumnBall == 23 && dx > 0))
                {
                    dx = -dx;
                PlayerOtskok();
                }
            if (getRowBall == 0 && dy < 0)
                {
                    dy = -dy;
                PlayerOtskok();
                }

            UIElement upDownBlock = MyGrid.Children.Cast<Rectangle>().Where(i => (Grid.GetRow(i) == getRowBall + dy) && (Grid.GetColumn(i) == getColumnBall)).FirstOrDefault();
                if (upDownBlock != null)
                {

                ColorsTransform(upDownBlock);

                scor += 50;
                this.Title = "Score - " + scor.ToString() + " Status - Active";
                PlayerOtskokPlatform();
                dy = -dy;
                    MyGrid.Children.Remove(upDownBlock);
                    tempRectangle.Remove((Rectangle)upDownBlock);
                }

                UIElement leftRightBlock = MyGrid.Children.Cast<Rectangle>().Where(i => (Grid.GetColumn(i) == getColumnBall + dx) && (Grid.GetRow(i) == getRowBall)).FirstOrDefault();
                if (leftRightBlock != null)
                {
                ColorsTransform(leftRightBlock);

                scor += 100;
                this.Title ="Score - " + scor.ToString() + " Status - Active";
                dx = -dx;
                PlayerOtskokPlatform();
                MyGrid.Children.Remove(leftRightBlock);
                tempRectangle.Remove((Rectangle)leftRightBlock);
                }

            UIElement skos = MyGrid.Children.Cast<Rectangle>().Where(i => (Grid.GetColumn(i) == getColumnBall + dx) && (Grid.GetRow(i) == getRowBall + dy)).FirstOrDefault();
                if (skos != null)
                {
                ColorsTransform(skos);
                scor += 150;
                this.Title = "Score - " + scor.ToString() + "Status - Active";
                dy = -dy;
                PlayerOtskokPlatform();
                MyGrid.Children.Remove(skos);
                tempRectangle.Remove((Rectangle)skos);
                }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (timer1 != null)
            {
                timer1.Stop();
            }

            MessageBoxResult rest = MessageBox.Show("Вы хотите сохранить игру перед тем как выйти?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if(rest == MessageBoxResult.Yes)
            {
                if (gameEnd == true)
                {
                    MessageBox.Show("Вы проиграли игру! Сохранение не возможно!!!");
                } 
                else if(gameStart == false)
                {
                    MessageBox.Show("Вы не начали игру! Сохранение не возможно!!!");
                }
                else
                {
                    Save();
                }
            }
            else
            {
               
            }
        }

        public void Levels()
        {

        }
    }
}
