﻿using BattleshipUIRework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            if (App.DEBUG_MODE)
            {
                LoginBtn.IsEnabled = true;
            }
        }

        private async void LoginBtn_Clicked(object sender, RoutedEventArgs e)
        {
            LoginBtn.IsEnabled = false;

            string status = "";
            string message = "Error connecting to server";
            string token = "";

            if (App.DEBUG_MODE)
            {
                status = "success";
            }
            else
            {
                using (SHA256 hashAlg = SHA256.Create())
                {
                    (status, message, token) = await HttpBattleshipClient.Login(UsrTextBox.Text, hashAlg.ComputeHash(Encoding.UTF8.GetBytes(PwdTextBox.Password)));
                }
            }

            if (status.Equals("success"))
            {
                MainWindow main = new MainWindow(UsrTextBox.Text, token);
                Window.GetWindow(this).Close();
                main.Show();
            }
            else
            {
                LoginBtn.IsEnabled = true;
                ErrorLabel.Content = message;
            }
        }

        private void RegisterBtn_Clicked(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).DataContext = new RegisterView();
        }

        private void UsrTextField_Changed(object sender, TextChangedEventArgs e)
        {
            LoginBtn.IsEnabled = TextFieldsNotEmpty();
        }

        private void PwField_Changed(object sender, RoutedEventArgs e)
        {
            LoginBtn.IsEnabled = TextFieldsNotEmpty();
        }

        private bool TextFieldsNotEmpty()
        {
            return !UsrTextBox.Text.Equals("") && !PwdTextBox.Password.Equals("") ? true : false;
        }
    }
}
