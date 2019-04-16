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
        public static readonly int globalMargin = 10;
        public static readonly int size = 15;

        //Colors
        public static SolidColorBrush water = new SolidColorBrush(Color.FromArgb(255, 174, 197, 214));
        public static SolidColorBrush land = new SolidColorBrush(Color.FromArgb(255, 233, 240, 116));
        public static SolidColorBrush ship = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
        public static SolidColorBrush hit = new SolidColorBrush(Color.FromArgb(255, 212, 4, 36));
        public static SolidColorBrush miss = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        public static Dictionary<int, SolidColorBrush> colorDic = new Dictionary<int, SolidColorBrush>();

        //matchstuff
        public static string token = "";
        public static string matchid = "";
        public static bool enqueued = false;
        public static bool ingame = false;

        //players
        public static Player player;
        public static Player opponent;

        public MainWindow(string username, string token)
        {
            colorDic = new Dictionary<int, SolidColorBrush>();
            colorDic.Add(0, water);
            colorDic.Add(1, land);
            colorDic.Add(2, ship);
            colorDic.Add(3, hit);
            colorDic.Add(4, miss);
            //colorDic.Add(4, miss);
            player = new Player
            {
                name = username
            };
            opponent = new Player();
            MainWindow.token = token;
            InitializeComponent();
        }
        private void CloseBtn_Clicked(object sender, RoutedEventArgs e)
        {
            if (enqueued) _ = HttpBattleshipClient.Dequeue(player.name, token);
            //if (ingame) _ = HttpBattleshipClient.LeaveGame;
            Close();
        }

        private async void DequeueUser(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string status = "";
            string message = "";
            Console.WriteLine("Sending dequeue request..."); 
            (status, message) = await HttpBattleshipClient.Dequeue(player.name, token);
            Console.WriteLine("Status: {0}, Message: {1}", status, message);

        }
    }
}
