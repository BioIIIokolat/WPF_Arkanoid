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
using System.IO;
using System.Collections;

namespace WPF_Arkanoid
{
    /// <summary>
    /// Interaction logic for Scores.xaml
    /// </summary>
    public partial class Scores : Window
    {
        public List<string> str = new List<string>();

        public Scores()
        {
            InitializeComponent();
        }

        public Scores(string scores)
        {
            lstbox1.Items.Add(scores);

            InitializeComponent();
        }

        public void AddScore(string scores)
        {
            //int[] mas = new int[lstbox1.Items.Count];
            //string[] str = File.ReadAllLines(@"D:\Lessons for C#\WPF\\WPF_Arkanoid\WPF_Arkanoid\Scores\record.txt");

            //for (int i = 0; i < lstbox1.Items.Count; i++)
            //{
            //    mas[i] = Convert.ToInt32(str[i]);
            //}

            //int[] mas2 = new int[lstbox1.Items.Count];
            //int sort = 0;
            //for (int i = 0; i < lstbox1.Items.Count; i++)
            //{
            //    if (mas[i] < 0)
            //    {
            //        mas2[sort] = mas[i];
            //        sort++;
            //    }
            //}
            //for (int i = 0; i < lstbox1.Items.Count; i++)
            //{
            //    if (mas[i] > 0)
            //    {
            //        mas2[sort] = mas[i];
            //        sort++;
            //    }
            //}

            //lstbox1.Items.Clear();

            //for (int i = 0; i < mas2.Length; i++)
            //{
            //    lstbox1.Items.Add(mas2[i]);
            //}

            lstbox1.Items.Add(scores);

            ArrayList Sorting = new ArrayList();

            foreach (var o in lstbox1.Items)
            {
                Sorting.Add(o);
            }

            Sorting.Sort();

            lstbox1.Items.Clear();

            foreach (var o in Sorting)
            {
                lstbox1.Items.Add(o);
            }

            foreach (var item in lstbox1.Items)
            {
                File.AppendAllText(@"D:\Lessons for C#\WPF\\WPF_Arkanoid\WPF_Arkanoid\Scores\record.txt", item + Environment.NewLine);
            }
        }

        public void ShowRecord()
        {
                foreach (var item in File.ReadAllLines(@"D:\Lessons for C#\WPF\\WPF_Arkanoid\WPF_Arkanoid\Scores\record.txt"))
                {
                lstbox1.Items.Add(item);
                }
        }

        public void RemoveScore(string scores)
        {
            lstbox1.Items.Remove(scores);
        }

        public void FindScore(string scores)
        {
            lstbox1.Items.Contains(scores);
        }
    }
}
