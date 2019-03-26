using System.Windows;
using System;
using System.Threading;
using System.Windows.Controls;
using BattleshipUIRework.Models;

namespace BattleshipUIRework.Views
{
    /// <summary>
    /// Starts Queue and loops until game found or stopped (still needs to be implemented)
    /// </summary>
    public partial class QueueView : UserControl
    {
        //Umbenennen
        private static bool _stopBtn_clicked = false;
        public QueueView()
        {
            InitializeComponent();
        }

        private async void StartQueueBtn_Clicked(object sender, RoutedEventArgs e)
        {
            string status = "";
            string message = "Error connecting to server.";

            //Place user in queue
            Console.WriteLine("User: {0} Token: {1}", MainWindow._username, MainWindow._token);
            (status, message) = await HttpBattleshipClient.Enqueue(MainWindow._username, MainWindow._token);
            Console.WriteLine("Checkpoint");

            if (status.Equals("success"))
            {
                //Check if match found
                status = "";
                string matchId = "";
                int[] map = null;
                string opponent = "";

                while(!status.Equals("success") && !_stopBtn_clicked)
                {
                    (status, message, matchId, map, opponent) = await HttpBattleshipClient.Queue(MainWindow._username, MainWindow._token);
                    if (status.Equals("success"))
                    {
                        Console.WriteLine("Status: {0} Message: {1}", status, message);
                        Console.WriteLine("Your opponent: {0}", opponent);
                        Window.GetWindow(this).DataContext = new GameView();
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            else
            {
                Console.WriteLine("Status: {0}, Message: {1}", status, message);
            }
        }
        private void StopQueueBtn_Clicked(object sender, RoutedEventArgs e)
        {
            _stopBtn_clicked = true;
        }

        /*
        private async void QueueLoop()
        {
            string status = "";
            string message = "Error connecting to Server.";
            
            //Update UI

            while(!status.Equals("success") && !_status)
            {

                (status, message) = await HttpBattleshipClient.QueueMatch(MainWindow._username, MainWindow._token);
                //Testing Purposes
                //status = "success";
                if (status.Equals("success"))
                {
                    MatchFound();
                }
                else
                {
                    ErrorLabel.Content = message;
                }
                Thread.Sleep(2000);
            }
        }
        private async void MatchFound()
        {
            string status = "";
            string message = "Error connecting to Server.";

            (status, message, MainWindow._matchid, MainWindow._playerMap, MainWindow._opponent) = await HttpBattleshipClient.MatchFound(MainWindow._username, MainWindow._token);
            if (status.Equals("success"))
            {
                Window.GetWindow(this).DataContext = new GameView();
            }
            else
            {
                ErrorLabel.Content = message;
            }
        }*/
    }
}
