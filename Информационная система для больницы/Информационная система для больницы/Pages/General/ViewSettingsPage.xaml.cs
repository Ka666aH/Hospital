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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Информационная_система_для_больницы.Pages.General
{
    /// <summary>
    /// Логика взаимодействия для ViewSettings.xaml
    /// </summary>
    public partial class ViewSettingsPage : Page
    {
        public ViewSettingsPage()
        {
            InitializeComponent();
            themePicker.ItemsSource = AppTheme.themeList;
            //MessageBox.Show(Properties.Settings.Default.theme);

            foreach (ComboBoxItem item in themePicker.Items)
            {
                if (item.Name == Properties.Settings.Default.theme)
                    themePicker.SelectedItem = item;
            } 
        }

        private void themePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = themePicker.SelectedItem as ComboBoxItem;
            AppTheme.ChangeTheme(item.Name);
        }
    }
}
