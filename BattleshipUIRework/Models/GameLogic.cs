﻿using System;
using System.Windows;
using System.Collections.Generic;
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
            int height = (int)sp.ActualHeight;
            int width = (int)sp.ActualWidth;
            Console.WriteLine("Höhe: {0}, Breite: {1}", height, width);
            return (Math.Min(height, width) - 2 * MainWindow.globalMargin) / MainWindow.size;
        }
        public static void Resize(Grid grid, int size)
        {
            foreach (Button button in grid.Children)
            {
                button.Width = button.Height = size;
            }
        }
    }
}