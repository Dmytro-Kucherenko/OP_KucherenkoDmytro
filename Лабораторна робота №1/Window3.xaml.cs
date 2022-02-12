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

namespace _1
{
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow ow = new MainWindow();
            Hide();
            ow.Show();
        }
        private void Num_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            char[] ch = output.Content.ToString().ToCharArray();
            if ((ch[ch.Length - 1] == '0' && ch.Length == 1) || (ch[ch.Length - 1] == '0' && (!char.IsNumber(ch[ch.Length - 2]) || ch[ch.Length - 2] == ',')))
            {
                output.Content = "";
                ch[ch.Length - 1] = button.Content.ToString()[0];
                foreach (char s in ch)
                    output.Content += s.ToString();
            }
            else
                output.Content += button.Content.ToString();
        }
        private void C_Click(object sender, RoutedEventArgs e)
        {
            output.Content = "0";
        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            char[] ch = output.Content.ToString().ToCharArray();
            if (!char.IsNumber(ch[0]) && ch.Length == 2)
                output.Content = "0";
            else if (output.Content.ToString() != "0" && output.Content.ToString().Length != 1)
            {
                output.Content = "";
                for (int i = 0; i < ch.Length - 1; i++)
                    output.Content += ch[i].ToString();
            }
            else
                output.Content = "0";
        }
        private void negative_Click(object sender, RoutedEventArgs e)
        {
            char[] ch = output.Content.ToString().ToCharArray();
            output.Content = "";
            int n = 0;
            for (int i = ch.Length - 1; i >= 0; i--)
            {
                if (ch[i] == '+')
                {
                    ch[i] = '-';
                    break;
                }
                else if (ch[i] == '-' && i != 0)
                {
                    ch[i] = '+';
                    break;
                }
                else if (i == 0 && ch[0] == '-')
                    n = 1;
                else if (i == 0 && ch[0] != '0')
                    output.Content += "-";

            }
            for (int i = n; i < ch.Length; i++)
                output.Content += ch[i].ToString();
        }
        private void zap_Click(object sender, RoutedEventArgs e)
        {
            char[] ch = output.Content.ToString().ToCharArray();
            for (int i = ch.Length - 1; i >= 0; i--)
            {
                if (ch[i] == ',')
                    break;
                else if (!char.IsNumber(ch[i]) && i != ch.Length - 1)
                {
                    output.Content += ",";
                    break;
                } 
                else if(i == 0)
                    output.Content += ",";
            }
        }
        private void znak_click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            bool f = true;
            char[] ch = output.Content.ToString().ToCharArray();
            for (int i = ch.Length - 1; i >= 0; i--)
            {
                if (!char.IsNumber(ch[i]) && ch[i] != ',' && i != 0)
                {
                    if (!char.IsNumber(ch[ch.Length - 1]))
                    {
                        ch[i] = button.Content.ToString()[0];
                        output.Content = "";
                        for (int j = 0; j < ch.Length; j++)
                            output.Content += ch[j].ToString();
                    }
                    else
                        f = false;
                }
            }
            if (f) 
            {
                if (char.IsNumber(ch[ch.Length - 1]))
                    output.Content += button.Content.ToString();
                else if (ch[ch.Length - 1] != ',')
                {
                    output.Content = "";
                    for (int i = 0; i < ch.Length - 1; i++)
                        output.Content += ch[i].ToString();
                    output.Content += button.Content.ToString();
                }
            }
        }
        private void equals_Click(object sender, RoutedEventArgs e)
        {
            if (output.Content.ToString()[output.Content.ToString().Length - 1] == '-') { }
            else if (output.Content.ToString().Where(x => x == '-').Count() == 2)
            {
                string[] str = output.Content.ToString().Split('-');
                output.Content =Math.Round( -1 * (double.Parse(str[1]) + double.Parse(str[2])), 3);
            }
            else if (output.Content.ToString().Where(x => x == '-').Count() == 1 && output.Content.ToString()[0] != '-')
            {
                string[] str = output.Content.ToString().Split('-');
                output.Content = Math.Round(double.Parse(str[0]) - double.Parse(str[1]), 3);
            }
            else
            {
                char[] c = new char[3] { '+', 'x', '/' };
                for (int i = 0; i < 3; i++)
                {
                    if (output.Content.ToString().Contains(c[i]))
                    {
                        string[] str = output.Content.ToString().Split(c[i]);
                        if (output.Content.ToString()[output.Content.ToString().Length - 1] != c[i])
                        {
                            switch (c[i])
                            {
                                case '+': output.Content = Math.Round(double.Parse(str[0]) + double.Parse(str[1]), 3); break;
                                case 'x': output.Content = Math.Round(double.Parse(str[0]) * double.Parse(str[1]), 3); break;
                                case '/': output.Content = Math.Round(double.Parse(str[0]) / double.Parse(str[1]), 3); break;
                            }
                        }
                    }
                }
            }
        }
    }
}