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
using Информационная_система_для_больницы.Pages.General;

namespace Информационная_система_для_больницы
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewSettingsPage viewSettingsPage;
        InfoPage infoPage;
        public MainWindow()
        {
            InitializeComponent();

            infoPage = new InfoPage();
            viewSettingsPage = new ViewSettingsPage();

            mainFrame.Content = infoPage;

            MaxHeight = SystemParameters.WorkArea.Height;
            MaxWidth = SystemParameters.WorkArea.Width;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new InfoPage();
        }

        private void Min_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        double lastHeight = 0;
        double lastWidth = 0;
        double lastLeft = 0;
        double lastTop = 0;
        private void Max_Click(object sender, RoutedEventArgs e)
        {
            if (App.Current.MainWindow.ActualHeight != App.Current.MainWindow.MaxHeight || App.Current.MainWindow.ActualWidth != App.Current.MainWindow.MaxWidth)
            {
                lastHeight = App.Current.MainWindow.ActualHeight;
                lastWidth = App.Current.MainWindow.ActualWidth;
                lastLeft = App.Current.MainWindow.Left;
                lastTop = App.Current.MainWindow.Top;

                App.Current.MainWindow.Left = 0;
                App.Current.MainWindow.Top = 0;
                App.Current.MainWindow.Height = App.Current.MainWindow.MaxHeight;
                App.Current.MainWindow.Width = App.Current.MainWindow.MaxWidth;
                windowBorder.CornerRadius = new CornerRadius(0);

            }
            else
            {
                if (lastHeight != 0)
                {
                    App.Current.MainWindow.Left = lastLeft;
                    App.Current.MainWindow.Top = lastTop;
                    App.Current.MainWindow.Height = lastHeight;
                    App.Current.MainWindow.Width = lastWidth;

                }
                else
                {
                    App.Current.MainWindow.Left = 0;
                    App.Current.MainWindow.Top = 0;
                    App.Current.MainWindow.Height = App.Current.MainWindow.MinHeight;
                    App.Current.MainWindow.Width = App.Current.MainWindow.MinWidth;
                }
                windowBorder.CornerRadius = new CornerRadius(10);
            }
        }

        private void DragMove(object sender, MouseButtonEventArgs e)
        {
            App.Current.MainWindow.DragMove();
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = viewSettingsPage;
        }
    }
}
