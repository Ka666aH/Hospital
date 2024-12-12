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
using Информационная_система_для_больницы.Pages.Nurse;
using Информационная_система_для_больницы.Pages.Registrar;

namespace Информационная_система_для_больницы
{
    /// <summary>
    /// Логика взаимодействия для NurseMenu.xaml
    /// </summary>
    public partial class NurseMenu : UserControl
    {
        NursePatients _nursePatients = new NursePatients();

        public static MainWindow main = (MainWindow)Application.Current.MainWindow;
        Frame mainframe = (Frame)main.FindName("mainFrame");
        RadioButton viewButton = (RadioButton)main.FindName("generalMenuView");
        RadioButton changeUserButton = (RadioButton)main.FindName("generalMenuChangeUser");
        RadioButton closeButton = (RadioButton)main.FindName("generalMenuClose");

        public NurseMenu()
        {
            InitializeComponent();
        }

        private void nursePatients_Click(object sender, RoutedEventArgs e)
        {
            mainframe.Content = _nursePatients;
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
