using System.Windows;
using System.Data;
using System.Data.SqlClient;

namespace _46
{
    public partial class ClientData : Window
    {
        string connectionStr = null;
        SqlConnection connection = null;
        SqlCommand command = null;
        SqlDataAdapter adapter = null;
        DataTable dt;
        public ClientData()
        {
            InitializeComponent();
            connectionStr = "Data Source = DMYTRO; Initial Catalog = Cursova; Integrated Security = True";
            dataUpdate();
        }
        private void dataUpdate()
        {
            connection = new SqlConnection(connectionStr);
            connection.Open();

            string strQ = $"SELECT*FROM dbo.Clients WHERE dbo.Clients.IDClient = {Temporary.serial};";
            adapter = new SqlDataAdapter(strQ, connection);
            dt = new DataTable("Наявні автомобілі");
            adapter.Fill(dt);

            Name.Text = dt.Rows[0][1].ToString();
            Surname.Text = dt.Rows[0][2].ToString();
            SecondName.Text = dt.Rows[0][3].ToString();
            Town.Text = dt.Rows[0][4].ToString();
            Adress.Text = dt.Rows[0][5].ToString();
            Phone.Text = dt.Rows[0][6].ToString();

            connection.Close();
        }
        private void ExitClick(object sender, RoutedEventArgs e) { Hide(); }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Equals(Name.Text, "") || Equals(Surname.Text, "") || Equals(Adress.Text, "") || Equals(Phone.Text, "")  || Equals(Town.Text, ""))
            {
                MessageBox.Show("Заповніть усі обов'язкові поля");
                return;
            }

            connection = new SqlConnection(connectionStr);
            connection.Open();

            string strQ = $"SELECT COUNT(*) FROM Clients WHERE (dbo.Clients.Name = '{Name.Text}') AND (dbo.Clients.Surname = '{Surname.Text}') AND (dbo.Clients.SecondName = '{SecondName.Text}') AND (dbo.Clients.IDClient != {Temporary.serial});";
            int amount = (int)new SqlCommand(strQ, connection).ExecuteScalar();

            if (amount != 0)
                MessageBox.Show("Користувач з даним ПІБ існує");
            else
            {
                strQ = $"UPDATE dbo.Clients SET dbo.Clients.Name = '{Name.Text}', dbo.Clients.Surname ='{Surname.Text}', dbo.Clients.SecondName = '{SecondName.Text}', dbo.Clients.Town = '{Town.Text}', dbo.Clients.Adress = '{Adress.Text}', dbo.Clients.Phone = '{Phone.Text}' WHERE dbo.Clients.IDClient = {Temporary.serial};";
                command = new SqlCommand(strQ, connection);
                command.ExecuteNonQuery();
                MessageBox.Show("Ви змінили дані профіля");
            }
            connection.Close();
        }
    }
}