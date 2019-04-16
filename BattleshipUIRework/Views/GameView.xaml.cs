using System;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using BattleshipUIRework.Models;

namespace BattleshipUIRework.Views
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        private bool currentturn;
        private bool gameEnded = false;

        public GameView()
        {
            InitializeComponent();
        }



        // --------------------- NOT DONE ---------------------



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //set names
            playerText.Text = MainWindow.player.name;
            opponenText.Text = MainWindow.opponent.name;

            //player UI
            Tuple<Grid, Button[]> tuple = GenerateUIField(MainWindow.player, false);
            MainWindow.player.buttonField = tuple.Item2;
            gameCol1.Children.Add(tuple.Item1);

            //opponent UI
            tuple = GenerateUIField(MainWindow.opponent, true);
            MainWindow.opponent.buttonField = tuple.Item2;
            gameCol2.Children.Add(tuple.Item1);

            //resize
            int size = GameLogic.CalcSize(gameCol1);
            GameLogic.Resize((Grid)gameCol1.Children[0], size);
            GameLogic.Resize((Grid)gameCol2.Children[0], size);

            Thread game = new Thread(new ThreadStart(GameLoop));
            game.Start();
        }

        /// <summary>
        /// Generates UI Grid with Buttons.
        /// </summary>
        /// <param name="player">Player, whose Map shall be pronted</param>
        /// <param name="fire">Disables friendly fire.</param>
        /// <returns>Tuple: Grid (UI) and Buttons in the UI (for later recoloring)</returns>
        public Tuple<Grid, Button[]> GenerateUIField(Player pl, bool fire)
        {
            //Create Gridfield
            Grid field = new Grid();
            field.HorizontalAlignment = HorizontalAlignment.Center;
            field.VerticalAlignment = VerticalAlignment.Center;

            // Generate necessary Rows/Columns
            for (int i = 0; i < MainWindow.size; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                field.ColumnDefinitions.Add(col);
                RowDefinition row = new RowDefinition();
                field.RowDefinitions.Add(row);
            }

            //Generate Buttons
            Button[] buttons = new Button[(int)Math.Pow(MainWindow.size, 2)];
            for (int i = 0; i < Math.Pow(MainWindow.size, 2); i++)
            {
                Button button = new Button();
                button.HorizontalAlignment = HorizontalAlignment.Stretch;
                button.VerticalAlignment = VerticalAlignment.Stretch;
                button.Background = MainWindow.colorDic[pl.field[i]];

                if (fire) button.Click += Fire;

                field.Children.Add(button);
                Grid.SetColumn(button, i % MainWindow.size);
                Grid.SetRow(button, (int)(i / MainWindow.size));
                buttons[i] = button;
            }
            return Tuple.Create(field, buttons);
        }
        public async void Fire(object sender, RoutedEventArgs e)
        {
            if(currentturn)
            {
                Button send = (Button)sender;
                int index = Array.IndexOf(MainWindow.player.buttonField, send);
                string status = "";
                string message = "Connection to Server failed.";
                string hit_status = "";
                (status, message, hit_status) = await HttpBattleshipClient.ShotFired(index % MainWindow.size, (int)(index / MainWindow.size), MainWindow.player.name, MainWindow.token);
                if (status.Equals("success"))
                {
                    if(hit_status.Equals("hit")) send.Background = MainWindow.hit; //Correct Equals
                    else send.Background = MainWindow.miss;

                    currentturn = false;
                }
                else
                {
                    ErrorLabel.Content = message;
                }
            }
        }
        private async void GameLoop()
        {
            string status = "";
            string currentPlayer = "";
            string message;
            int[] shotsFired;
            while (!gameEnded)
            {
                status = "";
                currentPlayer = "";
                message = "Connection to Server failed.";

                //Alex muss mal arbeiten
                (status, message, currentPlayer, shotsFired) = await HttpBattleshipClient.CurrentTurn(MainWindow.player.name, MainWindow.token);
                //(status, message, shotsFired) = await HttpBattleshipClient.CurrentTurn(MainWindow.player.name, MainWindow.token);
                if(currentPlayer.Equals(MainWindow.player.name))
                {
                    currentturn = true;
                }
                Thread.Sleep(1000);
            }
        }
        private async void LeaveBtn_Clicked(object sender, RoutedEventArgs e)
        {
            //Is Dequeue == leave?
            await HttpBattleshipClient.Dequeue(MainWindow.player.name, MainWindow.token);
            Window.GetWindow(this).DataContext = new QueueView();
        }
    }
}

// 1d to 2d logic:
// x = i % MainWindow.size
// y = (int) (i/MainWindow.size)
//