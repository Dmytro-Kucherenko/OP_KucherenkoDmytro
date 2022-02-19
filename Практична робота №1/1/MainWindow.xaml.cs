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

namespace Prj_Soft_Protection
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void StudyModeBtn_Click(object sender, RoutedEventArgs e)
        {
            StudyModeWindow studyModeWindow = new StudyModeWindow();
            studyModeWindow.Show();
            Hide();
        }
        private void ProtectionModeBtn_Click(object sender, RoutedEventArgs e)
        {
            ProtectionModeWindow protectionModeWindow = new ProtectionModeWindow();
            protectionModeWindow.Show();
            Hide();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
