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
    /// Логика взаимодействия для RegistrarPatients.xaml
    /// </summary>
    public partial class RegistrarPatients : Page
    {
        Data.AppContext db;
        bool addMode = false;
        Patient selectedPatient = null;
        //string selectedName = null;
        //string selectedAccess = null;
        public RegistrarPatients()
        {
            InitializeComponent();
        }

        public void GetPatients()
        {
            db = new Data.AppContext();

            var patients = from p in db.Patients
                           select p;

            if (string.IsNullOrEmpty(registrarPatientsSearchFullName.Text))
                registrarPatientsDataGrid.ItemsSource = patients.ToList();
            else
                 registrarPatientsDataGrid.ItemsSource = patients.ToList().Where(x => x.fullName.ToLower().Contains(registrarPatientsSearchFullName.Text.ToLower()));

            if (registrarPatientsDataGrid.Items.Count != 0)
            {
                registrarPatientsDataGrid.SelectedIndex = 0;
                registrarPatientsDataGrid.Columns[0].Header = "Идентификатор";
                registrarPatientsDataGrid.Columns[1].Header = "ФИО";
                registrarPatientsDataGrid.Columns[2].Header = "Дата рождения";
                registrarPatientsDataGrid.Columns[3].Header = "Пол";
                registrarPatientsDataGrid.Columns[4].Header = "ОМС";

                registrarPatientsDataGrid.Columns[0].Visibility = Visibility.Collapsed;

                registrarPatientsDataGrid.Columns[0].Width = new DataGridLength(2, DataGridLengthUnitType.Auto);
                registrarPatientsDataGrid.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                registrarPatientsDataGrid.Columns[2].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                registrarPatientsDataGrid.Columns[3].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                registrarPatientsDataGrid.Columns[4].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            }
            //else
            //{
            //    registrarPatientsDataGrid.Columns[0].Visibility = Visibility.Collapsed;
            //    registrarPatientsDataGrid.Columns[1].Visibility = Visibility.Collapsed;
            //    registrarPatientsDataGrid.Columns[2].Visibility = Visibility.Collapsed;
            //    registrarPatientsDataGrid.Columns[3].Visibility = Visibility.Collapsed;
            //    registrarPatientsDataGrid.Columns[4].Visibility = Visibility.Collapsed;
            //}

            CheckSelection();

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetPatients();
            CheckSelection();
        }
        private void registrarPatientsMainPart_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (registrarPatientsMainPart.IsEnabled)
                GetPatients();
        }

        private void registrarPatientsAddPatient_Click(object sender, RoutedEventArgs e)
        {

            addMode = true;
            OpenPatientForm();
        }

        private void registrarPatientsAlterPatient_Click(object sender, RoutedEventArgs e)
        {

            selectedPatient = (Patient)registrarPatientsDataGrid.SelectedItem;
            //selectedName = selectedPatient.GetType().GetProperty("Name").GetValue(selectedPatient).ToString();
            registrarPatientsPatientFormFullName.Text = selectedPatient.fullName;
            registrarPatientsPatientFormBirthDate.Text = selectedPatient.birthDate;
            registrarPatientsPatientFormGender.Text = selectedPatient.gender;
            registrarPatientsPatientFormOMC.Text = selectedPatient.OMC;

            addMode = false;
            OpenPatientForm();

        }
        public void OpenPatientForm()
        {
            registrarPatientsPatientForm.Visibility = Visibility.Visible;
            registrarPatientsMainPart.IsEnabled = false;
        }
        public void ClosePatientForm()
        {
            registrarPatientsMainPart.IsEnabled = true;
            registrarPatientsPatientForm.Visibility = Visibility.Collapsed;
        }

        private void registrarPatientsPatientFormAccept_Click(object sender, RoutedEventArgs e)
        {
            if (addMode)
            {

                if (!string.IsNullOrEmpty(registrarPatientsPatientFormFullName.Text))
                {
                    Patient patient = new Patient();
                    patient.id = Guid.NewGuid().ToString();
                    patient.fullName = registrarPatientsPatientFormFullName.Text;
                    patient.birthDate = registrarPatientsPatientFormBirthDate.Text;
                    patient.gender = registrarPatientsPatientFormGender.Text;
                    patient.OMC = registrarPatientsPatientFormOMC.Text;

                    db.Patients.Add(patient);
                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все обязательные поля.");
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(registrarPatientsPatientFormFullName.Text))
                {
                        selectedPatient.fullName = registrarPatientsPatientFormFullName.Text;
                        selectedPatient.birthDate= registrarPatientsPatientFormBirthDate.Text;
                        selectedPatient.gender= registrarPatientsPatientFormGender.Text;
                        selectedPatient.OMC = registrarPatientsPatientFormOMC.Text;
                        db.Patients.AddOrUpdate(selectedPatient);
                        db.SaveChanges();

                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все обязательные поля.");
                }
            }
            ClosePatientForm();
        }
        private void registrarPatientsDeletePatient_Click(object sender, RoutedEventArgs e)
        {
            selectedPatient = (Patient)registrarPatientsDataGrid.SelectedItem;
            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите безвозвратно удалить пациента \"{selectedPatient.fullName}\"?\nЭто действие нельзя отменить.", "Удаление пациента", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                db.Patients.Attach(selectedPatient);
                db.Entry(selectedPatient).State = EntityState.Deleted;
                db.SaveChanges();
                GetPatients();
            }

        }

        private void registrarPatientsPatientFormCancel_Click(object sender, RoutedEventArgs e)
        {
            ClosePatientForm();
        }

        private void registrarPatientsDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

            CheckSelection();
            
        }

        private void registrarPatientsSearchFullName_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetPatients();
        }

        public void CheckSelection()
        {
            if (registrarPatientsDataGrid.SelectedItem == null)
            {
                registrarPatientsAlterPatient.IsEnabled = false;
                registrarPatientsDeletePatient.IsEnabled = false;
            }
            else
            {
                registrarPatientsAlterPatient.IsEnabled = true;
                registrarPatientsDeletePatient.IsEnabled = true;
            }
        }
    }
}
