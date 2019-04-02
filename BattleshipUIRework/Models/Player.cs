using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BattleshipUIRework.Models
{
    public class Player
    {
        public int _uiColumn;
        public Button[] _buttonField;
        public int[] _fields;
        public string _username;

        public Player(int uiColumn, string username, int[] fields)
        {
            _uiColumn = uiColumn;
            _username = username;
            _fields = fields;
        }
    }
}
