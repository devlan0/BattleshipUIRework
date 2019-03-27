using System;
using System.Security.Cryptography;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BattleshipUIRework
{
    public partial class MainWindowOld : Window
    {
        private static Player player1;
        private static SolidColorBrush _colorPlayer1 = new SolidColorBrush(Color.FromArgb(255, 110, 90, 125));
        private static SolidColorBrush _colorPlayer1Dark = new SolidColorBrush(Color.FromArgb(255, 49, 40, 56));
        private static SolidColorBrush _colorPlayer1Light = new SolidColorBrush(Color.FromArgb(255, 162, 132, 184));

        private static Player player2;
        private static SolidColorBrush _colorPlayer2 = new SolidColorBrush(Color.FromArgb(255, 245, 115, 130));
        private static SolidColorBrush _colorPlayer2Dark = new SolidColorBrush(Color.FromArgb(255, 74, 35, 39));
        private static SolidColorBrush _colorPlayer2Light = new SolidColorBrush(Color.FromArgb(255, 222, 167, 189));

        private static string _token;
        private static string _name;
        private static string _gameID;
        private static string _opponent;

        private static int _globalMargin = 10;
        private static int _size = 15;

        private static int _stage = 0;      //0 = Login/Whatever, 1 = Build, 2 = Fight
        private static int _buildStage = 0; //0 = Build, 1 = Restricted

        /// <summary>
        /// Main Window
        /// </summary>
        public MainWindowOld()
        {
            InitializeComponent();
        }

        #region WindowResize
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_stage > 0)
            {
                CalcFieldSize();
            }
        }
        
        private void CalcFieldSize()
        {
            int height, width, min;
            if (_stage == 1)
            {
                height = (int)buildCol1.ActualHeight - 2 * _globalMargin;
                width = (int)buildCol1.ActualWidth - 2 * _globalMargin;
                min = Math.Min(height, width) / 15;
            }
            else
            {
                height = (int)buildCol1.ActualHeight - 2 * _globalMargin;
                width = (int)buildCol1.ActualWidth - 2 * _globalMargin;
                min = Math.Min(height, width) / 15;
                /*height = (int)gameColumn1.ActualHeight - 2 * _globalMargin;
                width = (int)gameColumn1.ActualWidth - 2 * _globalMargin;
                min = Math.Min(height, width) / 15;

                player2.ChangedSize(min);
            }

            player1.ChangedSize(min);
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /*loginButton.Click += Login;
            registerButton.Click += RegisterStart;
            registerAccountButton.Click += RegisterEnd;
            submitShips.Click += SubmitBattleships;
            Logout();*/
        }

        //stack.Children.Remove((ContentPanel.FindName("textBox1") as TextBox));
        //YourTextBox.Visibility = Visibility.Collapsed;
        private async void BuildShips()
        {
            //Generate 2 Fields with colors
            //Grid opponentField = GenerateField(_colorPlayer2);
            player1.SetUIField(GenerateUIField(player1));
            buildCol1.Children.Add(player1.uiField);
            Grid.SetColumn(player1.uiField, player1.uiColumn);
            CalcFieldSize();

            //column2.Children.Add(opponentField);
            string matchID, int[] field, string opponent) = await HttpBattleshipClient.MatchFound(_name, _token);

            int column,row = column = 0;
            foreach (int square in field)
            {
                _playerField[column, row] = square;
                column++;
                if (column == 15)
                {
                    row++;
                    column %= 15;
                }
            }
            //player1.PrintField();
        }

        private async void SubmitBattleships(object sender, RoutedEventArgs e)
        {
            //(string status, string message) = await HttpBattleshipClient.SubmitBattleships(ConvertMap(player1.field), _name, _token);
            Game();
        }

        private void Game()
        {
            _stage++;
            buildCol1.Children.Remove(player1.uiField);
            buildShipsGrid.Visibility = Visibility.Collapsed;
            gameGrid.Visibility = Visibility.Visible;

            //Disable Names/Eventlistener player1
            Random rand = new Random();
            foreach (Button button in player1.uiField.Children)
            {
                button.Click -= SetShip;
                button.Name = "a" + rand.Next().ToString();
            }
            gameColumn1.Children.Add(player1.uiField);
            Grid.SetColumn(player1.uiField, player1.uiColumn);


            player2 = new Player(_colorPlayer2, _colorPlayer2Dark, _colorPlayer2Light, 1, gameGrid, _size);
            int[] map = Enumerable.Range(0, 15 * 15).Select(n => 0).ToArray();                              //hack
            player2.field = ConvertMap(map);
            player2.SetUIField(GenerateUIField(player2));
            gameColumn2.Children.Add(player2.uiField);
            Grid.SetColumn(player2.uiField, player2.uiColumn);
            CalcFieldSize();
        }


        void SetShip(object sender, RoutedEventArgs e)
        {
            if (_buildStage == 0)
            {
                Button send = (Button)sender;
                string[] text = send.Name.Split('b');
                int x = Int32.Parse(text[1]);
                int y = Int32.Parse(text[2]);
                debugText.Text = x + "|" + y;
                player1.field[x, y] = 1;
                player1.buttonField[x, y].Background = player1.dark;

                //more building logic
            }
        }

        async void Fire(object sender, RoutedEventArgs e)
        {
            Button send = (Button)sender;
            string[] text = send.Name.Split('b');
            debugText2.Text = text[1] + "|" + text[2];
            //(string status, string message) = await HttpBattleshipClient.ShotFired(Int32.Parse(text[1]), Int32.Parse(text[2]), _name, _token);
            //antwortlogik
        }

    }

    //Playerclass, handles everything playerbased (like individual fields, colors etc.)
    /*internal class Player
    {
        public SolidColorBrush color;
        public SolidColorBrush dark;
        public SolidColorBrush light;
        public int uiColumn;
        public Grid uiField;
        public Button[,] buttonField;
        public int[,] field;
        private int _size;

        public Player(SolidColorBrush color, SolidColorBrush dark, SolidColorBrush light, int uiColumn, Grid subgrid, int size)
        {
            this.color = color;
            this.dark = dark;
            this.light = light;
            this.field = new int[_size, _size];
            this.uiColumn = uiColumn;
            this._size = size;
        }
        public void SetUIField(Tuple<Grid, Button[,]> tuple)
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
    }*/
}
