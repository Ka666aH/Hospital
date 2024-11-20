using System;
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
            //foreach (ComboBoxItem item in AppTheme.themeList)
            //{
            //    //ComboBoxItem newitem = new ComboBoxItem();
            //    //newitem.Name = item.Name;
            //    //newitem.Content= item.Content;

            //}

        }
    }
}
