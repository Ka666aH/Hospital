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

namespace Информационная_система_для_больницы.Pages.Nurse
{
    /// <summary>
    /// Логика взаимодействия для NursePatients.xaml
    /// </summary>
    public partial class NursePatients : Page
    {
        Data.AppContext db;

        public static MainWindow main = (MainWindow)Application.Current.MainWindow;

        bool appointmentAddMode = false;

        Registration selectedPatient = null;
        Indicator selectedIndicator = null;
        Appointment selectedAppointment = null;
        Schedule selectedSchedule = null;

        string completed = "Выполнено";
        string uncompleted = "Не выполнено";
        public NursePatients()
        {
            InitializeComponent();
            SetData();
        }

        public void SetData()
        {
            db = new Data.AppContext();
            nursePatientsCurrentPatientsAmount.Content = $"Количество текущих госпитализаций : {GetCurrentRegistration().Count()}";
            nursePatientsSearchPatient.ItemsSource = GetCurrentRegistration().Select(r => r.patient.fullName).ToList();
            nursePatientsSearchWard.ItemsSource = GetCurrentRegistration().Select(x => x.bed.ward).Distinct().ToList();
        }

        public List<Registration> GetCurrentRegistration()
        {
            var registrations = db.Registrations.ToList();
            var patients = from r in registrations
                           where r.end == null || Convert.ToDateTime(r.end) > DateTime.Today
                           select r;
            return patients.ToList();
        }
    }
}
