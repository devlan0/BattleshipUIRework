using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BattleshipUIRework.Models;

namespace BattleshipUIRework.Views
{
    /// <summary>
    /// Interaction logic for BuildView.xaml
    /// </summary>
    public partial class BuildView : UserControl
    {
        public BuildView()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Tuple<Grid, Button[]> tuple = GenerateUIField(MainWindow.player);
            MainWindow.player.buttonField = tuple.Item2;
            buildCol1.Children.Add(tuple.Item1);
            GameLogic.Resize((Grid)buildCol1.Children[0], GameLogic.CalcSize(buildCol1));
        }

        /// <summary>
        /// Generates UI Grid with Buttons.
        /// </summary>
        /// <param name="player">Player, whose Map shall be pronted</param>
        /// <returns>Tuple: Grid (UI) and Buttons in the UI (for later recoloring)</returns>
        public static Tuple<Grid, Button[]> GenerateUIField(Player pl)
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
            for (int i = 0; i < Math.Pow(MainWindow.size,2); i++)
            {
                Button button = new Button();
                button.HorizontalAlignment = HorizontalAlignment.Stretch;
                button.VerticalAlignment = VerticalAlignment.Stretch;
                button.Background = MainWindow.colorDic[pl.field[i]];

                button.Click += SetShip;

                field.Children.Add(button);
                Grid.SetColumn(button, i % MainWindow.size);
                Grid.SetRow(button, (int)(i / MainWindow.size));
                buttons[i] = button;
            }
            return Tuple.Create(field, buttons);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Obligatory</param>
        /// <param name="e">Obligatory</param>
        public static void SetShip(object sender, RoutedEventArgs e)
        {
            Button send = (Button)sender;
            int index = Array.IndexOf(MainWindow.player.buttonField, send);
            MainWindow.player.field[index] = 2;
            MainWindow.player.buttonField[index].Background = MainWindow.ship;

            //more building logic

            // ein Schlachtschiff (5 Kästchen)
            // zwei Kreuzer(je 4 Kästchen)
            // drei Zerstörer(je 3 Kästchen)
            // vier U-Boote(je 2 Kästchen)
        }



        private async void SendShipsBtn_Clicked(object sender, RoutedEventArgs e)
        {
            string status = "";
            string message = "No response from the Server.";
            if (App.DEBUG_MODE)
            {
                status = "success";
            }
            else
            {
                (status, message) = await HttpBattleshipClient.SubmitBattleships(MainWindow.player.field, MainWindow.player.name, MainWindow.token);
            }
            
            if(status.Equals("success"))
            {
                Window.GetWindow(this).DataContext = new WaitOpponentView();
            }
            else
            {
                ErrorLabel.Content = message;
                Array.Copy(MainWindow.player.originalField, 0, MainWindow.player.field, 0, MainWindow.player.originalField.Length);
                int i = 0;
                foreach(Button button in MainWindow.player.buttonField)
                {
                    button.Background = MainWindow.colorDic[MainWindow.player.field[i]];
                    i++;
                }
            }
        }
    }
}


// 1d to 2d logic:
// x = i % MainWindow._size
// y = (int) (i/MainWindow._size)
//
//
//
//
//
//