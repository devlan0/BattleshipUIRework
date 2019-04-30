using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BattleshipUIRework.Models;

namespace BattleshipUIRework.Views
{
    /// <summary>
    /// Interaction logic for WaitOpponentView.xaml
    /// </summary>
    public partial class WaitOpponentView : UserControl
    {
        public WaitOpponentView()
        {
            InitializeComponent();
            waitingLoop();
        }
        private async void waitingLoop()
        {
            string status = "success";
            string message = "Connection to Server failed";
            int isReady = 0;
            int[] enemyMap = new int[(int) Math.Pow(MainWindow.size,2)];
            //
            //
            // TODO: Debug Mode missing
            //
            //

            while (status.Equals("success") && isReady == 0)
            {
                (status, message, isReady, enemyMap) = await HttpBattleshipClient.OpponentReady(MainWindow.player.name, MainWindow.token);
                Thread.Sleep(1000);
            }
            if (isReady == 1)
            {
                MainWindow.opponent.field = enemyMap;
                Window.GetWindow(this).DataContext = new GameView();
            }
            ProgressRing.IsActive = false;
            ErrorLabel.Content = message;

            // TODO: Some Code if Server fails
            // Throw back to QueueView maybe?
        }
        private async void LogoutBtn_Clicked(object sender, RoutedEventArgs e)
        {
            await HttpBattleshipClient.Dequeue(MainWindow.player.name, MainWindow.token);
            Window.GetWindow(this).DataContext = new QueueView();
        }
    }
}
