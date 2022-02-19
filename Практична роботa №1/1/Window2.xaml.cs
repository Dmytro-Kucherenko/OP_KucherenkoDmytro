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
using System.Diagnostics;

namespace Prj_Soft_Protection
{
    public partial class ProtectionModeWindow : Window
    {
        static int counter = 0;
        static double attempts = 0;
        static double sa;
        static StreamReader sr = new StreamReader(@"E:\\Stud\\OP\\Prac 1\\1\\bin\\Debug\\net5.0-windows\\result.txt");
        static Stopwatch stopwatch = new Stopwatch();
        static List<double> inter = new List<double>();
        public ProtectionModeWindow()
        {
            InitializeComponent();
        }
        private void InputField_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (InputField.Text.Length != VerifField.Text.Length)
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
                {
                    MessageBox.Show("Ви ввели некоректне слово");
                    attempts++;
                }
                inter.Clear();
                InputField.Text = "";
                if (counter == CountProtection.SelectedIndex + 3)
                {
                    InputField.IsEnabled = false;
                    DispField.Content = sa;
                    StatisticsBlock.Content = counter / attempts;
                    P1Field.Content = 1 - counter / attempts;
                    counter = 0;
                }
            }
        }
        private void calc()
        { 
            double f;
            double[] student = { 6.314, 2.92, 2.353, 2.132, 2.015, 1.943, 1.895, 1.86, 1.833, 1.813, 1.8, 1.782, 1.761, 1.75, 1.75, 1.74, 1.734, 1.725, 1.72 };
            double[] fisher = { 161, 19.0, 9.28, 6.39, 5.05, 4.28, 3.79, 3.44, 3.18, 2.98, 2.82, 2.69 };

            double ma, suma = 0;
            ma = inter.Sum() / (VerifField.Text.Length - 1);
            foreach (double del in inter)
                suma += Math.Pow(del - ma, 2);
            sa = suma / (VerifField.Text.Length - 2);

            string[] lines = sr.ReadToEnd().Split(';');
            sr.BaseStream.Position = 0;
            for (int i = 0; i < lines.Length - 1; i++)
            {
                double s, t;
                string[] str = sr.ReadLine().Split(';');
                f = Math.Max(double.Parse(str[1]), sa) / Math.Min(double.Parse(str[1]), sa);
                if (f > fisher[inter.Count - 1])
                {
                    attempts++;
                    MessageBox.Show("Дисперсії неоднорідні");
                    return;
                }

                s = Math.Sqrt((double.Parse(str[1]) + sa) * (inter.Count - 1) / (2 * inter.Count - 1));
                t = Math.Abs(double.Parse(str[0]) - ma) / s;
                if (t > student[inter.Count - 1])
                {
                    attempts++;
                    MessageBox.Show("Розбіжність не випадкова");
                    return;
                }
            }
            counter++;
            attempts++;
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