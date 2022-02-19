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
using System.Diagnostics;
using System.IO;

namespace Prj_Soft_Protection
{
    public partial class StudyModeWindow : Window
    {
        static int counter = 0;
        static Stopwatch stopwatch = new Stopwatch();
        static List<double> inter = new List<double>();
        static StreamWriter sr = new StreamWriter(@"result.txt");
        public StudyModeWindow()
        {
            InitializeComponent();
        }
        private void TextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (InputField.Text.Length != VerifField.Text.Length )
            {
                stopwatch.Stop();
                CountProtection.IsEnabled = false;
                if (InputField.Text.Length != 1)
                    inter.Add(stopwatch.ElapsedMilliseconds / 1000.0);
                SymbolCount.Content = InputField.Text.Length;
                stopwatch.Reset();
                stopwatch.Start();
            }
            else
            {
                stopwatch.Stop();
                inter.Add(stopwatch.ElapsedMilliseconds / 1000.0);
                stopwatch.Reset();
                SymbolCount.Content = 0;
                if (Equals(InputField.Text, VerifField.Text))
                    calc();
                else
                    MessageBox.Show("Ви ввели некоректне слово");
                inter.Clear();
                InputField.Text = "";
                if (counter == CountProtection.SelectedIndex + 3)
                {
                    counter = 0;
                    sr.Close();
                    MainWindow mainwindow = new MainWindow();
                    Hide();
                    mainwindow.Show();
                }
            }
        }
        private void calc()
        {
            double[] student = { 6.314, 2.92, 2.353, 2.132, 2.015, 1.943, 1.895, 1.86, 1.833, 1.813, 1.8, 1.782, 1.761, 1.75, 1.75, 1.74, 1.734, 1.725, 1.72 };
            for (int i = 0; i < VerifField.Text.Length - 1; i++)
            {
                double m, s, t, sum = 0;
                List<double> z = inter.GetRange(0, inter.Count);
                z.RemoveAt(i);

                m = z.Sum() / (VerifField.Text.Length - 1);
                foreach (double del in z)
                    sum += Math.Pow(del - m, 2);
                s = Math.Sqrt(sum / (VerifField.Text.Length - 2));
                t = (inter[i] - m) / s;
                if (t > student[inter.Count - 2])
                {
                    MessageBox.Show("Присутній незначущий елемент");
                    return;
                }
            }

            double ma, sa, suma = 0;
            ma = inter.Sum()/ (VerifField.Text.Length - 1);
            foreach (double del in inter)
                suma += Math.Pow(del - ma, 2);
            sa = suma / (VerifField.Text.Length - 2);
            sr.WriteLine($"{ma};{sa}");
            counter++;
            MessageBox.Show($"{counter} слово зараховане");
        }
        private void CloseStudyMode_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            Hide();
            mainwindow.Show();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            counter = 0;
            Application.Current.Shutdown();
        }
    }
}