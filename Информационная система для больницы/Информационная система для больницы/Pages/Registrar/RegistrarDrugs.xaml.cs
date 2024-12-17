using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity;
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
    /// Логика взаимодействия для RegistrarDrugs.xaml
    /// </summary>
    public partial class RegistrarDrugs : Page
    {
        Data.AppContext db;
        bool addMode = false;
        string group = "Препараты";
        DrugProcedure selectedDrug = null;
        public RegistrarDrugs()
        {
            InitializeComponent();
        }
        public void GetDrugs()
        {
            db = new Data.AppContext();

            var drugs = from d in db.DrugsProcedures
                        select d;

            if (string.IsNullOrEmpty(registrarDrugsSearchName.Text))
                registrarDrugsDataGrid.ItemsSource = drugs.ToList().Where(x => x.group == "Препараты");
            else
                registrarDrugsDataGrid.ItemsSource = drugs.ToList().Where(x => x.group == "Препараты" && x.name.ToLower().Contains(registrarDrugsSearchName.Text.ToLower()));

            if (registrarDrugsDataGrid.Items.Count != 0)
            {
                registrarDrugsDataGrid.SelectedIndex = 0;

            registrarDrugsDataGrid.Columns[0].Header = "Идентификатор";
            registrarDrugsDataGrid.Columns[1].Header = "Группа";
            registrarDrugsDataGrid.Columns[2].Header = "Название";
            registrarDrugsDataGrid.Columns[3].Header = "Описание";

            registrarDrugsDataGrid.Columns[0].Visibility = Visibility.Collapsed;
            registrarDrugsDataGrid.Columns[1].Visibility = Visibility.Collapsed;

            registrarDrugsDataGrid.Columns[0].Width = new DataGridLength(2, DataGridLengthUnitType.Auto);
            registrarDrugsDataGrid.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            registrarDrugsDataGrid.Columns[2].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            registrarDrugsDataGrid.Columns[3].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
            CheckSelection();

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetDrugs();
            CheckSelection();
        }
        private void registrarDrugsMainPart_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (registrarDrugsMainPart.IsEnabled)
                GetDrugs();
        }

        private void registrarDrugsAddDrug_Click(object sender, RoutedEventArgs e)
        {
            registrarDrugsDrugFormName.Text = string.Empty;
            registrarDrugsDrugFormDescription.Text = string.Empty;

            addMode = true;
            OpenDrugForm();
        }

        private void registrarDrugsAlterDrug_Click(object sender, RoutedEventArgs e)
        {

            selectedDrug = (DrugProcedure)registrarDrugsDataGrid.SelectedItem;
            registrarDrugsDrugFormName.Text = selectedDrug.name;
            registrarDrugsDrugFormDescription.Text = selectedDrug.description;

            addMode = false;
            OpenDrugForm();

        }
        public void OpenDrugForm()
        {
            registrarDrugsDrugForm.Visibility = Visibility.Visible;
            registrarDrugsMainPart.IsEnabled = false;
        }
        public void CloseDrugForm()
        {
            registrarDrugsMainPart.IsEnabled = true;
            registrarDrugsDrugForm.Visibility = Visibility.Collapsed;
        }

        private void registrarDrugsDrugFormAccept_Click(object sender, RoutedEventArgs e)
        {
            if (addMode)
            {
                if (!string.IsNullOrEmpty(registrarDrugsDrugFormName.Text))
                {
                    DrugProcedure drug = new DrugProcedure();
                    drug.id = Guid.NewGuid().ToString();
                    drug.group = group;
                    drug.name = registrarDrugsDrugFormName.Text;
                    drug.description = registrarDrugsDrugFormDescription.Text;

                    db.DrugsProcedures.Add(drug);
                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все обязательные поля.");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(registrarDrugsDrugFormName.Text))
                {
                    selectedDrug.name = registrarDrugsDrugFormName.Text;
                    selectedDrug.description = registrarDrugsDrugFormDescription.Text;

                    db.DrugsProcedures.AddOrUpdate(selectedDrug);
                    db.SaveChanges();

                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все обязательные поля.");
                }
            }
            CloseDrugForm();
        }
        private void registrarDrugsDeleteDrug_Click(object sender, RoutedEventArgs e)
        {
            selectedDrug = (DrugProcedure)registrarDrugsDataGrid.SelectedItem;
            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите безвозвратно удалить препарат \"{selectedDrug.name}\"?\nЭто действие нельзя отменить.", "Удаление препарата", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                db.DrugsProcedures.Attach(selectedDrug);
                db.Entry(selectedDrug).State = EntityState.Deleted;
                db.SaveChanges();
                GetDrugs();
            }
        }

        private void registrarDrugsDrugFormCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseDrugForm();
        }

        private void registrarDrugsDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            CheckSelection();
        }

        private void registrarDrugsSearchFullName_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetDrugs();
        }

        public void CheckSelection()
        {
            if (registrarDrugsDataGrid.SelectedItem == null)
            {
                registrarDrugsAlterDrug.IsEnabled = false;
                registrarDrugsDeleteDrug.IsEnabled = false;
            }
            else
            {
                registrarDrugsAlterDrug.IsEnabled = true;
                registrarDrugsDeleteDrug.IsEnabled = true;
            }
        }
    }
}
