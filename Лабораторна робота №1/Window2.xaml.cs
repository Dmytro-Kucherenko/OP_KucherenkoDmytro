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
    public partial class Window2 : Window
    {
        static int counter = 0;
        static bool b = false;
        static Button[,] g;
        public Window2()
        {
            InitializeComponent();
            g = new Button[5, 5]
            {
                { _1, _2, _3, _4, _5 },
                { _6, _7, _8, _9, _10 },
                { _11, _12, _13, _14, _15 },
                { _16, _17, _18, _19, _20 },
                { _21, _22, _23, _24, _25 }
            };
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow ow = new MainWindow();
            Hide();
            ow.Show();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            if (!b)
            {
                counter++;
                if (counter % 2 != 0)
                {
                    output.Content = "Ходить 0";
                    button.Content = "X";
                }
                else
                {
                    output.Content = "Ходить X";
                    button.Content = "0";
                }
                button.IsEnabled = false;
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
    }
}