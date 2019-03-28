using System.Windows;
using System;
using System.Threading;
using System.Windows.Controls;
using BattleshipUIRework.Models;

namespace BattleshipUIRework.Views
{
    /// <summary>
    /// Starts Queue and loops until game found or stopped
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

            // Change UI
            QueueButton.Click -= StartQueueBtn_Clicked;
            QueueButton.Click += StopQueueBtn_Clicked;
            QueueButtonText.Text = "Stop Queue";
            ProgressRing.IsActive = true;

             //Place user in queue
             (status, message) = await HttpBattleshipClient.Enqueue(MainWindow._username, MainWindow._token);

            if (status.Equals("success"))
            {
                //Check if match found
                status = "";

                while(!status.Equals("success") && !_stopBtn_clicked)
                {
                    (status, message, MainWindow._matchid, MainWindow._playerMap, MainWindow._opponent) = await HttpBattleshipClient.Queue(MainWindow._username, MainWindow._token);
                    if (status.Equals("success"))
                    {
                        Window.GetWindow(this).DataContext = new BuildView();
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            else
            {
                ErrorLabel.Content = message;
            }
        }
        private void StopQueueBtn_Clicked(object sender, RoutedEventArgs e)
        {
            // Dequeue
            _stopBtn_clicked = true;
            QueueButton.Click -= StopQueueBtn_Clicked;
            QueueButton.Click += StartQueueBtn_Clicked;
            QueueButtonText.Text = "Start Queue";
            ProgressRing.IsActive = false;
        }
    }
}
