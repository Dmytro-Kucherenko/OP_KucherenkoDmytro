using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _3
{
    public partial class Admin : Window
    {
        string connectionStr = null;
        SqlConnection connection = null;
        SqlCommand command = null;
        SqlDataAdapter adapter = null;
        DataTable dt = null;
        int length = 0, index = 0;
        public Admin()
        {
            InitializeComponent();
            connectionStr = "Data Source = DMYTRO; Initial Catalog = Prac3; Integrated Security = True";
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }
        private void datagridUpdate()
        {
            connection = new SqlConnection(connectionStr);
            connection.Open();

            adapter = new SqlDataAdapter("SELECT*FROM MainTable", connection);
            dt = new DataTable("Користувачі системи");
            adapter.Fill(dt);
            output.ItemsSource = dt.DefaultView;
            length = dt.Rows.Count;

            connection.Close();
        }
        static string password;
        private void AutoBut_Click(object sender, RoutedEventArgs e)
        {
            
            if (Equals(AutoEnt.Text, ""))
            {
                MessageBox.Show("Введіть логін користувача");
                return;
            }
            connection = new SqlConnection(connectionStr);
            connection.Open();

            string strQ = $"SELECT COUNT(*) FROM MainTable WHERE (dbo.MainTable.Login = 'ADMIN') AND (dbo.MainTable.Password = '{AutoEnt.Text}');";
            int amount = (int)new SqlCommand(strQ, connection).ExecuteScalar();
            password = AutoEnt.Text;
            if (amount != 0)
            {
                ChangePass.IsEnabled = ChangeNewPass.IsEnabled = ChangeName.IsEnabled = ChangeSurname.IsEnabled = ChangeUpdate.IsEnabled = Previous.IsEnabled = Next.IsEnabled = StatChange.IsEnabled = AddLogin.IsEnabled = AddUser.IsEnabled = true;
                AutoEnt.Text = "";
                datagridUpdate();
                check();
                MessageBox.Show("Ви авторизувались");
            }
            else
                MessageBox.Show("Ви ввели невірний пароль");
            
            connection.Close();
        }
        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            if (Equals(AddLogin.Text, ""))
            {
                MessageBox.Show("Введіть логін");
                return;
            }
            try
            {
                connection = new SqlConnection(connectionStr);
                connection.Open();

                string strQ = $"INSERT INTO MainTable values('{AddLogin.Text}', '', '', '', 1);";
                command = new SqlCommand(strQ, connection);
                command.ExecuteNonQuery();
                AddLogin.Text = "";
                datagridUpdate();
                MessageBox.Show("Новий користувач зареєстрований");
            }
            catch
            {
                MessageBox.Show("Користувач з даним логіном вже інснує");
            }
           
            connection.Close();
        }
        private void ChangeUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (Equals(ChangePass.Text, "") || Equals(ChangeNewPass.Text, ""))
            {
                MessageBox.Show("Введіть новий пароль користувача");
                return;
            }
            if (!Equals(ChangePass.Text, password))
            {
                MessageBox.Show("Старий пароль не є вірним");
                return;
            }
            if (!User.IsPassword(ChangeNewPass.Text))
            {
                MessageBox.Show("Новий пароль не підходить під обмеження");
                return;
            }
            connection = new SqlConnection(connectionStr);
            connection.Open();

            string strQ = $"UPDATE MainTable SET Password = '{ChangeNewPass.Text}', Name = '{ChangeName.Text}', Surname = '{ChangeSurname.Text}' WHERE (dbo.MainTable.Login = 'ADMIN')";
            command = new SqlCommand(strQ, connection);
            command.ExecuteNonQuery();
            ChangePass.Text = ChangeNewPass.Text = ChangeName.Text = ChangeSurname.Text = "";
            datagridUpdate();
            MessageBox.Show("Ви оновили дані");

            connection.Close();
        }
        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (index > 0)
            {
                index--;
                check();
            }
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (index < length - 1)
            {
                index++;
                check();
            }
        }
        private void StatChange_Click(object sender, RoutedEventArgs e)
        {
            connection = new SqlConnection(connectionStr);
            connection.Open();

            int i;
            if (Equals(StatCheck.Content, "Активований"))
            {
                StatCheck.Content = "Деактивований";
                i = 0;
            }
            else
            {
                StatCheck.Content = "Активований";
                i = 1;
            }
            string strQ = $"UPDATE MainTable SET Status = '{i}' WHERE (dbo.MainTable.Login = '{LogCheck.Content}')";
            command = new SqlCommand(strQ, connection);
            command.ExecuteNonQuery();
            datagridUpdate();
            MessageBox.Show("Ви оновили дані");

            connection.Close();
        }
        private void check()
        {
            int k = index;
            PCheck.Content = dt.Rows[index][3].ToString();
            ICheck.Content = dt.Rows[index][2].ToString();
            LogCheck.Content = dt.Rows[index][0].ToString();
            if ((bool)dt.Rows[index][4])
                StatCheck.Content = "Активований";
            else
                StatCheck.Content = "Деактивований";
            if (Equals(LogCheck.Content, "Admin") )
                StatChange.IsEnabled = false;
            else
                StatChange.IsEnabled = true;
        }
    }
}
