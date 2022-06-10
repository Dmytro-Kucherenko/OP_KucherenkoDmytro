using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.IO;

namespace _46
{
    public partial class CarData : Window
    {
        string connectionStr = null;
        SqlConnection connection = null;
        SqlCommand command = null;
        public CarData()
        {
            InitializeComponent();
            connectionStr = "Data Source = DMYTRO; Initial Catalog = Cursova; Integrated Security = True";
            dataUpdate();

        }
        private void dataUpdate()
        {
            Ser.Content = "Номер " + Dealer.dt.Rows[Dealer.numb][0].ToString();
            NewModel.Text = Dealer.dt.Rows[Dealer.numb][2].ToString();
            NewRun.Text = Dealer.dt.Rows[Dealer.numb][3].ToString();
            NewPrice.Text = Dealer.dt.Rows[Dealer.numb][4].ToString();
            NewSum.Content = Math.Round(int.Parse(NewPrice.Text) * 0.05, 2);
            DateP.SelectedDate = (DateTime)Dealer.dt.Rows[Dealer.numb][5];
            image = (byte[])Dealer.dt.Rows[Dealer.numb][1];
            ImageSave.Source = Temporary.GetImageFromByteArray((byte[])Dealer.dt.Rows[Dealer.numb][1]);
        }
        private void NewAuto_Click(object sender, RoutedEventArgs e)
        {
            if (Equals(NewModel.Text, "") || Equals(NewRun.Text, "") || Equals(NewPrice.Text, "") || ImageSave.Source == null)
            {
                MessageBox.Show("Заповніть усі обов'язкові поля");
                return;
            }
            if (Dealer.DateCorrection(DateP))
            {
                MessageBox.Show("Заповніть дату коректно (Масимальний можливий час використання 10 років)");
                return;
            }
            if (int.Parse(NewRun.Text) >= 200000)
            {
                MessageBox.Show("Масимальний можливий пробіг 199999 км");
                return;
            }

            connection = new SqlConnection(connectionStr);
            connection.Open();

            string strQ = $"UPDATE dbo.Car SET dbo.Car.Model = '{NewModel.Text}', dbo.Car.Photo = @image, dbo.Car.Date = '{DateP.SelectedDate}', dbo.Car.Run = {NewRun.Text}, dbo.Car.Com = {NewSum.Content}, dbo.Car.Price = {NewPrice.Text} WHERE dbo.Car.IDCar = {Dealer.dt.Rows[Dealer.numb][0]};";
            command = new SqlCommand(strQ, connection);
            command.Parameters.AddWithValue("@image", image);
            command.ExecuteNonQuery();

            MessageBox.Show("Ви змінили дані автомобіля");
            connection.Close();
            Hide();
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
        public static BitmapFrame GetImageFromByteArray(byte[] array)
        {
            return GetImageFromMemoryStream(new MemoryStream(array));
        }
        public static BitmapFrame GetImageFromMemoryStream(MemoryStream memoryStream)
        {
            return BitmapFrame.Create(memoryStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
        }
        private void ExitClick(object sender, RoutedEventArgs e){ Hide(); }
        private void NewRun_KeyUp(object sender, KeyEventArgs e) { NewRun.Text = Dealer.NumberCorrection(NewRun.Text); }
        private void NewPrice_KeyUp(object sender, KeyEventArgs e)
        {
            NewPrice.Text = Dealer.NumberCorrection(NewPrice.Text);
            if (NewPrice.Text != "")
                NewSum.Content = Math.Round(double.Parse(NewPrice.Text) * 0.05, 2);
        }
    }
}