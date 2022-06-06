using System.Windows;

namespace _46
{
    public partial class FeedbackW : Window
    {
        static public bool f = true;
        static public string fd;
        public FeedbackW()
        {
            InitializeComponent();
            Model.Content = Client.dt.Rows[Client.index][2].ToString();
            Run.Content = Client.dt.Rows[Client.index][3].ToString();
            Price.Content = Client.dt.Rows[Client.index][4].ToString();
            if (Client.dt.Rows[Client.index][5].ToString().Contains(" "))
                Date.Content = Client.dt.Rows[Client.index][5].ToString().Substring(0, 9);
            else
                Date.Content = Client.dt.Rows[Client.index][5].ToString().Substring(0, 10);

            Surname.Content = Client.dt.Rows[Client.index][6].ToString();
            Name.Content = Client.dt.Rows[Client.index][7].ToString();
            Phone.Content = Client.dt.Rows[Client.index][8].ToString();
        }
        public static string res = "";
        private void Buy_Click(object sender, RoutedEventArgs e) 
        {
            fd = Feedbacks.Text;
            fd = Client.FeedbackCorrection(fd);
            Hide();
        }
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            f = false;
            Hide();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            f = false;
            Hide();
        }
    }
}
