using BattleshipUIRework.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private LoginViewModel viewModel;

        public LoginView()
        {
            viewModel = new LoginViewModel();
            InitializeComponent();
        }

        private void LoginBtn_Clicked(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            Window.GetWindow(this).Close();
            main.Show();
        }

        private void RegisterBtn_Clicked(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).DataContext = new RegisterViewModel();
        }

    }
}
