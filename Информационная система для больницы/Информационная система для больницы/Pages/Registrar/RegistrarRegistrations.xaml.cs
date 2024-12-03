using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
        dynamic selectedRegistration = null;
        public RegistrarRegistrations()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetCurrentRegistrationsAmount();
        }

        public void GetCurrentRegistrationsAmount()
        {
            db = new Data.AppContext();
            var currentRegistration = from r in db.Registrations
                                      where string.IsNullOrEmpty(r.end) || Convert.ToDateTime(r.end) < DateTime.Today
                                      select r;
            registrarRegistrationsCurrentRegistrationsAmount.Content = $"Количество текущих госпитализаций : {currentRegistration.ToList().Count()}";
        }

        private void registrarRegistrationsAddRegistration_Click(object sender, RoutedEventArgs e)
        {
            addMode = true;
            OpenRegistrationForm();
        }
        private void registrarRegistrationsFindRegistration_Click(object sender, RoutedEventArgs e)
        {

            var registrations = GetFiltredRegistrations();

            if(registrations.ToList().Count()>0)
            {
                registrarRegistrationsDataGrid.ItemsSource = registrations.ToList();
            }
            else
            {
                MessageBox.Show("Оформления не найдены.");
            }
        }

        public List<object> GetFiltredRegistrations()
        {
            var registrations = from r in db.Registrations
                                where
                                (string.IsNullOrEmpty(registrarRegistrationsSearchPatient.Text) || r.patient.fullName   == registrarRegistrationsSearchPatient.Text)  &&
                                (string.IsNullOrEmpty(registrarRegistrationsSearchDoctor.Text)  || r.doctor.fullName    == registrarRegistrationsSearchDoctor.Text)   &&
                                (string.IsNullOrEmpty(registrarRegistrationsSearchWard.Text)    || r.bed.ward           == registrarRegistrationsSearchWard.Text)     &&
                                (string.IsNullOrEmpty(registrarRegistrationsSearchDate.Text)    || r.start              == registrarRegistrationsSearchDate.Text)
                                select new
                                {
                                    Id = r.id,
                                    Patient = r.patient.fullName,
                                    Reason = r.reason,
                                    Start = r.start,
                                    End = r.end,
                                    Doctor = r.doctor.fullName,
                                    Ward = r.bed.ward,
                                    Bed = r.bed.bed
                                };

            return registrations.Select(r => (object)r).ToList();
        }
        private void registrarRegistrationsRegistrationListFormCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseListRegistrationForm();
        }

        public void OpenListRegistrationForm()
        {
            registrarRegistrationsListRegistrationForm.Visibility = Visibility.Visible;
            registrarRegistrationsMainPart.IsEnabled = false;
        }
        public void CloseListRegistrationForm()
        {
            registrarRegistrationsMainPart.IsEnabled = true;
            registrarRegistrationsListRegistrationForm.Visibility = Visibility.Collapsed;
        }

        private void registrarRegistrationsMainPart_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (registrarRegistrationsMainPart.IsEnabled)
                GetCurrentRegistrationsAmount();
        }

        private void registrarRegistrationsListFormAlter_Click(object sender, RoutedEventArgs e)
        {
            addMode = false;

            registrarRegistrationsRegistrationFormPatient.Text  = (string)selectedRegistration.Patient;
            registrarRegistrationsRegistrationFormReason.Text   = (string)selectedRegistration.Reason;
            registrarRegistrationsRegistrationFormStart.Text    = (string)selectedRegistration.Start;
            registrarRegistrationsRegistrationFormEnd.Text      = (string)selectedRegistration.End;
            registrarRegistrationsRegistrationFormDoctor.Text   = (string)selectedRegistration.Doctor;
            registrarRegistrationsRegistrationFormWard.Text     = (string)selectedRegistration.Ward;
            registrarRegistrationsRegistrationFormBed.Text      = (string)selectedRegistration.Bed;

            OpenRegistrationForm();
        }

        public void OpenRegistrationForm()
        {
            var patients = from p in db.Patients
                           select p.fullName;

            var doctors = from p in db.Employees
                          where p.access == "Врач"
                          select p.fullName;

            var wards = from w in db.Beds
                       select w.ward;

            registrarRegistrationsRegistrationFormPatient.ItemsSource = patients.ToList();
            registrarRegistrationsRegistrationFormDoctor.ItemsSource = doctors.ToList();
            registrarRegistrationsRegistrationFormWard.ItemsSource = wards.ToList();

            registrarRegistrationsRegistrationForm.Visibility = Visibility.Visible;
            if (addMode)
                registrarRegistrationsMainPart.IsEnabled = false;
            else
                registrarRegistrationsListRegistrationForm.IsEnabled = false;

        }
        public void CloseRegistrationForm()
        {
            if (addMode)
                registrarRegistrationsMainPart.IsEnabled = true;
            else
                registrarRegistrationsListRegistrationForm.IsEnabled = true;
            registrarRegistrationsRegistrationForm.Visibility = Visibility.Visible;
        }

        private void registrarRegistrationsRegistrationFormWard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            registrarRegistrationsRegistrationFormWard.Text = registrarRegistrationsRegistrationFormWard.SelectedValue.ToString();
            registrarRegistrationsRegistrationFormBed.Text = string.Empty;

            //! r.bedId?
            var bedsWithLatestEnd = from r in db.Registrations
                              group r by r.bedId into g
                              select new
                              {
                                  BedId = g.Key,
                                  LatestEnd = g.Max(r => r.end != null ? Convert.ToDateTime(r.end) : (DateTime?)null),
                              };
            var usingBeds = bedsWithLatestEnd.Where(x => x.LatestEnd == null || x.LatestEnd < DateTime.Today).Select(x => x.BedId);
            var beds = from b in db.Beds
                       where b.ward == registrarRegistrationsRegistrationFormWard.Text && usingBeds.ToList().Contains(b.id)
                       select b.bed;

            registrarRegistrationsRegistrationFormBed.ItemsSource = beds.ToList();
        }

        private void registrarRegistrationsListFormDelete_Click(object sender, RoutedEventArgs e)
        {
            string selectedId = selectedRegistration.Id;
            var appointments = from a in db.Appointments
                               where a.registrationId == selectedId
                               select a;

            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите безвозвратно удалить это оформление госпитализации?\nВместе с этой записью будут удалены {appointments.Count()} связанных записей из таблицы Назначения.\nЭто действие нельзя отменить.", "Удаление оформления гостпитализации", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var selectedReg = from r in db.Registrations
                                  where r.id == selectedId
                                  select r;
                Registration registration = selectedReg.First();

                //db.Registrations.Remove(registration);
                db.Registrations.Attach(registration);
                db.Entry(registration).State = EntityState.Deleted;
                db.SaveChanges();
                registrarRegistrationsDataGrid.ItemsSource = GetFiltredRegistrations().ToList();
            }
        }

        private void registrarRegistrationsDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            selectedRegistration = registrarRegistrationsDataGrid.SelectedItem;

            if (registrarRegistrationsDataGrid.SelectedItem == null)
            {
                registrarRegistrationsListFormAlter.IsEnabled = false;
                registrarRegistrationsListFormDelete.IsEnabled = false;
            }
            else
            {
                registrarRegistrationsListFormAlter.IsEnabled = true;
                registrarRegistrationsListFormDelete.IsEnabled = true;
            }
        }

        private void registrarRegistrationsRegistrationFormAccept_Click(object sender, RoutedEventArgs e)
        {
            if(
                !string.IsNullOrEmpty(registrarRegistrationsRegistrationFormPatient.Text)   && 
                !string.IsNullOrEmpty(registrarRegistrationsRegistrationFormReason.Text)    && 
                !string.IsNullOrEmpty(registrarRegistrationsRegistrationFormStart.Text)     && 
                !string.IsNullOrEmpty(registrarRegistrationsRegistrationFormDoctor.Text)    && 
                !string.IsNullOrEmpty(registrarRegistrationsRegistrationFormWard.Text)      && 
                !string.IsNullOrEmpty(registrarRegistrationsRegistrationFormBed.Text)   
                )
            {
                    var patient = from p in db.Patients
                                  where p.fullName == registrarRegistrationsRegistrationFormPatient.Text
                                  select p.id.First();

                    var doctor = from d in db.Employees
                                 where d.fullName == registrarRegistrationsRegistrationFormDoctor.Text
                                 select d.id.First();
                    var bed = from b in db.Beds
                              where b.ward == registrarRegistrationsRegistrationFormWard.Text && b.bed == registrarRegistrationsRegistrationFormBed.Text
                              select b.id.First();

                    Registration registration = new Registration();


                    registration.patientId = patient.ToString();
                    registration.reason = registrarRegistrationsRegistrationFormReason.Text;
                    registration.start = registrarRegistrationsRegistrationFormStart.Text;
                    registration.end = registrarRegistrationsRegistrationFormEnd.Text;
                    registration.doctorId = doctor.ToString();
                    registration.bedId = bed.ToString();
                if (addMode)
                {
                    registration.id = Guid.NewGuid().ToString();
                    db.Registrations.Add(registration);
                }
                else
                {                
                    registration.id = selectedRegistration.Id;
                    db.Registrations.AddOrUpdate(registration);
                }
                    db.SaveChanges();
            }
            else
            {
                MessageBox.Show("Необходимо заполнить все обязательные поля");
            }
        }

        private void registrarRegistrationsRegistrationFormCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseRegistrationForm();
        }


    }
}