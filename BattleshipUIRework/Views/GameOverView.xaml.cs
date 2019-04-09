using BattleshipUIRework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for GameOverView.xaml
    /// </summary>
    public partial class GameOverView : UserControl
    {
        private bool _stopBtn_clicked = false;

        public GameOverView()
        {
            InitializeComponent();
        }

        private void NewMatchBtn_Clicked(object sender, RoutedEventArgs e)
        {
            NewMatchBtn.IsEnabled = false;
            _stopBtn_clicked = false;
            // Change btn appearance
            NewMatchBtn.Click -= NewMatchBtn_Clicked;
            NewMatchBtn.Click += StopQueueBtn_Clicked;
            NewMatchBtn.Content = "Stop Queue";
            ProgressRing.IsActive = true;
            NewMatchBtn.IsEnabled = true;
            MainWindow.enqueued = true;

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

                                //
                                //
                                // MAY CREATE ERROR, need server for further testing
                                //
                                //

                                Array.Copy(MainWindow.player.field, 0, MainWindow.player.originalField, 0, MainWindow.player.field.Length);
                            }

                            if (status.Equals("success"))
                            {
                                Console.WriteLine("Your opponent: " + MainWindow.opponent.name);
                                Dispatcher.Invoke(() =>
                                {
                                    MainWindow.enqueued = false;
                                    MainWindow.ingame = true;
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

        private async void QuitBtn_Clicked(object sender, RoutedEventArgs e)
        {
            if (MainWindow.enqueued)
            {
                MainWindow.enqueued = false;
                await HttpBattleshipClient.Dequeue(MainWindow.player.name, MainWindow.token);
            }
            LoginWindow login = new LoginWindow();
            Window.GetWindow(this).Close();
            login.Show();
        }

        private async void StopQueueBtn_Clicked(object sender, RoutedEventArgs e)
        {
            NewMatchBtn.IsEnabled = false;
            _stopBtn_clicked = true;
            NewMatchBtn.Click -= StopQueueBtn_Clicked;
            NewMatchBtn.Click += NewMatchBtn_Clicked;
            NewMatchBtn.Content = "Find new match";
            ProgressRing.IsActive = false;
            await HttpBattleshipClient.Dequeue(MainWindow.player.name, MainWindow.token);
            MainWindow.enqueued = false;
            NewMatchBtn.IsEnabled = true;
        }
    }
}
