﻿using System.Windows;
using System;
using System.Linq;
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
        private bool _stopBtn_clicked = false;

        public QueueView()
        {
            InitializeComponent();
        }

        private async void StartQueueBtn_Clicked(object sender, RoutedEventArgs e)
        {
            QueueButton.IsEnabled = false;

            string status = "";
            string message = "Error connecting to server.";

            // Change UI
            QueueButton.Click -= StartQueueBtn_Clicked;
            QueueButton.Click += StopQueueBtn_Clicked;
            QueueButtonText.Text = "Stop Queue";
            ProgressRing.IsActive = true;

            //TODO: ADJUST COMMENTS IN THIS SECTION ACCORDINGLY
            //Place user in queue
            (status, message) = await HttpBattleshipClient.Enqueue(MainWindow._player._username, MainWindow._token);
            //status = "success";

            if (status.Equals("success"))
            {
                //Check if match found
                status = "";

                while (!status.Equals("success") && !_stopBtn_clicked)
                {
                    //TODO: ADJUST COMMENTS IN THIS SECTION ACCORDINGLY
                    (status, message, MainWindow._matchid, MainWindow._player._fields, MainWindow._opponent._username) = await HttpBattleshipClient.Queue(MainWindow._player._username, MainWindow._token);
                    //status = "success";
                    MainWindow._player._fields = Enumerable.Range(0, 225).Select(n => 0).ToArray();
                    if (status.Equals("success"))
                    {
                        Console.WriteLine("Your opponent: " + MainWindow._opponent._username);
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

            QueueButton.IsEnabled = true;
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
        
        public void LogoutBtn_Clicked(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            Window.GetWindow(this).Close();
            login.Show();
        }
    }
}
