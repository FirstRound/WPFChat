using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ModernUINavigationApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppearanceManager.Current.AccentColor = System.Windows.Media.Color.FromRgb(69, 45, 8);
            AppearanceManager.Current.ThemeSource = AppearanceManager.DarkThemeSource;

        }
    }
}
