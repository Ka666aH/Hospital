using System;
using System.Collections.Generic;
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
            //themeDic.Add("Blue", "Синяя");
            //themeDic.Add("GreyBlue", "Серо-Синяя");

            ChangeTheme(Properties.Settings.Default.theme);
        }
        public static void ChangeTheme(string themeName)
        {

            Properties.Settings.Default.theme = themeName;
            Properties.Settings.Default.Save();

            ResourceDictionary theme = new ResourceDictionary()
            {
                Source = new Uri($"Themes/{themeName}.xaml", UriKind.Relative),
            };

            App.Current.Resources.FindName("theme");
            App.Current.Resources.MergedDictionaries.Add(theme);

        }

    }

    //public class Theme
    //{
    //    public string name { get; set; }
    //    public string content { get; set; }

    //    public Theme(string name, string content) 
    //    {
    //        this.name = name;
    //        this.content = content;
    //    }
    //}
}
