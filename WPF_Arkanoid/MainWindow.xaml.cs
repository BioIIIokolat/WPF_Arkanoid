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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Arkanoid
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Game game = new Game();

            game.Show();
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            Info info = new Info();

            info.Show();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Settings set = new Settings();

            set.Show();
        }

        private void Records_Click(object sender, RoutedEventArgs e)
        {
            Scores scr = new Scores();

            scr.ShowRecord();

            scr.Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
                MessageBoxResult result = MessageBox.Show("Вы точно хотите выйти из игры?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if(result == MessageBoxResult.Yes)
                {
                foreach (Window w in App.Current.Windows) w.Close();
                }
               else
               {
                MessageBox.Show("Вы захотели продолжить игру, моё уважение друг! Удачной игры тебе!");
               }
        }

        private void Media_Ended(object sender, RoutedEventArgs e)
        {
            playa.Position = TimeSpan.Zero;
            playa.Play();
        }
    }
}
