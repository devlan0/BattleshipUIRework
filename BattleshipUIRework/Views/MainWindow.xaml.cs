using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro.Controls;
using BattleshipUIRework.Models;
using BattleshipUIRework.Views;

namespace BattleshipUIRework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        //global ui constants
        public static readonly int _globalMargin = 10;
        public static readonly int _size = 15;

        //Colors
        public static SolidColorBrush _water = new SolidColorBrush(Color.FromArgb(255, 174, 197, 214));
        public static SolidColorBrush _land = new SolidColorBrush(Color.FromArgb(255, 233, 240, 116));
        public static SolidColorBrush _ship = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
        public static SolidColorBrush _hit = new SolidColorBrush(Color.FromArgb(255, 212, 4, 36));

        //matchstuff
        public static string _token = "";
        public static string _matchid = "";

        //players
        public static Player _player;
        public static Player _opponent;

        public MainWindow(string username, string token)
        {
            _player = new Player
            {
                uiColumn = 0,
                username = username
            };
            _opponent = new Player
            {
                uiColumn = 1
            };
            _token = token;
            InitializeComponent();
        }
        private void CloseBtn_Clicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void DequeueUser(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string status = "";
            string message = "";
            Console.WriteLine("Sending dequeue request..."); 
            (status, message) = await HttpBattleshipClient.Dequeue(_player.username, _token);
            Console.WriteLine("Status: {0}, Message: {1}", status, message);

        }
    }
}
