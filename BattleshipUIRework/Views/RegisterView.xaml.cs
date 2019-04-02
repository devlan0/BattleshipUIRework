using BattleshipUIRework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BattleshipUIRework.Views
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : UserControl
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        private async void RegisterBtn_Clicked(object sender, RoutedEventArgs e)
        {
            string status = "";
            string message = "Error connecting to server";
            string token = "";

            RegisterBtn.IsEnabled = false;

            //Check if entered credentials are valid
            if (!EmailValid(EmailTxtBox.Text))
            {
                message = "Email invalid!";
            }
            else if (!PasswordValid(PwdTxtBox.Password))
            {
                message = "Password invalid!";
            }
            else if (!PwdTxtBox.Password.Equals(PwdRepeatTxtBox.Password))
            {
                message = "Passwords do not match!";
            }
            else
            {
                //Submit credentials to server if local validation is successful
                using (SHA256 hashAlg = SHA256.Create())
                {
                    byte[] hashedPw = hashAlg.ComputeHash(Encoding.UTF8.GetBytes(PwdTxtBox.Password));
                    (status, message, token) = await HttpBattleshipClient.Register(UsrTxtBox.Text, EmailTxtBox.Text, hashedPw);
                }
            }
            if (status.Equals("success"))
            {
                //Switch to main window if remote validation successful
                Console.WriteLine("Registration successful");
                MainWindow main = new MainWindow(UsrTxtBox.Text, token);
                Window.GetWindow(this).Close();
                main.Show();
            }
            else
            {
                ErrorLabel.Content = message;
                RegisterBtn.IsEnabled = true;
            }
        }

        private void AccExistsBtn_Clicked(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).DataContext = new LoginView();
        }

        private bool EmailValid(string email)
        {
            Regex rx = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                 + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
            if (!rx.IsMatch(email))
            {
                return false;
            }
            return true;
        }

        private bool PasswordValid(string password)
        {
            Regex rx = new Regex("^.{8,88}$");
            if (!rx.IsMatch(password))
            {
                return false;
            }
            return true;
        }

        private void UsrTextChanged(object sender, TextChangedEventArgs e)
        {
            RegisterBtn.IsEnabled = TextFieldsNotEmpty();
        }

        private void EmailTextChanged(object sender, TextChangedEventArgs e)
        {
            RegisterBtn.IsEnabled = TextFieldsNotEmpty();
        }

        private void PasswordTextChanged(object sender, RoutedEventArgs e)
        {
            RegisterBtn.IsEnabled = TextFieldsNotEmpty();
        }

        private void RepPasswordChanged(object sender, RoutedEventArgs e)
        {
            RegisterBtn.IsEnabled = TextFieldsNotEmpty();
        }

        private bool TextFieldsNotEmpty()
        {
            return !UsrTxtBox.Text.Equals("") && !EmailTxtBox.Text.Equals("") && !PwdTxtBox.Password.Equals("") && !PwdRepeatTxtBox.Password.Equals("")
                ? true
                : false;
        }
    }
}