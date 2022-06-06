using System.Windows;
using System.Data;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.IO;

namespace _46
{
    public partial class DealerData : Window
    {
        string connectionStr = null;
        SqlConnection connection = null;
        SqlCommand command = null;
        SqlDataAdapter adapter = null;
        DataTable dt;
        public DealerData()
        {
            InitializeComponent();
            connectionStr = "Data Source = DMYTRO; Initial Catalog = Cursova; Integrated Security = True";
            dataUpdate();
        }
        private void ExitClick(object sender, RoutedEventArgs e) { Hide(); }
        private void dataUpdate()
        {
            connection = new SqlConnection(connectionStr);
            connection.Open();

            string strQ = $"SELECT*FROM dbo.Dealer WHERE dbo.Dealer.IDDealer = {Temporary.serial};";
            adapter = new SqlDataAdapter(strQ, connection);
            dt = new DataTable("Наявні автомобілі");
            adapter.Fill(dt);

            Name.Text = dt.Rows[0][1].ToString();
            Surname.Text = dt.Rows[0][2].ToString();
            SecondName.Text = dt.Rows[0][3].ToString();
            Adress.Text = dt.Rows[0][5].ToString();
            Phone.Text = dt.Rows[0][6].ToString();
            image = (byte[])dt.Rows[0][4];
            ImageSel.Source = Temporary.GetImageFromByteArray((byte[])dt.Rows[0][4]);

            connection.Close();
        }
        byte[] image;
        private void Photo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "pngs (*.png)|*.png|jpgs (*.jpg or *.jpeg)|*.jpg;*.jpeg";
            if (dialog.ShowDialog() == true)
            {
                image = File.ReadAllBytes(dialog.FileName);
                ImageSel.Source = GetImageFromByteArray(image);
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
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Equals(Name.Text, "") || Equals(Surname.Text, "") || Equals(Adress.Text, "") || Equals(Phone.Text, "") || ImageSel.Source == null)
            {
                MessageBox.Show("Заповніть усі обов'язкові поля");
                return;
            }

            connection = new SqlConnection(connectionStr);
            connection.Open();

            string strQ = $"SELECT COUNT(*) FROM Dealer WHERE (dbo.Dealer.Name = '{Name.Text}') AND (dbo.Dealer.Surname = '{Surname.Text}') AND (dbo.Dealer.SecondName = '{SecondName.Text}') AND (dbo.Dealer.IDDealer != {Temporary.serial});";
            int amount = (int)new SqlCommand(strQ, connection).ExecuteScalar();

            if (amount != 0)
                MessageBox.Show("Користувач з даним ПІБ існує");
            else
            {
                strQ = $"UPDATE dbo.Dealer SET dbo.Dealer.Name = '{Name.Text}', dbo.Dealer.Surname ='{Surname.Text}', dbo.Dealer.SecondName = '{SecondName.Text}', dbo.Dealer.Photo = @image, dbo.Dealer.Adress = '{Adress.Text}', dbo.Dealer.Phone = '{Phone.Text}' WHERE dbo.Dealer.IDDealer = {Temporary.serial};";
                command = new SqlCommand(strQ, connection);
                command.Parameters.AddWithValue("@image", image);
                command.ExecuteNonQuery();
                MessageBox.Show("Ви змінили дані профіля");
            }
            connection.Close();
        }
    }
}
