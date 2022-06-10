using System.Windows;
using System.Data.SqlClient;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;

namespace _46
{
    public partial class Temporary : Window
    {
        string connectionStr = null;
        SqlConnection connection = null;
        SqlCommand command = null;
        public Temporary()
        {
            InitializeComponent();
            connectionStr = "Data Source = DMYTRO; Initial Catalog = Cursova; Integrated Security = True";
            if (MainWindow.b) //dealer
            {
                Changable.Content = "Фотографія";
                NewTown.IsEnabled = false;
                NewTown.Visibility = Visibility.Collapsed;
                Image.IsEnabled = true;
                Image.Visibility = Visibility.Visible;
            }
            else //client
            {
                Changable.Content = "Місто";
                Image.IsEnabled = false;
                Image.Visibility = Visibility.Collapsed;
                NewTown.IsEnabled = true;
                NewTown.Visibility = Visibility.Visible;
            }
        }
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            if (Equals(NewName.Text, "") || Equals(NewSurname.Text, "") || Equals(NewAdress.Text, "") || Equals(NewPhone.Text, "") || (ImageSave.Source == null && MainWindow.b) || (Equals(NewTown.Text, "") && !MainWindow.b))
            {
                MessageBox.Show("Заповніть усі обов'язкові поля");
                return;
            }

            connection = new SqlConnection(connectionStr);
            connection.Open();

            string strQ;
            if (MainWindow.b)
                strQ = $"SELECT COUNT(*) FROM Dealer WHERE (dbo.Dealer.Name = '{NewName.Text}') AND (dbo.Dealer.Surname = '{NewSurname.Text}') AND (dbo.Dealer.SecondName = '{NewSecondName.Text}');";
            else
                strQ = $"SELECT COUNT(*) FROM Clients WHERE (dbo.Clients.Name = '{NewName.Text}') AND (dbo.Clients.Surname = '{NewSurname.Text}') AND (dbo.Clients.SecondName = '{NewSecondName.Text}');";

            int amount = (int)new SqlCommand(strQ, connection).ExecuteScalar();
            if (amount != 0)
                MessageBox.Show("Користувач з даним ПІБ існує");
            else
            {
                if (MainWindow.b)
                {
                    strQ = $"SELECT COUNT(*) FROM Dealer";
                    amount = (int)new SqlCommand(strQ, connection).ExecuteScalar() + 3;
                }
                else
                {
                    strQ = $"SELECT COUNT(*) FROM Clients";
                    amount = (int)new SqlCommand(strQ, connection).ExecuteScalar() + 1;
                }

                if (MainWindow.b)
                {
                    strQ = $"INSERT INTO Dealer values('{NewName.Text}', '{NewSurname.Text}', '{NewSecondName.Text}', @image,'{NewAdress.Text}', '{NewPhone.Text}');";
                    command = new SqlCommand(strQ, connection);
                    command.Parameters.AddWithValue("@image", image);
                }
                else
                {
                    strQ = $"INSERT INTO Clients values('{NewName.Text}', '{NewSurname.Text}', '{NewSecondName.Text}', '{NewTown.Text}','{NewAdress.Text}', '{NewPhone.Text}');";
                    command = new SqlCommand(strQ, connection);
                }
                command.ExecuteNonQuery();

                NewName.Text = NewSurname.Text = NewSecondName.Text = NewAdress.Text = NewPhone.Text = NewTown.Text = "";
                ImageSave.Source = null;
                MessageBox.Show($"Ви зареєструвалися. Ваш серійний номер {amount}");
            }
            connection.Close();
        }
        byte[] image;
        private void DialogClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "pngs (*.png)|*.png|jpgs (*.jpg or *.jpeg)|*.jpg;*.jpeg";
            if (dialog.ShowDialog() == true)
            {
                image = File.ReadAllBytes(dialog.FileName);
                ImageSave.Source = GetImageFromByteArray(image);
            }
        }
        public static BitmapFrame GetImageFromByteArray(byte[] array) { return GetImageFromMemoryStream(new MemoryStream(array)); }
        public static BitmapFrame GetImageFromMemoryStream(MemoryStream memoryStream) { return BitmapFrame.Create(memoryStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad); }
        public static int serial = 0;
        private void AutoClick(object sender, RoutedEventArgs e)
        {
            if (Equals(Serial.Text, ""))
            {
                MessageBox.Show("Заповніть усі обов'язкові поля");
                return;
            }

            connection = new SqlConnection(connectionStr);
            connection.Open();
            string strQ;
            if (MainWindow.b)
                strQ = $"SELECT COUNT(*) FROM Dealer WHERE (dbo.Dealer.IDDealer = '{Serial.Text}');";
            else
                strQ = $"SELECT COUNT(*) FROM Clients WHERE (dbo.Clients.IDClient = '{Serial.Text}');";
            int amount = (int)new SqlCommand(strQ, connection).ExecuteScalar();
            if (amount == 0)
                MessageBox.Show("Користувач з даним серійним номером не існує");
            else
            {
                serial = int.Parse(Serial.Text);
                if (MainWindow.b)
                {
                    Dealer dl = new Dealer();
                    Hide();
                    dl.Show();
                }
                else
                {
                    Client cl = new Client();
                    Hide();
                    cl.Show();
                }
            }
            connection.Close();
        }
    }
}