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
        public Button[] buttonField;
        public int[] field;
        public int[] originalField;
        public string name;

        public Player()
        {
        }
    }
}
