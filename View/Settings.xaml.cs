using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace PolMedUMG.View
{
    /// <summary>
    /// Logika interakcji dla klasy Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void ChangeEmail_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewEmailBox.Text) ||
               string.IsNullOrWhiteSpace(ConfirmEmailBox.Text))
            {
                MessageBox.Show("Proszę wprowadzić nowy adres e-mail i potwierdzić go.");
                return;
            }
            else if (NewEmailBox.Text != ConfirmEmailBox.Text)
            {
                MessageBox.Show("Adresy e-mail różnią się od siebie. Proszę spróbować ponownie.");
                return;
            }
            else if (!IsValidEmail(NewEmailBox.Text))
            {
                MessageBox.Show("Niepoprawny e-mail.");
                return;
            }
            else
            {
                try
                {
                    using (var conn = new MySqlConnection(SessionManager.connStrSQL))
                    {
                        conn.Open();
                        string sql = "UPDATE users SET mail=@mail WHERE uid=@uid";
                        using (var cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@mail", NewEmailBox.Text);
                            cmd.Parameters.AddWithValue("@uid", SessionManager.CurrentUsername);

                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                        MessageBox.Show("Adres e-mail został zmieniony pomyślnie!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

                private void ChangePassword_Click(object sender, RoutedEventArgs e)
                {
                    if (string.IsNullOrWhiteSpace(NewPasswordBox.Password) ||
                       string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password))
                    {
                        MessageBox.Show("Proszę wprowadzić nowe hasło i  je potwierdzić.");
                        return;
                    }
                    else if (NewPasswordBox.Password != ConfirmPasswordBox.Password)
                    {
                        MessageBox.Show("Wprowadzone hasła różnią się od siebie. Proszę spróbować ponownie.");
                        return;
                    }
            else if (!IsValidPassword(NewPasswordBox.Password))
            {
                MessageBox.Show("Hasło musi mieć od 1 do 15 znaków.");
                return;
            }
            else
            {
                try
                {
                    using (var conn = new MySqlConnection(SessionManager.connStrSQL))
                    {
                        conn.Open();
                        string sql = "UPDATE users SET pwd=@pwd WHERE uid=@uid";
                        using (var cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@pwd", NewPasswordBox.Password);
                            cmd.Parameters.AddWithValue("@uid", SessionManager.CurrentUsername);

                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                        MessageBox.Show("Hasło zostało zmienione pomyślnie!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (Regex.IsMatch(email, pattern) == false) return false;
            using (MySqlConnection conn = new MySqlConnection(SessionManager.connStrSQL))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM users WHERE mail = @mail";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@mail", email);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count == 0;
                }
            }
        }
        private bool IsValidPassword(string password)
        {
            return password.Length > 0 && password.Length <= 15;
        }
    }
}
