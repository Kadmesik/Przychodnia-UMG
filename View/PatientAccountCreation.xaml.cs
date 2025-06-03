using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FontAwesome.WPF;
using MySql.Data.MySqlClient;
using PolMedUMG.ViewModel;

namespace PolMedUMG.View
{
    public partial class PatientAccountCreation : UserControl
    {
        public PatientAccountCreation()
        {
            InitializeComponent();
        }

        private void go_Back(object sender, RoutedEventArgs e)
        {
            var Conv = new LoginPrompt();
            var parentWindow = Window.GetWindow(this) as LoginScreen;
            parentWindow?.LoadContent(Conv);
        }

        private void go_Next(object sender, RoutedEventArgs e)
        {
            string uid = Nickname.Text;
            string pwd = Password.Text;
            string acc_type = "0";
            string mail = Email.Text;
            string firstName = Name.Text;
            string secondName = Surname.Text;
            string last_login = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            bool valid = true;

            // Resetuj tła i ukryj błędy
            ResetTextBoxBackground(Nickname);
            ResetTextBoxBackground(Email);
            ResetTextBoxBackground(Name);
            ResetTextBoxBackground(Surname);
            ResetTextBoxBackground(Password);
            ResetIconBackground(NicknameIcon);
            ResetIconBackground(EmailIcon);
            ResetIconBackground(NameIcon);
            ResetIconBackground(SurnameIcon);
            ResetIconBackground(PasswordIcon);
            UsernameError.Visibility = Visibility.Collapsed;
            EmailError.Visibility = Visibility.Collapsed;
            NameError.Visibility = Visibility.Collapsed;
            SurnameError.Visibility = Visibility.Collapsed;
            PasswordError.Visibility = Visibility.Collapsed;

            if (!IsValidUsername(uid))
            {
                MarkTextBoxInvalid(Nickname);
                MarkIconInvalid(NicknameIcon);
                UsernameError.Visibility = Visibility.Visible;
                valid = false;
            }

            if (!IsValidEmail(mail))
            {
                MarkTextBoxInvalid(Email);
                MarkIconInvalid(EmailIcon);
                EmailError.Visibility = Visibility.Visible;
                valid = false;
            }

            if (!IsValidName(firstName))
            {
                MarkTextBoxInvalid(Name);
                MarkIconInvalid(NameIcon);
                NameError.Visibility = Visibility.Visible;
                valid = false;
            }

            if (!IsValidSurname(secondName))
            {
                MarkTextBoxInvalid(Surname);
                MarkIconInvalid(SurnameIcon);
                SurnameError.Visibility = Visibility.Visible;
                valid = false;
            }

            if (!IsValidPassword(pwd))
            {
                MarkTextBoxInvalid(Password);
                MarkIconInvalid(PasswordIcon);
                PasswordError.Visibility = Visibility.Visible;
                valid = false;
            }

            if (!valid) return;

            CreateUser(uid, pwd, acc_type, mail, last_login, firstName, secondName);
        }

        private void MarkTextBoxInvalid(TextBox tb)
        {
            if (tb.Parent is Border border)
                border.Background = new SolidColorBrush(Color.FromRgb(255, 229, 229));
        }

        private void MarkIconInvalid(ImageAwesome icon)
        {
            if (icon.Parent is Border border)
                border.Background = new SolidColorBrush(Color.FromRgb(255, 229, 229));
        }

        private void ResetTextBoxBackground(TextBox tb)
        {
            if (tb.Parent is Border border)
                border.Background = Brushes.White;
        }

        private void ResetIconBackground(ImageAwesome icon)
        {
            if (icon.Parent is Border border)
                border.Background = Brushes.White;
        }

        private void CreateUser(string uid, string pwd, string acc_type, string mail, string last_login, string firstName, string secondName)
        {
            using (MySqlConnection conn = new MySqlConnection(SessionManager.connStrSQL))
            {
                try
                {
                    conn.Open();
                    string sql = @"INSERT INTO users (uid, pwd, acc_type, mail, last_login, firstName, secondName)
                                   VALUES (@uid, @pwd, @acc_type, @mail, @last_login, @firstName, @secondName);";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", uid);
                        cmd.Parameters.AddWithValue("@pwd", pwd);
                        cmd.Parameters.AddWithValue("@acc_type", acc_type);
                        cmd.Parameters.AddWithValue("@mail", mail);
                        cmd.Parameters.AddWithValue("@last_login", last_login);
                        cmd.Parameters.AddWithValue("@firstName", firstName);
                        cmd.Parameters.AddWithValue("@secondName", secondName);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Utworzono konto pacjenta!");
                    var Conv = new LoginPrompt();
                    var parentWindow = Window.GetWindow(this) as LoginScreen;
                    parentWindow?.LoadContent(Conv);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd przy dodawaniu konta: " + ex.Message);
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && (tb.Text == "Imie" || tb.Text == "Nazwisko" || tb.Text == "E-mail" || tb.Text == "Hasło" || tb.Text == "Nazwa użytkownika"))
            {
                tb.Text = string.Empty;
                tb.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && string.IsNullOrWhiteSpace(tb.Text))
            {
                switch (tb.Name)
                {
                    case "Name":
                        tb.Text = "Imie";
                        break;
                    case "Surname":
                        tb.Text = "Nazwisko";
                        break;
                    case "Email":
                        tb.Text = "E-mail";
                        break;
                    case "Password":
                        tb.Text = "Hasło";
                        break;
                    case "Nickname":
                        tb.Text = "Nazwa użytkownika";
                        break;
                }
                tb.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                EmailError.Text = "Nie podano adresu e-mail";
                return false;
            }
            if (email.Length > 40)
            {
                EmailError.Text = "Adres e-mail jest za długi (40)";
                return false;
            }
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, pattern))
            {
                EmailError.Text = "Adres e-mail ma złą składnię";
                return false;
            }

            using (MySqlConnection conn = new MySqlConnection(SessionManager.connStrSQL))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM users WHERE mail = @mail";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@mail", email);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count != 0)
                    {
                        EmailError.Text = "Użytkownik z takim adresem e-mail już istnieje";
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsValidUsername(string uid)
        {
            if (string.IsNullOrWhiteSpace(uid))
            {
                UsernameError.Text = "Nie podano nazwy użytkownika";
                return false;
            }
            if (uid.Length < 3)
            {
                UsernameError.Text = "Nazwa użytkownika jest zbyt krótka (min. 3)";
                return false;
            }
            if (uid.Length > 25)
            {
                UsernameError.Text = "Nazwa użytkownika jest zbyt długa (max 25)";
                return false;
            }
            if (!Regex.IsMatch(uid, @"^[a-zA-Z0-9_-]+$"))
            {
                UsernameError.Text = "Dozwolone znaki: litery, cyfry, _ i -";
                return false;
            }

            using (MySqlConnection conn = new MySqlConnection(SessionManager.connStrSQL))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM users WHERE uid = @uid";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", uid);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count != 0)
                    {
                        UsernameError.Text = "Taka nazwa użytkownika już istnieje";
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name == "Imie")
            {
                NameError.Text = "Nie podano imienia";
                return false;
            }
            if (name.Length < 3)
            {
                NameError.Text = "Zbyt krótkie imię (min. 3)";
                return false;
            }
            if (name.Length > 25)
            {
                NameError.Text = "Zbyt długie imię (max 25)";
                return false;
            }
            if (!Regex.IsMatch(name, @"^[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ]+$"))
            {
                NameError.Text = "Dozwolone są tylko litery i polskie znaki";
                return false;
            }
            return true;
        }

        private bool IsValidSurname(string surname)
        {
            if (string.IsNullOrWhiteSpace(surname) || surname == "Nazwisko")
            {
                SurnameError.Text = "Nie podano nazwiska";
                return false;
            }
            if (surname.Length < 3)
            {
                SurnameError.Text = "Zbyt krótkie nazwisko (min. 3)";
                return false;
            }
            if (surname.Length > 25)
            {
                SurnameError.Text = "Zbyt długie nazwisko (max 25)";
                return false;
            }
            if (!Regex.IsMatch(surname, @"^[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ]+(-[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ]+)?$"))
            {
                SurnameError.Text = "Niepoprawny format nazwiska";
                return false;
            }
            return true;
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                PasswordError.Text = "Nie podano hasła";
                return false;
            }
            if (password.Length < 8)
            {
                PasswordError.Text = "Zbyt krótkie hasło (min. 8)";
                return false;
            }
            if (password.Length > 25)
            {
                PasswordError.Text = "Zbyt długie hasło (max 25)";
                return false;
            }
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$";
            if (!Regex.IsMatch(password, pattern))
            {
                PasswordError.Text = "Hasło musi zawierać: małą i wielką literę, cyfrę oraz znak specjalny";
                return false;
            }
            return true;
        }
    }
}