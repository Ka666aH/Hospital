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
using Информационная_система_для_больницы.Data;

namespace Информационная_система_для_больницы.Pages.Registrar
{
    /// <summary>
    /// Логика взаимодействия для RegistrarRegistrations.xaml
    /// </summary>
    public partial class RegistrarRegistrations : Page
    {
        Data.AppContext db;
        bool addMode = false;
        Registration selectedRegistration = null;
        public RegistrarRegistrations()
        {
            InitializeComponent();
        }

        private void registrarRegistrationsFindRegistration_Click(object sender, RoutedEventArgs e)
        {

        }
        public void OpenListRegistrationForm()
        {
            //registrarRegistrationsRegistrationForm.Visibility = Visibility.Visible;
            //registrarRegistrationsMainPart.IsEnabled = false;

            //registrarRegistrationsRegistrationFormWard.ItemsSource = GetWards().Distinct();
        }
        public void CloseListRegistrationForm()
        {
            //registrarRegistrationsMainPart.IsEnabled = true;
            //registrarRegistrationsRegistrationForm.Visibility = Visibility.Collapsed;
        }
    }
}
