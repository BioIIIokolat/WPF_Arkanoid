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
using Microsoft.Win32;

namespace WPF_Arkanoid
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {

        public string path;

        public Settings()
        {
            InitializeComponent();
        }

        public string Width
        {
            get { return WidthText.Text; }
        }

        public string Height
        {
            get { return HeightText.Text; }
        }

        public string Speed
        {
            get { return SpeedText.Text; }
        }

        public Color ColorBall
        {
            get { return (Color)ClrPcker_Ball.SelectedColor; }
        }
     

        public Color ColorPlatform
        {
            get { return (Color)ClrPcker_PlatForm.SelectedColor; }
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            if (WidthText.Text != "" && HeightText.Text != "" && SpeedText.Text != "" && ClrPcker_Ball.SelectedColor != null && ClrPcker_PlatForm.SelectedColor != null && path != null)
            {
                int width = Convert.ToInt32(WidthText.Text);
                int height = Convert.ToInt32(HeightText.Text);
                int speed = Convert.ToInt32(SpeedText.Text);

                if (width > 24 && width <= 0)
                {
                    WidthText.Text = "24";
                }

                if (height > 6 && height <= 0)
                {
                    HeightText.Text = "4";
                }

                if (speed > 500 && speed < 0)
                {
                    SpeedText.Text = "150";
                }

                Game game = new Game(WidthText.Text, HeightText.Text, SpeedText.Text, (Color)ClrPcker_Ball.SelectedColor, (Color)ClrPcker_PlatForm.SelectedColor, path);

                game.Show();
            }
            else
            {
                MessageBox.Show("Вы не ввели все параметры для начала игры! Введите все параметры и нажимайте Apply!!!");
            }
        }

        private void ClrPcker_Ball_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
        }

        private void ClrPcker_Platform_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {

        }

        private void KeyDownTextBox_Click(object sender, KeyEventArgs e)
        {
           
        }


        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter = "mp3 file (*.mp3)|*.mp3|Wav file (*.wav*)|*.wav*";

            if (fileDialog.ShowDialog() == true)
            {
                path = fileDialog.FileName;
                selectBut.Background = Brushes.Green;
            }
        }
    }
}
