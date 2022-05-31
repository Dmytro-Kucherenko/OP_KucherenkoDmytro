using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _3
{
    public partial class User : Window 
    {
        string connectionStr = null;
        SqlConnection connection = null;
        SqlCommand command = null;
        public User()
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
        private void AddNewClick(object sender, RoutedEventArgs e)
        {
            
            if (Equals(NewLogin.Text, "") || Equals(NewPass.Text, ""))
            {
                MessageBox.Show("Заповніть усі обов'язкові поля");
                return;
            }
            if (!IsPassword(NewPass.Text))
            {
                MessageBox.Show("Пароль не підходить під обмеження");
                return;
            }
            try
            {
                connection = new SqlConnection(connectionStr);
                connection.Open();

                string strQ = $"INSERT INTO MainTable values('{NewLogin.Text}', '{NewPass.Text}', '{NewName.Text}', '{NewSurname.Text}', 1);";
                command = new SqlCommand(strQ, connection);
                command.ExecuteNonQuery();
                NewPass.Text = NewName.Text = NewSurname.Text = NewLogin.Text = "";
                MessageBox.Show("Новий користувач зареєстрований");
            }
            catch
            {
                MessageBox.Show("Користувач з даним логіном вже інснує");
            }

            connection.Close();
        }
        static string login;
        static int counter = 0;
        private void AutoClick(object sender, RoutedEventArgs e)
        {
            if (Equals(UserLog.Text, ""))
            {
                MessageBox.Show("Введіть логін користувача");
                return;
            }

            connection = new SqlConnection(connectionStr);
            connection.Open();

            string strQ = $"SELECT COUNT(*) FROM MainTable WHERE (dbo.MainTable.Login = '{UserLog.Text}') AND (dbo.MainTable.Password = '{UserPass.Text}') AND (Status = 1);";
            int amount = (int)new SqlCommand(strQ, connection).ExecuteScalar();
            if (amount != 0)
            {
                login = UserLog.Text;
                ChangeName.IsEnabled = ChangeSurname.IsEnabled = ChangePass.IsEnabled = ChangeButt.IsEnabled = true;
                UserLog.Text = UserPass.Text = "";
                counter = 0;
                MessageBox.Show("Ви авторизувались");
            }
            else if (counter < 3)
            {
                MessageBox.Show("Ви ввели некоректні дані");
                counter++;
            }
            else
            {
                MessageBox.Show("Ви ввели некоректні дані тричі");
                System.Windows.Application.Current.Shutdown();
            }

            connection.Close();
        }
        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            
            if (Equals(ChangePass.Text, ""))
            {
                MessageBox.Show("Введіть новий пароль користувача");
                return;
            }
            if (!IsPassword(ChangePass.Text))
            {
                MessageBox.Show("Пароль не підходить під обмеження");
                return;
            }
            connection = new SqlConnection(connectionStr);
            connection.Open();

            string strQ = $"UPDATE MainTable SET Password = '{ChangePass.Text}', Name = '{ChangeName.Text}', Surname = '{ChangeSurname.Text}' WHERE (dbo.MainTable.Login = '{login}') AND (Status = 1);";
            command = new SqlCommand(strQ, connection);
            command.ExecuteNonQuery();
            ChangePass.Text = ChangeName.Text = ChangeSurname.Text = "";
            MessageBox.Show("Ви оновили дані");

            connection.Close();
        }
        public static bool IsPassword(string pass) => pass.Any(x => char.IsNumber(x)) && pass.Any(x => char.IsUpper(x)) && pass.Any(x => char.IsLower(x));
        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            ChangeName.IsEnabled = ChangeSurname.IsEnabled = ChangePass.IsEnabled = ChangeButt.IsEnabled = false;
            login = null;
        }
    }
}
