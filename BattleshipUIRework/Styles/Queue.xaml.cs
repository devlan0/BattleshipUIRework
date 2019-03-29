using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BattleshipUI
{
    /// <summary>
    /// Interaction logic for Queue.xaml
    /// </summary>
    public partial class Queue : Window
    {
        public Queue()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Makes Queue Visible. Adds Function to Button.
        /// </summary>
        /*private void ahhh()
        {
            gameGrid.Visibility = Visibility.Collapsed;
            if (loginGrid.ColumnDefinitions.Count == 3) loginGrid.ColumnDefinitions.RemoveAt(2);
            emailPanel.Visibility = Visibility.Collapsed;
            loginGrid.Visibility = Visibility.Collapsed;
            queueGrid.Visibility = Visibility.Visible;
            queueButton.Click += StartQueue;
        }

        private async void StartQueue(object sender, RoutedEventArgs e)
        {
            int[] map = Enumerable.Range(0, 15 * 15).Select(n => 0).ToArray();                                          //Dirty
            player1 = new Player(_colorPlayer1, _colorPlayer1Dark, _colorPlayer1Light, 0, buildShipsGrid, _size);
            player1.field = ConvertMap(map);
            BuildShips();                                                                                               //Dirty


            (string status, string message) = await HttpBattleshipClient.QueueMatch(_name, _token);
            if (status.Equals("success"))
            {
                
                (string gameID, int[] map, string opponent) = await HttpBattleshipClient.MatchFound(_name, _token);
                _playerField = ConvertMap(map);
                _gameID = gameID;
                _opponent = opponent;
                BuildShips();
            }
            else
            {
                string msg = message;
                textbox.print
            }
        }*/
    }
}
