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