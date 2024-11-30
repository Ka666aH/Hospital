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
        AdminMenu adminMenu;
        RegistrarMenu registrarMenu;
        DoctorMenu doctorMenu;
        NurseMenu nurseMenu;

        ViewSettingsPage viewSettingsPage;
        InfoPage infoPage;

        UserControl userMenuButtons;
        public MainWindow()
        {
            InitializeComponent();

            //Loaded += MainWindow_Loaded;

            Height = MinHeight;
            Width = MinWidth;

            infoPage = new InfoPage();
            viewSettingsPage = new ViewSettingsPage();

            mainFrame.Content = infoPage;

            userMenuButtons = menuButtons.Content as UserControl;


            MaxHeight = SystemParameters.WorkArea.Height;
            MaxWidth = SystemParameters.WorkArea.Width;

        }

        private void GetUserMenu(string access)
        {
            if (access == "Администратор")
                menuButtons.Content = new AdminMenu(); //adminMenu
            if (access == "Регистратор")
                menuButtons.Content = new RegistrarMenu(); /*registrarMenu*/
            if (access == "Врач")
                menuButtons.Content = new DoctorMenu(); /*doctorMenu*/
            if (access == "Медицинский персонал")
                menuButtons.Content = new NurseMenu(); /*nurseMenu*/
            if (access == "Нет")
                {
                    MessageBox.Show("У вас пока нет доступа к системе. Обратитесь к системному администратору для получения доступа.");
                    AuthWindow auth = new AuthWindow();
                    Application.Current.MainWindow = auth;
                    auth.Show();
                    this.Close();
                }
                    
        }
                    

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            generalMenuClose.IsChecked = true;
            //UncheckUserMenu();
            MessageBox.Show("Вы уверены, что хотите выйти?");
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
            UncheckUserMenu();
            mainFrame.Content = viewSettingsPage;
        }

        private void ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            if(authWindow.ShowDialog() == true)
            {
                mainFrame.Content = infoPage;
                GetUserMenu((string)Tag);
                
            }
            else
                generalMenuChangeUser.IsChecked = false;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            GetUserMenu((string)Tag);

        }

        public void UncheckUserMenu()
        {
            //foreach (RadioButton radioButton in LogicalTreeHelper.GetChildren(userMenuButtons).OfType<RadioButton>().ToList())
            //{
            //    radioButton.IsChecked = false;
            //}
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            adminMenu = new AdminMenu();
            registrarMenu = new RegistrarMenu();
            doctorMenu = new DoctorMenu();
            nurseMenu = new NurseMenu();
        }
    }
}
