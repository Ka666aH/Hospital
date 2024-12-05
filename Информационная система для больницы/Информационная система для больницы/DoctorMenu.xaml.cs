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
using Информационная_система_для_больницы.Pages.Doctor;

namespace Информационная_система_для_больницы
{
    /// <summary>
    /// Логика взаимодействия для DoctorMenu.xaml
    /// </summary>
    public partial class DoctorMenu : UserControl
    {
        DoctorPatients docPatients = new DoctorPatients();
        DoctorDrugsAndProcedures docDrugsAndProcedures = new DoctorDrugsAndProcedures();

        public static MainWindow main = (MainWindow)Application.Current.MainWindow;
        Frame mainframe = (Frame)main.FindName("mainFrame");
        RadioButton viewButton = (RadioButton)main.FindName("generalMenuView");
        RadioButton changeUserButton = (RadioButton)main.FindName("generalMenuChangeUser");
        RadioButton closeButton = (RadioButton)main.FindName("generalMenuClose");
        public DoctorMenu()
        {
            InitializeComponent();
        }
        private void doctorPatients_Click(object sender, RoutedEventArgs e)
        {
            mainframe.Content = docPatients;
            UncheckGeneralMenu();
        }

        private void doctorDrugs_Click(object sender, RoutedEventArgs e)
        {
            mainframe.Content = docDrugsAndProcedures;
            UncheckGeneralMenu();
        }
        public void UncheckGeneralMenu()
        {
            viewButton.IsChecked = false;
            changeUserButton.IsChecked = false;
            closeButton.IsChecked = false;
        }
    }
}
