using System;
using System.Windows;
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
        public GameView()
        {
            InitializeComponent();
        }



        // --------------------- NOT DONE ---------------------



        /// <summary>
        /// Generates UI Grid with Buttons.
        /// </summary>
        /// <param name="player">Player, whose Map shall be pronted</param>
        /// <param name="buildingPhase">Wether Buttons shall build Ships or Fire upon said.</param>
        /// <returns>Tuple: Grid (UI) and Buttons in the UI (for later recoloring)</returns>
        public static Tuple<Grid, Button[]> GenerateUIField(Player pl)
        {
            Grid field = new Grid();

            // Generate necessary Rows/Columns
            for (int i = 0; i < MainWindow._size; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                field.ColumnDefinitions.Add(col);
                RowDefinition row = new RowDefinition();
                field.RowDefinitions.Add(row);
            }
            Button[] buttons = new Button[(int)Math.Pow(MainWindow._size, 2)];
            for (int i = 0; i < MainWindow._size; i++)
            {
                Button button = new Button();
                button.HorizontalAlignment = HorizontalAlignment.Stretch;
                button.VerticalAlignment = VerticalAlignment.Stretch;
                button.Margin = new Thickness(0.5);
                int f = pl._fields[i];

                // 0 = Water
                // 1 = Land -> Ship
                // 2 = Ship
                // 3 = Hit
                // 4 = or Miss
                switch (f)
                {
                    case 0:
                        button.Background = MainWindow._water;
                        break;
                    case 1:
                        button.Background = MainWindow._land;
                        break;
                    case 2:
                        button.Background = MainWindow._ship;
                        break;
                }

                button.Click += Fire;

                field.Children.Add(button);
                Grid.SetColumn(button, (int)i / MainWindow._size);
                Grid.SetRow(button, (int)i / MainWindow._size);
                buttons[i] = button;
            }
            return Tuple.Create(field, buttons);
        }
        public static void Fire(object sender, RoutedEventArgs e)
        {

        }
    }
}
