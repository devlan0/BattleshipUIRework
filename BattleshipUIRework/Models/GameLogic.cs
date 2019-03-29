using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BattleshipUIRework.Models
{
    class GameLogic
    {
        public GameLogic()
        {

        }
        public static int CalcSize(StackPanel sp)
        {
            int height = (int) sp.ActualHeight;
            int width = (int) sp.ActualWidth;
            Console.WriteLine("Höhe: {0}, Breite: {1}", height, width);
            return (Math.Min(height, width)/MainWindow._size - MainWindow._globalMargin);
        }
        public static void Resize(Grid grid, int size)
        {
            foreach(Button button in grid.Children)
            {
                button.Width = button.Height = size;
            }
        }
    }
    public class Player
    {
        public int uiColumn;
        public Button[] buttonField;
        public int[] field;
        public string name;

        public Player(int uiColumn, string name)
        {
            this.uiColumn = uiColumn;
            this.name = name;
        }
    }
}

// () = CurrentTurn()