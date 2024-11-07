using System;
using System.Windows;

namespace WpfTest
{
    class AppTheme
    {
        public static void ChangeTheme(Uri themeUri)
        {
            ResourceDictionary theme = new ResourceDictionary()
            {
                Source = themeUri,
            };

            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(theme);

        }

    }
}
