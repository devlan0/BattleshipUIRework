using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BattleshipUIRework
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ThemeManager.AddAccent("AccentBaseColorBrush", new Uri("pack://application:,,,/BattleshipUIRework;component/Views/AppStyle.xaml"));
            Tuple <AppTheme, Accent> theme = ThemeManager.DetectAppStyle(Current);
            ThemeManager.ChangeAppStyle(Current, ThemeManager.GetAccent("AccentBaseColorBrush"), theme.Item1);
            base.OnStartup(e);
        }
    }
}
