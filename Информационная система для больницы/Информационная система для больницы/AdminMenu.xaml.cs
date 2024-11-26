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
using Информационная_система_для_больницы.Pages.Admin;
using Информационная_система_для_больницы.Pages.Registrar;

namespace Информационная_система_для_больницы
{
    /// <summary>
    /// Логика взаимодействия для AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu : UserControl
    {
        AdminEmployees admEmployees = new AdminEmployees();

        public static MainWindow main = (MainWindow)Application.Current.MainWindow;
        Frame mainframe = (Frame)main.FindName("mainFrame");
        public AdminMenu()
        {
            InitializeComponent();
        }

        private void adminEmployees_Click(object sender, RoutedEventArgs e)
        {
            mainframe.Content = admEmployees;
        }
    }
}
