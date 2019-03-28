using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro.Controls;
using BattleshipUIRework.Models;

namespace BattleshipUIRework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        //Variables needed in multiple views.
        public static string _token = "";
        public static string _matchid = "";
        public static string _username = "";
        public static string _opponent = "";
        public static Player player;
        public static Player opponent;

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
