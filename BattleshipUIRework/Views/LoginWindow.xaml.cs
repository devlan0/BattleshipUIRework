﻿using System;
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
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace BattleshipUIRework
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {
        private SolidColorBrush window_btn_color = new SolidColorBrush(Color.FromRgb(255,255,255));

        public LoginWindow()
        {
            
            InitializeComponent();
        }

        private void CloseBtn_Clicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}