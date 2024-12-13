using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Информационная_система_для_больницы.Properties;

namespace Информационная_система_для_больницы
{
    
    class AppTheme
    {
        //public static Dictionary<string, string> themeDic = new Dictionary<string, string>();
        public static List<ComboBoxItem> themeList = new List<ComboBoxItem>();
        public static void StartUpTheme()
        {
            themeList.Add(new ComboBoxItem { Name = "Blue", Content = "Синяя" });
            themeList.Add(new ComboBoxItem { Name = "GreyBlue", Content = "Серо-Синяя" });
            themeList.Add(new ComboBoxItem { Name = "Sienna", Content = "Охра" });
            themeList.Add(new ComboBoxItem { Name = "Red", Content = "Красная" });
            themeList.Add(new ComboBoxItem { Name = "MyLove", Content = "Надежда" });
            themeList.Add(new ComboBoxItem { Name = "HealthHarmony", Content = "Гармония здоровья" });
            //themeDic.Add("Blue", "Синяя");
            //themeDic.Add("GreyBlue", "Серо-Синяя");

            ChangeTheme(Properties.Settings.Default.theme);
        }
        public static void ChangeTheme(string themeName)
        {
            //Properties.Settings.Default.theme = themeName;
            //Properties.Settings.Default.Save();

            //ResourceDictionary theme = new ResourceDictionary()
            //{
            //    Source = new Uri($"pack://application:,,,/Themes/{themeName}.xaml", UriKind.Absolute)
            //};

            //// Удаляем предыдущую тему (если есть)
            //var existingTheme = App.Current.Resources.MergedDictionaries.FirstOrDefault(d => d.Source.ToString().Contains("Themes"));
            //if (existingTheme != null)
            //{
            //    App.Current.Resources.MergedDictionaries.Remove(existingTheme);
            //}

            //// Добавляем новую тему
            //App.Current.Resources.MergedDictionaries.Add(theme);


            Properties.Settings.Default.theme = themeName;
            Properties.Settings.Default.Save();

            ResourceDictionary theme = new ResourceDictionary()
            {
                Source = new Uri($"pack://application:,,,/Themes/{themeName}.xaml", UriKind.Absolute),
            };

            App.Current.Resources.FindName("theme");
            App.Current.Resources.MergedDictionaries.Add(theme);

        }

    }
}
