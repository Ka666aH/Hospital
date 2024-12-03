using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Информационная_система_для_больницы.Properties;

namespace Информационная_система_для_больницы
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.Default.theme))
                Settings.Default.theme = "Blue";
            AppTheme.StartUpTheme();
            //AppTheme.ChangeTheme(Settings.Default.theme);

        }
    }
}
