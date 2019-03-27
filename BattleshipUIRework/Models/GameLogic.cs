using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BattleshipUIRework.Models
{
    class GameLogic
    {
        public static int _globalMargin = 10;
        public static int _size = 15;
        private static SolidColorBrush _water = new SolidColorBrush(Color.FromArgb(255, 174, 197, 214));
        private static SolidColorBrush _land = new SolidColorBrush(Color.FromArgb(255, 233, 240, 116));
        private static SolidColorBrush _ship = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
        private static SolidColorBrush _hit = new SolidColorBrush(Color.FromArgb(255, 212, 4, 36));
        /// <summary>
        /// Converts between 1D and 2D array representation of the map
        /// </summary>
        /// <param name="rawData">2D/1D representation of map thats to be converted</param>
        /// <returns>2D/1D representation of the map</returns>

        #region ConvertMap
        // 1-Dimensional -> 2-Dimensional representation of Game
        public static int[,] ConvertMap(int[] rawData)
        {
            int[,] map = new int[_size, _size];
            int i = 0;
            foreach (int tile in rawData)
            {
                map[i % _size, (int)Math.Floor((double)(i / _size))] = tile; //Dirty
                i++;
            }

            return map;
        }
        // 2-Dimensional -> 1-Dimensional representation of Game
        public static int[] ConvertMap(int[,] rawData)
        {
            int[] map = new int[(int)Math.Pow(_size, 2)];
            for (int i = 0; i < map.Length; i++)
            {
                map[i] = rawData[i % _size, (int)i / _size];
            }

            return map;
        }
        #endregion

        /// <summary>
        /// Generates UI Grid with Buttons.
        /// </summary>
        /// <param name="player">Player, whose Map shall be pronted</param>
        /// <param name="field">Grid in which he shall put the Grid</param>
        /// <param name="buildingPhase">Wether Buttons shall build Ships or Fire upon said.</param>
        /// <returns>Tuple: Grid (UI) and Buttons in the UI (for later recoloring)</returns>
        public static Tuple<Grid, Button[,]> GenerateUIField(Player player, Grid field, bool buildingPhase)
        {
            // Generate necessary Rows/Columns
            for (int i = 0; i < _size; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                field.ColumnDefinitions.Add(col);
                RowDefinition row = new RowDefinition();
                field.RowDefinitions.Add(row);
            }
            Button[,] buttons = new Button[_size, _size];
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    Button button = new Button();
                    button.Name = (string)("b" + j + "b" + i);
                    button.HorizontalAlignment = HorizontalAlignment.Stretch;
                    button.VerticalAlignment = VerticalAlignment.Stretch;
                    button.Margin = new Thickness(0.5);
                    int f = player.field[j, i];
                    switch (f)
                    {
                        case 0:
                            button.Background = _water;
                            break;
                        case 1:
                            button.Background = _land;
                            break;
                        case 2:
                            button.Background = _ship;
                            break;
                    }

                    if (buildingPhase) button.Click += player.SetShip;
                    else button.Click += Fire;

                    field.Children.Add(button);
                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    buttons[j, i] = button;
                }
            }
            return Tuple.Create(field, buttons);
        }
        public static void Fire(object sender, RoutedEventArgs e)
        {

        }
        public void SetShip(object sender, RoutedEventArgs e)
        {
            Button send = (Button)sender;
            string[] text = send.Name.Split('b');
            int x = Int32.Parse(text[1]);
            int y = Int32.Parse(text[2]);
            this.field[x, y] = 2;
            this.buttonField[x, y].Background = _ship;

            //more building logic
        }
    }
    class Player
    {
        public int uiColumn;
        public Grid uiField;
        public Button[,] buttonField;
        public int[,] field;
        private int _size;

        public Player(int size, int[,] map, int uiColumn)
        {
            this._size = size;
            this.field = map;
            this.uiColumn = uiColumn;
        }
        public void SetField(Tuple<Grid, Button[,]> tuple)
        {
            this.uiField = tuple.Item1;
            this.buttonField = tuple.Item2;
        }
        public void ChangedSize(int size)
        {
            foreach (Button button in uiField.Children)
            {
                button.Width = size;
                button.Height = size;
            }
        }
    }
}
