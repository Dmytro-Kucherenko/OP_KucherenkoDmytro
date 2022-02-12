using System.Collections.Generic;
using System.Windows;
using System.IO;

namespace _1
{
    public partial class Window1 : Window
    {
        static List<(string, string)> students = new List<(string, string)>();
        public Window1()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow ow = new MainWindow();
            Hide();
            ow.Show();
        }
        private void num_change(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (num.Text == "Номер залікової книжки")
                num.Text = "";
        }
        private void num_unchange(object sender, RoutedEventArgs e)
        {
            if (num.Text == "")
                num.Text = "Номер залікової книжки";
        }
        private void pip_change(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (pip.Text == "ПІП")
                pip.Text = "";
        }
        private void pip_unchange(object sender, RoutedEventArgs e)
        {
            if (pip.Text == "")
                pip.Text = "ПІП";
        }
        private void fuc_change(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (fuc.Text == "Факультет")
                fuc.Text = "";
        }
        private void fuc_unchange(object sender, RoutedEventArgs e)
        {
            if (fuc.Text == "")
                fuc.Text = "Факультет";
        }
        private void ad_change(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ad.Text == "Особиста інформація (необов'язково)")
                ad.Text = "";
        }
        private void ad_unchange(object sender, RoutedEventArgs e)
        {
            if (ad.Text == "")
                ad.Text = "Особиста інформація (необов'язково)";
        }
        private void dt_change(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (dt.Text == "Номер залікової книжки")
                dt.Text = "";
        }
        private void dt_unchange(object sender, RoutedEventArgs e)
        {
            if (dt.Text == "")
                dt.Text = "Номер залікової книжки";
        }

        private void st_Click(object sender, RoutedEventArgs e)
        {
            
            if (num.Text == "Номер залікової книжки" || pip.Text == "ПІП"|| fuc.Text == "Факультет")
            {
                MessageBox.Show("Введіть усі обов'язкові дані");
            }
            else 
            {
                if (output.Content.ToString() == "Додайте нового студента")
                    output.Content = "";
                if (ad.Text != "Особиста інформація (необов'язково)")
                {
                    students.Add((num.Text, $", {pip.Text}, {fuc.Text}, {ad.Text}"));
                    output.Content += $"{num.Text}, {pip.Text}, {fuc.Text}, {ad.Text}\n";
                }
                else
                {
                    students.Add((num.Text, $", {pip.Text}, {fuc.Text}"));
                    output.Content += $"{num.Text}, {pip.Text}, {fuc.Text}\n";
                }
            }
        }
        private void del_Click(object sender, RoutedEventArgs e)
        {
            if (dt.Text == "Номер залікової книжки")
                MessageBox.Show("Введіть номер залікової книжки");
            else
            {
                bool b = true;
                foreach ((string, string) el in students)
                    if (Equals(el.Item1, dt.Text))
                    {
                        students.Remove(el);
                        b = false;
                        break;
                    }
                if (b)
                    MessageBox.Show("Студента з таким номером зал. кн. не існує");
                else
                {
                    output.Content = "";
                    foreach ((string, string) el in students)
                        output.Content += $"{el.Item1}{el.Item2}\n";
                }
            }
        }
        private void load_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter sr = new StreamWriter(@"result.txt");
            sr.WriteLine(output.Content.ToString());
            sr.Close();
            MessageBox.Show("Файл збережено");
        }
    }
}