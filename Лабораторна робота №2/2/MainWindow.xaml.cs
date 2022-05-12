using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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

namespace _2
{
    public partial class MainWindow : Window
    {
        static BrushConverter bc = new BrushConverter();
        static Label output;
        static int counter = 0;
        static bool b = false;
        static Button[,] g;
        static List<(string, string)> students = new List<(string, string)>();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void B1_Click(object sender, RoutedEventArgs e)
        {
            Window nw = new Window();
            Hide();
            nw.Width = 1240;
            nw.Height = 480;
            nw.FontSize = 22;
            nw.Title = "Зчитування даних";
            nw.Background = (Brush)bc.ConvertFrom("#FFB8B8B8");
            nw.ResizeMode = ResizeMode.NoResize;

            Grid mygrid = new Grid();
            mygrid.ShowGridLines = false;
            mygrid.Width = Math.Min(nw.Width * 1.0 - 40, (nw.Height - 80) * 3.0);
            mygrid.Height = Math.Min(nw.Height * 1.0 - 80, (nw.Width - 40) / 3.0);
            mygrid.HorizontalAlignment = HorizontalAlignment.Center;
            mygrid.VerticalAlignment = VerticalAlignment.Center;

            string[] text = new string[5] { "Номер залікової книжки", "ПІП", "Факультет", "Особиста інформація \n(необов'язково)", "Номер залікової книжки" };
            TextBox[] data = new TextBox[5];
            for (int k = 0; k < 5; k++)
            {
                int i = data.ToList().IndexOf(data[k]);
                data[i] = new TextBox();
                data[i].BorderBrush = Brushes.Black;
                data[i].Text = text[i];
                data[i].IsMouseCapturedChanged += (s, e) =>
                {
                    if (data[i].Text == text[i])
                        data[i].Text = "";
                };
                data[i].LostFocus += (s, e) =>
                {
                    if (data[i].Text == "")
                        data[i].Text = text[i];
                };
            }

            Button[] func = new Button[4];
            for (int i = 0; i < 4; i++)
            {
                func[i] = new Button();
                func[i].BorderBrush = Brushes.Black;
            }
            func[0].Content = "Додати нового студента";
            func[1].Content = "Завантажити у файл";
            func[2].Content = "До головного вікна";
            func[2].Background = (Brush)bc.ConvertFrom("#FFFFAAAA");
            func[3].Content = "Видалити за номером \nзал. кн.";

            Label stats = new Label();
            stats.Content = "Додайте нового студента";
            stats.Background = Brushes.White;
            stats.BorderBrush = Brushes.Black;
            stats.BorderThickness = new Thickness(1);


            func[0].Click += (s, e) =>
            {
                if (data[0].Text == text[0] || data[1].Text == text[1] || data[2].Text == text[2])
                {
                    MessageBox.Show("Введіть усі обов'язкові дані");
                }
                else
                {
                    if (stats.Content.ToString() == "Додайте нового студента")
                        stats.Content = "";
                    if (data[3].Text != "Особиста інформація \n(необов'язково)")
                    {
                        students.Add((data[0].Text, $", {data[1].Text}, {data[2].Text}, {data[3].Text}"));
                        stats.Content += $"{data[0].Text}, {data[1].Text}, {data[2].Text}, {data[3].Text}\n";
                    }
                    else
                    {
                        students.Add((data[0].Text, $", {data[1].Text}, {data[2].Text}"));
                        stats.Content += $"{data[0].Text}, {data[1].Text}, {data[2].Text}\n";
                    }
                }
            };
            func[1].Click += (s, e) =>
            {
                StreamWriter sr = new StreamWriter(@"result.txt");
                sr.WriteLine(stats.Content.ToString());
                sr.Close();
                MessageBox.Show("Файл збережено");
            };
            func[2].Click += (s, e) => { new MainWindow().Show(); nw.Hide(); };
            func[3].Click += (s, e) =>
            {
                if (data[4].Text == "Номер залікової книжки")
                    MessageBox.Show("Введіть номер залікової книжки");
                else
                {
                    bool b = true;
                    foreach ((string, string) el in students)
                        if (Equals(el.Item1, data[4].Text))
                        {
                            students.Remove(el);
                            b = false;
                            break;
                        }
                    if (b)
                        MessageBox.Show("Студента з таким номером зал. кн. не існує");
                    else
                    {
                        stats.Content = "";
                        foreach ((string, string) el in students)
                            stats.Content += $"{el.Item1}{el.Item2}\n";
                    }
                }
            };

            RowDefinition[] rows = new RowDefinition[4];
            ColumnDefinition[] cols = new ColumnDefinition[4];

            for (int i = 0; i < 4; i++)
            {
                rows[i] = new RowDefinition();
                mygrid.RowDefinitions.Add(rows[i]);
            }
            for (int i = 0; i < 4; i++)
            {
                cols[i] = new ColumnDefinition();
                mygrid.ColumnDefinitions.Add(cols[i]);
            }
            for (int i = 0; i < 4; i++)
            {
                Grid.SetRow(data[i], i);
                Grid.SetColumn(data[i], 0);
            }
            Grid.SetRow(data[4], 3);
            Grid.SetColumn(data[4], 2);

            Grid.SetRow(stats, 0);
            Grid.SetColumn(stats, 1);
            Grid.SetRowSpan(stats, 3);
            Grid.SetColumnSpan(stats, 2);

            for (int i = 0; i < 4; i++)
            {
                Grid.SetRow(func[i], i);
                Grid.SetColumn(func[i], 3);
            }

            for (int i = 0; i < 5; i++)
                mygrid.Children.Add(data[i]);
            for (int i = 0; i < 4; i++)
                mygrid.Children.Add(func[i]);
            mygrid.Children.Add(stats);

            nw.Content = mygrid;
            nw.Show();
            nw.Closing += Closing_Click;
        }
        private void B2_Click(object sender, RoutedEventArgs e)
        {
            Window nw = new Window();
            Hide();
            nw.Width = 740;
            nw.Height = 580;
            nw.FontSize = 22;
            nw.Title = "Хрестики - нулики";
            nw.Background = (Brush)bc.ConvertFrom("#FFB8B8B8");
            nw.ResizeMode = ResizeMode.NoResize;

            Grid mygrid = new Grid();
            mygrid.ShowGridLines = false;
            mygrid.Width = Math.Min(nw.Width * 1.0 - 40, (nw.Height - 80) * 1.4);
            mygrid.Height = Math.Min(nw.Height * 1.0 - 80, (nw.Width - 40) / 1.4);
            mygrid.HorizontalAlignment = HorizontalAlignment.Center;
            mygrid.VerticalAlignment = VerticalAlignment.Center;

            g = new Button[5, 5];
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    g[i, j] = new Button();
                    g[i, j].Background = Brushes.White;
                    g[i, j].BorderBrush = Brushes.Black;
                    g[i, j].Click += x0_Click;
                    g[i, j].Content = "";
                }
            Button exit = new Button();
            exit.Click += (s, e) => { new MainWindow().Show(); nw.Hide(); };
            exit.Content = "До головного \nвікна";
            exit.Background = (Brush)bc.ConvertFrom("#FFFFAAAA");

            output = new Label();
            output.Background = Brushes.White;
            output.HorizontalAlignment = HorizontalAlignment.Center;
            output.VerticalAlignment = VerticalAlignment.Center;
            output.Content = "Ходить X";

            RowDefinition[] rows = new RowDefinition[5];
            ColumnDefinition[] cols = new ColumnDefinition[7];

            for (int i = 0; i < 5; i++)
            {
                rows[i] = new RowDefinition();
                mygrid.RowDefinitions.Add(rows[i]);
            }
            for (int i = 0; i < 7; i++)
            {
                cols[i] = new ColumnDefinition();
                mygrid.ColumnDefinitions.Add(cols[i]);
            }

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    Grid.SetRow(g[i, j], i);
                    Grid.SetColumn(g[i, j], j);
                }

            Grid.SetRow(exit, 4);
            Grid.SetColumn(exit, 5);
            Grid.SetColumnSpan(exit, 2);

            Grid.SetRow(output, 0);
            Grid.SetColumn(output, 5);
            Grid.SetColumnSpan(output, 2);

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    mygrid.Children.Add(g[i, j]);
                }
            mygrid.Children.Add(exit);
            mygrid.Children.Add(output);

            nw.Content = mygrid;
            nw.Closing += Closing_Click;
            nw.Show();
        }
        private void x0_Click(object sender, RoutedEventArgs e)
        {
            if (!b)
            {
                counter++;
                if (counter % 2 != 0)
                {
                    output.Content = "Ходить 0";
                    ((Button)sender).Content = "X";
                }
                else
                {
                    output.Content = "Ходить X";
                    ((Button)sender).Content = "0";
                }
                ((Button)sender).IsEnabled = false;
            }
            for (int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    if (Equals(g[i, j].Content, g[i + 1, j].Content) && Equals(g[i + 1, j].Content, g[i + 2, j].Content) &&
                        Equals(g[i + 2, j].Content, g[i + 3, j].Content) && g[i, j].Content.ToString() != "")
                    {
                        b = true;
                        break;
                    }
                    if (j < 2 && Equals(g[i, j].Content, g[i + 1, j + 1].Content) && Equals(g[i + 1, j + 1].Content, g[i + 2, j + 2].Content) &&
                    Equals(g[i + 2, j + 2].Content, g[i + 3, j + 3].Content) && g[i, j].Content.ToString() != "")
                    {
                        b = true;
                        break;
                    }
                    else if (j > 2 && Equals(g[i, j].Content, g[i + 1, j - 1].Content) && Equals(g[i + 1, j - 1].Content, g[i + 2, j - 2].Content) &&
                    Equals(g[i + 2, j - 2].Content, g[i + 3, j - 3].Content) && g[i, j].Content.ToString() != "")
                    {
                        b = true;
                        break;
                    }
                }
            }
            for (int i = 0; i <= 4; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    if (Equals(g[i, j].Content, g[i, j + 1].Content) && Equals(g[i, j + 1].Content, g[i, j + 2].Content) &&
                        Equals(g[i, j + 2].Content, g[i, j + 3].Content) && g[i, j].Content.ToString() != "")
                    {
                        b = true;
                        break;
                    }
                }
            }
            if (b)
            {
                b = false;
                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 5; j++)
                    {
                        g[i, j].IsEnabled = true;
                        g[i, j].Content = "";
                    }
                if (counter % 2 != 0)
                    MessageBox.Show("Виграли хрестики!");
                else
                    MessageBox.Show("Виграли нулики!");
                counter = 0;
                output.Content = "Ходить X";
            }
        }
        private void B3_Click(object sender, RoutedEventArgs e)
        {
            Window nw = new Window();
            Hide();
            nw.Width = 440;
            nw.Height = 680;
            nw.FontSize = 22;
            nw.Title = "Калькултор";
            nw.Background = (Brush)bc.ConvertFrom("#FFB8B8B8");
            nw.ResizeMode = ResizeMode.NoResize;

            Grid mygrid = new Grid();
            mygrid.ShowGridLines = false;
            mygrid.Width = Math.Min(nw.Width * 1.0 - 40, (nw.Height - 80) / 1.5);
            mygrid.Height = Math.Min(nw.Height * 1.0 - 80, (nw.Width - 40) * 1.5);
            mygrid.HorizontalAlignment = HorizontalAlignment.Center;
            mygrid.VerticalAlignment = VerticalAlignment.Center;

            g = new Button[5, 4];
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 4; j++)
                {
                    g[i, j] = new Button();
                    g[i, j].Background = Brushes.White;
                    g[i, j].BorderBrush = Brushes.Black;
                }
            int c = 0;
            for (int i = 3; i > 0; i--)
                for (int j = 0; j < 3; j++)
                {
                    c++;
                    g[i, j].Content = c;
                    g[i, j].Click += (s, e) =>
                     {
                         char[] ch = output.Content.ToString().ToCharArray();
                         if ((ch[ch.Length - 1] == '0' && ch.Length == 1) || (ch[ch.Length - 1] == '0' && (!char.IsNumber(ch[ch.Length - 2]) || ch[ch.Length - 2] == ',')))
                         {
                             output.Content = "";
                             ch[ch.Length - 1] = ((Button)s).Content.ToString()[0];
                             foreach (char el in ch)
                                 output.Content += el.ToString();
                         }
                         else
                             output.Content += ((Button)s).Content.ToString();
                     };
                }
            g[4, 1].Content = "0";
            g[4, 1].Click += (s, e) =>
            {
                char[] ch = output.Content.ToString().ToCharArray();
                if ((ch[ch.Length - 1] == '0' && ch.Length == 1) || (ch[ch.Length - 1] == '0' && (!char.IsNumber(ch[ch.Length - 2]) || ch[ch.Length - 2] == ',')))
                {
                    output.Content = "";
                    ch[ch.Length - 1] = ((Button)s).Content.ToString()[0];
                    foreach (char el in ch)
                        output.Content += el.ToString();
                }
                else
                    output.Content += ((Button)s).Content.ToString();
            };

            g[0, 0].Content = "Exit";
            g[0, 0].Background = (Brush)bc.ConvertFrom("#FFFFAAAA");
            g[0,0].Click += (s, e) => { new MainWindow().Show(); nw.Hide(); };

            g[0, 1].Content = "=";
            g[0, 1].Click += (s, e) => 
            {
                if (output.Content.ToString()[output.Content.ToString().Length - 1] == '-') { }
                else if (output.Content.ToString().Where(x => x == '-').Count() == 2)
                {
                    string[] str = output.Content.ToString().Split('-');
                    output.Content = Math.Round(-1 * (double.Parse(str[1]) + double.Parse(str[2])), 3);
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
            };

            g[0, 2].Content = "C";
            g[0, 2].Click += (s, e) => { output.Content = "0"; };

            g[0, 3].Content = "back";
            g[0, 3].Click += (s, e) => 
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
            };

            g[1, 3].Content = "/";
            g[2, 3].Content = "x";
            g[3, 3].Content = "-";
            g[4, 3].Content = "+";
            for (int i = 1; i < 5; i++)
            {
                g[i, 3].Click += znak_click;
            }

            g[4, 2].Content = ",";
            g[4, 2].Click += (s, e) =>
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
                    else if (i == 0)
                        output.Content += ",";
                }
            };

            g[4, 0].Content = "+/-";
            g[4, 0].Click += (s, e) =>
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
            };

            output = new Label();
            output.Background = Brushes.DarkGray;
            output.Content = "0";

            RowDefinition[] rows = new RowDefinition[6];
            ColumnDefinition[] cols = new ColumnDefinition[4];

            for (int i = 0; i < 6; i++)
            {
                rows[i] = new RowDefinition();
                mygrid.RowDefinitions.Add(rows[i]);
            }
            for (int i = 0; i < 4; i++)
            {
                cols[i] = new ColumnDefinition();
                mygrid.ColumnDefinitions.Add(cols[i]);
            }

            for (int i = 1; i < 6; i++)
                for (int j = 0; j < 4; j++)
                {
                    Grid.SetRow(g[i - 1, j], i);
                    Grid.SetColumn(g[i - 1, j], j);
                }

            Grid.SetRow(output, 0);
            Grid.SetColumn(output, 0);
            Grid.SetColumnSpan(output, 4);

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 4; j++)
                {
                    mygrid.Children.Add(g[i, j]);
                }
            mygrid.Children.Add(output);

            nw.Content = mygrid;
            nw.Closing += Closing_Click;
            nw.Show();
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
        private void B3_Copy_Click(object sender, RoutedEventArgs e)
        {
            Window nw = new Window();
            Hide();
            nw.Width = 940;
            nw.Height = 380;
            nw.FontSize = 28;
            nw.Title = "Дані";
            nw.Background = (Brush)bc.ConvertFrom("#FFB8B8B8");
            nw.ResizeMode = ResizeMode.NoResize;

            Grid mygrid = new Grid();
            mygrid.ShowGridLines = false;
            mygrid.Width = Math.Min(nw.Width * 1.0 - 40, (nw.Height - 80) * 3.0);
            mygrid.Height = Math.Min(nw.Height * 1.0 - 80, (nw.Width - 40) / 3.0);
            mygrid.HorizontalAlignment = HorizontalAlignment.Center;
            mygrid.VerticalAlignment = VerticalAlignment.Center;

            Label[] output = new Label[3];
            for (int i = 0; i < 3; i++)
            {
                output[i] = new Label();
                output[i].Background = Brushes.White;
                output[i].BorderBrush = Brushes.Black;
                output[i].BorderThickness = new Thickness(1);
            }
            output[0].Content = "Кучеренко Дмитро Олегович";
            output[1].Content = "КП-11";
            output[2].Content = "06.05.2022";
            Button exit = new Button();
            exit.Click += (s, e) => { new MainWindow().Show(); nw.Hide(); };
            exit.Content = "ДО ГОЛОВНОГО ВІКНА";
            exit.Background = (Brush)bc.ConvertFrom("#FFFFAAAA");

            RowDefinition[] rows = new RowDefinition[3];
            ColumnDefinition[] cols = new ColumnDefinition[2];

            for (int i = 0; i < 3; i++)
            {
                rows[i] = new RowDefinition();
                mygrid.RowDefinitions.Add(rows[i]);
            }
            for (int i = 0; i < 2; i++)
            {
                cols[i] = new ColumnDefinition();
                mygrid.ColumnDefinitions.Add(cols[i]);
            }

            for (int i = 1; i < 3; i++)
            {
                Grid.SetRow(output[i], i);
                Grid.SetColumn(output[i], 0);
            }
            Grid.SetRow(exit, 0);
            Grid.SetColumn(exit, 1);
            Grid.SetRowSpan(exit, 3);

            for (int i = 0; i < 3; i++)
            {
                mygrid.Children.Add(output[i]);
            }
            mygrid.Children.Add(exit);

            nw.Content = mygrid;
            nw.Closing += Closing_Click;
            nw.Show();
        }
        private void Closing_Click(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}