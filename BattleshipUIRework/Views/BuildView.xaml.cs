using System;
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
        public BuildView(int[] map)
        {
            InitializeComponent();
            Player player = new Player(0, GameLogic.ConvertMap(map), GameLogic._size);
            player.SetField(GameLogic.GenerateUIField(player, buildCol1, true));
            buildCol1.Children.Add(player.uiField);
            Grid.SetColumn(player.uiField, player.uiColumn);
            CalcFieldSize();
        }
    }
}
