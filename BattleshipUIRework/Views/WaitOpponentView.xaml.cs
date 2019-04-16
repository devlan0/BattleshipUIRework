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
            string status = "";
            string message = "Connection to Server failed";
            int isReady = 0;
            int[] enemyMap;
            while (status.Equals("success") && isReady == 0)
            {
                (status, message, isReady, enemyMap) = await HttpBattleshipClient.OpponentReady(MainWindow.player.name, MainWindow.token);
                Thread.Sleep(1000);
            }
            if (isReady == 1)
            {
                Window.GetWindow(this).DataContext = new GameView();
            }
            ErrorLabel.Content = message;
        }
    }
}
