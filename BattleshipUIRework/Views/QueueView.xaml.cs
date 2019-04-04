using System.Windows;
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

        private void StartQueueBtn_Clicked(object sender, RoutedEventArgs e)
        {
            QueueButton.IsEnabled = false;
            _stopBtn_clicked = false;
            // Change btn appearance
            QueueButton.Click -= StartQueueBtn_Clicked;
            QueueButton.Click += StopQueueBtn_Clicked;
            QueueButton.Content = "Stop Queue";
            ProgressRing.IsActive = true;
            QueueButton.IsEnabled = true;
            new Thread(async () =>
            {
                Thread.CurrentThread.IsBackground = true;
                try
                {
                    string status = "";
                    string message = "Error connecting to server.";

                   if (App.DEBUG_MODE)
                    {
                        status = "success";
                    }
                   else
                    {
                        (status, message) = await HttpBattleshipClient.Enqueue(MainWindow.player.name, MainWindow.token);
                    }

                    if (status.Equals("success"))
                    {
                        //Check if match found
                        status = "";

                        while (!status.Equals("success") && !_stopBtn_clicked)
                        {
                            if (App.DEBUG_MODE)
                            {
                                status = "success";
                                MainWindow.player.field = Enumerable.Range(0, 225).Select(n => 0).ToArray();
                                MainWindow.player.originalField = Enumerable.Range(0, 225).Select(n => 0).ToArray();
                                MainWindow.opponent.name = "Otto";
                                MainWindow.player.name = "Adolf";
                                MainWindow.opponent.field = Enumerable.Range(0, 225).Select(n => 0).ToArray();
                            }
                            else
                            {
                                (status, message, MainWindow.matchid, MainWindow.player.field, MainWindow.opponent.name) = await HttpBattleshipClient.Queue(MainWindow.player.name, MainWindow.token);
                                Array.Copy(MainWindow.player.field, 0, MainWindow.player.originalField, 0, MainWindow.player.field.Length);
                            }

                            if (status.Equals("success"))
                            {
                                Console.WriteLine("Your opponent: " + MainWindow.opponent.name);
                                Dispatcher.Invoke(() =>
                                {
                                    Window.GetWindow(this).DataContext = new BuildView();
                                });
                            }
                            else
                            {
                                Console.WriteLine("Searching...");
                                Thread.Sleep(1000);
                            }
                        }
                    }
                    else
                    {
                        ErrorLabel.Content = message;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException().Message);
                }
            }).Start();

        }

        private async void StopQueueBtn_Clicked(object sender, RoutedEventArgs e)
        {
            QueueButton.IsEnabled = false;
            _stopBtn_clicked = true;
            QueueButton.Click -= StopQueueBtn_Clicked;
            QueueButton.Click += StartQueueBtn_Clicked;
            QueueButton.Content = "Start Queue";
            ProgressRing.IsActive = false;
            await HttpBattleshipClient.Dequeue(MainWindow.player.name, MainWindow.token);
            QueueButton.IsEnabled = true;
        }
        
        public void LogoutBtn_Clicked(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            Window.GetWindow(this).Close();
            login.Show();
        }
    }
}
