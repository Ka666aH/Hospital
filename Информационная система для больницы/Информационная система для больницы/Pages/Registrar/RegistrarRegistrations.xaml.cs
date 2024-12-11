using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
            SetSearchingItems();
        }

        public void GetCurrentRegistrationsAmount()
        {
            db = new Data.AppContext();
            
            var allRegistrations = db.Registrations.ToList();
            var currentRegistrations = from r in allRegistrations
                                       let endDate = string.IsNullOrEmpty(r.end) ? (DateTime?)null : Convert.ToDateTime(r.end)
                                       where endDate == null || endDate > DateTime.Today
                                       select r;

            registrarRegistrationsCurrentRegistrationsAmount.Content = $"Количество текущих госпитализаций : {currentRegistrations.Count()}";
        }

        public void SetSearchingItems()
        {
            registrarRegistrationsSearchPatient.ItemsSource = GetPatientsList();
            registrarRegistrationsSearchDoctor.ItemsSource = GetDoctorsList();
            registrarRegistrationsSearchWard.ItemsSource = GetWardsList();
        }

        public List<string> GetPatientsList()
        {
            db = new Data.AppContext();

            var _patients = from p in db.Patients
                           select p.fullName;

            List<string> patients = new List<string>{string.Empty};
            patients.AddRange( _patients.OrderBy(x => x).ToList());
            //registrarRegistrationsSearchPatient.ItemsSource = patients.ToList().OrderBy(x => x);
            return patients;

        }
        public List<string> GetDoctorsList()
        {
            db = new Data.AppContext();
            var _doctors = from d in db.Employees
                          where d.access == "Врач"
                          select d.fullName;
            List<string> doctors = new List<string> { string.Empty };
            doctors.AddRange(_doctors.OrderBy(x => x).ToList());
            return doctors;
        }
        public List<string> GetWardsList()
        {
            db = new Data.AppContext();
            var _wards = from w in db.Beds
                       select w.ward;
            List<string> wards = new List<string> { string.Empty };
            wards.AddRange(_wards.Distinct().OrderBy(x => x).ToList());
            return wards;
        }

        private void registrarRegistrationsAddRegistration_Click(object sender, RoutedEventArgs e)
        {
            addMode = true;
            registrarRegistrationsRegistrationFormPatient.Text = string.Empty;
            registrarRegistrationsRegistrationFormReason.Text = string.Empty;
            registrarRegistrationsRegistrationFormStart.Text = string.Empty;
            registrarRegistrationsRegistrationFormEnd.Text = string.Empty;
            registrarRegistrationsRegistrationFormDoctor.Text = string.Empty;
            registrarRegistrationsRegistrationFormWard.Text = string.Empty;
            registrarRegistrationsRegistrationFormBed.Text = string.Empty;
            OpenRegistrationForm();
        }
        private void registrarRegistrationsFindRegistration_Click(object sender, RoutedEventArgs e)
        {

            var registrations = GetFiltredRegistrations();

            if(registrations.ToList().Count()>0)
            {
                registrarRegistrationsDataGrid.ItemsSource = registrations.ToList();


                registrarRegistrationsListRegistrationForm.Visibility = Visibility.Visible;

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

            return registrations.AsEnumerable().Cast<dynamic>().ToList();
            //return registrations.Select(r => (object)r).ToList();
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
            {
                GetCurrentRegistrationsAmount();
                SetSearchingItems();
            }
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

            registrarRegistrationsRegistrationFormPatient.ItemsSource = GetPatientsList();
            registrarRegistrationsRegistrationFormDoctor.ItemsSource = GetDoctorsList();
            registrarRegistrationsRegistrationFormWard.ItemsSource = GetWardsList();

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
            registrarRegistrationsRegistrationForm.Visibility = Visibility.Collapsed;
            GetCurrentRegistrationsAmount();
        }

        private void registrarRegistrationsRegistrationFormWard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(registrarRegistrationsRegistrationFormWard.SelectedValue != null)
            {
            registrarRegistrationsRegistrationFormWard.Text = registrarRegistrationsRegistrationFormWard.SelectedValue.ToString();
            registrarRegistrationsRegistrationFormBed.Text = string.Empty;
            }

            var registrationsWithDates = db.Registrations.ToList()
                .Select(r => new
                {
                    r.bedId,
                    EndDate = r.end != null ? DateTime.ParseExact(r.end, "dd.MM.yyyy", CultureInfo.InvariantCulture) : (DateTime?)null
                });

            var bedsWithLatestEnd = from r in registrationsWithDates
                                    group r by r.bedId into g
                                    select new
                                    {
                                        BedId = g.Key,
                                        LatestEnd = g.Max(r => r.EndDate)
                                    };

            var usingBeds = bedsWithLatestEnd.Where(x => x.LatestEnd == null || x.LatestEnd > DateTime.Today).Select(x => x.BedId).ToList();


            var beds = from b in db.Beds
                       where b.ward == registrarRegistrationsRegistrationFormWard.Text && !usingBeds.Contains(b.id)
                       select b.bed;

            registrarRegistrationsRegistrationFormBed.ItemsSource = beds.ToList().OrderBy(x => x);
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
                GetCurrentRegistrationsAmount();
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
                                  select p.id;

                    var doctor = from d in db.Employees
                                 where d.fullName == registrarRegistrationsRegistrationFormDoctor.Text
                                 select d.id;
                    var bed = from b in db.Beds
                              where b.ward == registrarRegistrationsRegistrationFormWard.Text && b.bed == registrarRegistrationsRegistrationFormBed.Text
                              select b.id;

                    Registration registration = new Registration();

                    registration.patientId = patient.FirstOrDefault().ToString();
                    registration.reason = registrarRegistrationsRegistrationFormReason.Text;
                    registration.start = registrarRegistrationsRegistrationFormStart.Text;
                    registration.end = registrarRegistrationsRegistrationFormEnd.Text;
                    registration.doctorId = doctor.FirstOrDefault().ToString();
                    registration.bedId = bed.FirstOrDefault().ToString();
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
                CloseRegistrationForm();
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


        private void registrarRegistrationsDataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            registrarRegistrationsDataGrid.Columns[0].Header = "Идентификатор";
            registrarRegistrationsDataGrid.Columns[1].Header = "Пациент";
            registrarRegistrationsDataGrid.Columns[2].Header = "Причина госпитализации";
            registrarRegistrationsDataGrid.Columns[3].Header = "Дата начала";
            registrarRegistrationsDataGrid.Columns[4].Header = "Дата окончания";
            registrarRegistrationsDataGrid.Columns[5].Header = "Лечащий врач";
            registrarRegistrationsDataGrid.Columns[6].Header = "Палата";
            registrarRegistrationsDataGrid.Columns[7].Header = "Кровать";

            registrarRegistrationsDataGrid.Columns[0].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            registrarRegistrationsDataGrid.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            registrarRegistrationsDataGrid.Columns[2].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            registrarRegistrationsDataGrid.Columns[3].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            registrarRegistrationsDataGrid.Columns[4].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            registrarRegistrationsDataGrid.Columns[5].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            registrarRegistrationsDataGrid.Columns[6].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            registrarRegistrationsDataGrid.Columns[7].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);

            registrarRegistrationsDataGrid.Columns[0].Visibility = Visibility.Collapsed;

            registrarRegistrationsDataGrid.SelectedIndex = 0;
        }
    }
}