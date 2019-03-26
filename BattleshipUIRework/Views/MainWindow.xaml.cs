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
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace BattleshipUIRework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static string _token = "";
        public static string _username = "";
        public static int[] _playerMap;
        public static string _matchid = "";
        public static string _opponent = "";

        public MainWindow(string username, string token)
        {
            _username = username;
            _token = token;
            InitializeComponent();
        }
        private void CloseBtn_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
