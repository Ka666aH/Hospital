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
using Информационная_система_для_больницы.Pages.Registrar;

namespace Информационная_система_для_больницы
{
    /// <summary>
    /// Логика взаимодействия для RegistrarMenu.xaml
    /// </summary>
    public partial class RegistrarMenu : UserControl
    {
        RegistrarEmployees regEmployees = new RegistrarEmployees();
        RegistrarPatients regPatients = new RegistrarPatients();
        RegistrarDrugs regDrugs = new RegistrarDrugs();
        RegistrarProcedures regProcedures = new RegistrarProcedures();
        RegistrarIndicators regIndicators = new RegistrarIndicators();
        RegistrarWards regWards = new RegistrarWards();

        public static MainWindow main = (MainWindow)Application.Current.MainWindow;
        Frame mainframe = (Frame)main.FindName("mainFrame");
        public RegistrarMenu()
        {
            InitializeComponent();
            
        }

        private void registrarEmployees_Click(object sender, RoutedEventArgs e)
        {
            mainframe.Content = regEmployees;
        }

        private void registrarPatients_Click(object sender, RoutedEventArgs e)
        {
            mainframe.Content = regPatients;
        }

        private void registrarDrugs_Click(object sender, RoutedEventArgs e)
        {
            mainframe.Content = regDrugs;
        }

        private void registrarProcedures_Click(object sender, RoutedEventArgs e)
        {
            mainframe.Content = regProcedures;
        }

        private void registrarIndicators_Click(object sender, RoutedEventArgs e)
        {
            mainframe.Content = regIndicators;
        }

        private void registrarWards_Click(object sender, RoutedEventArgs e)
        {
            mainframe.Content = regWards;
        }
    }
}
