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

namespace _1
{
    public partial class MainWindow : Window
    {
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
            Window1 ow = new Window1();
            Hide();
            ow.Show();
        }
        private void B2_Click(object sender, RoutedEventArgs e)
        {
            Window2 ow = new Window2();
            Hide();
            ow.Show();
        }
        private void B3_Click(object sender, RoutedEventArgs e)
        {
            Window3 ow = new Window3();
            Hide();
            ow.Show();
        }

        private void B3_Copy_Click(object sender, RoutedEventArgs e)
        {
            Window4 ow = new Window4();
            Hide();
            ow.Show();
        }
    }
}
