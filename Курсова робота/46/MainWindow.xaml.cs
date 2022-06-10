using System.Windows;
using System.Data.SqlClient;

namespace _46
{
    public partial class MainWindow : Window
    {
        public static bool b;
        string connectionStr = null;
        SqlConnection connection = null;
        public MainWindow()
        {
            connectionStr = "Data Source = DMYTRO; Initial Catalog = Cursova; Integrated Security = True";
            InitializeComponent();
            StatsCheck();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void Dealer_Click(object sender, RoutedEventArgs e)
        {
            b = true;
            Temporary temp = new Temporary();
            Hide();
            temp.Show();
        }
        private void Client_Click(object sender, RoutedEventArgs e)
        {
            b = false;
            Temporary temp = new Temporary();
            Hide();
            temp.Show();
        }
        private void StatsCheck()
        {
            connection = new SqlConnection(connectionStr);
            connection.Open();

            string strQ = $"SELECT IDDealer, SUM(dbo.Car.Price) FROM dbo.Car WHERE dbo.Car.Status = 0 GROUP BY IDDealer ORDER BY SUM(dbo.Car.Price) DESC;";
            int dealer = (int)new SqlCommand(strQ, connection).ExecuteScalar(); 
            strQ = $"SELECT (dbo.Dealer.Surname + ' ' + dbo.Dealer.Name + ' ' + dbo.Dealer.SecondName) FROM dbo.Dealer WHERE dbo.Dealer.IDDealer = {dealer};";
            string res = new SqlCommand(strQ, connection).ExecuteScalar().ToString();
            BiggestSum.Content = $"Дилер за максимальною \nсумою продажу: \n{res}";

            strQ = $"SELECT IDDealer, SUM(dbo.Car.Com) FROM dbo.Car WHERE dbo.Car.Status = 0 GROUP BY IDDealer ORDER BY SUM(dbo.Car.Com) DESC;";
            dealer = (int)new SqlCommand(strQ, connection).ExecuteScalar();
            strQ = $"SELECT (dbo.Dealer.Surname + ' ' + dbo.Dealer.Name + ' ' + dbo.Dealer.SecondName) FROM dbo.Dealer WHERE dbo.Dealer.IDDealer = {dealer};";
            res = new SqlCommand(strQ, connection).ExecuteScalar().ToString();
            BiggestCom.Content = $"Дилер за максимальною \nсумою комісійних: \n{res}";

            strQ = $"SELECT IDCar FROM dbo.Car WHERE dbo.Car.Status = 0 Order By dbo.Car.Price DESC;";
            int car = (int)new SqlCommand(strQ, connection).ExecuteScalar();
            strQ = $"SELECT IDCLient FROM dbo.Contract WHERE IDCar = {car};";
            car = (int)new SqlCommand(strQ, connection).ExecuteScalar();
            strQ = $"SELECT (dbo.Clients.Surname + ' ' + dbo.Clients.Name + ' ' + dbo.Clients.SecondName) FROM dbo.Clients WHERE dbo.Clients.IDClient = {car};";
            res = new SqlCommand(strQ, connection).ExecuteScalar().ToString();
            BiggestPrice.Content = $"Клієнт, що придбав \nнайдорожчий авто: \n{res}";

            connection.Close();
        }
        private void Stats_Click(object sender, RoutedEventArgs e)
        {
            Stats sw = new Stats();
            Hide();
            sw.Show();
        }
    }
}