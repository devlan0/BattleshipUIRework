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
        private static bool _status = false;
        public QueueView()
        {
            InitializeComponent();
        }

        private void StartQueueBtn_Clicked(object sender, RoutedEventArgs e)
        {
            QueueLoop();
        }
        private void StopQueueBtn_Clicked(object sender, RoutedEventArgs e)
        {
            _status = true;
        }


        private async void QueueLoop()
        {
            string status = "";
            string message = "Error connecting to Server.";
            
            //Update UI

            while(!status.Equals("success") && !_status)
            {

                (status, message) = await HttpBattleshipClient.QueueMatch(MainWindow._username, MainWindow._token);
                //Testing Purposes
                status = "success";
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
        }
    }
}
