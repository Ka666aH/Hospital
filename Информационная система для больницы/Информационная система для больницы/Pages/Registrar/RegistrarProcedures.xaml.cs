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
    /// Логика взаимодействия для RegistrarProcedures.xaml
    /// </summary>
    public partial class RegistrarProcedures : Page
    {
        Data.AppContext db;
        bool addMode = false;
        string group = "Процедуры";
        DrugProcedure selectedProcedure = null;
        public RegistrarProcedures()
        {
            InitializeComponent();
        }
        public void GetProcedures()
        {
            db = new Data.AppContext();

            var procedures = from d in db.DrugsProcedures
                        select d;

            if (string.IsNullOrEmpty(registrarProceduresSearchName.Text))
                registrarProceduresDataGrid.ItemsSource = procedures.ToList().Where(x => x.group == group);
            else
                registrarProceduresDataGrid.ItemsSource = procedures.ToList().Where(x => x.group == group && x.name.ToLower().Contains(registrarProceduresSearchName.Text.ToLower()));

            if (registrarProceduresDataGrid.Items.Count != 0)
            {
                registrarProceduresDataGrid.SelectedIndex = 0;

                registrarProceduresDataGrid.Columns[0].Header = "Идентификатор";
                registrarProceduresDataGrid.Columns[1].Header = "Группа";
                registrarProceduresDataGrid.Columns[2].Header = "Название";
                registrarProceduresDataGrid.Columns[3].Header = "Описание";

                registrarProceduresDataGrid.Columns[0].Visibility = Visibility.Collapsed;
                registrarProceduresDataGrid.Columns[1].Visibility = Visibility.Collapsed;

                registrarProceduresDataGrid.Columns[0].Width = new DataGridLength(2, DataGridLengthUnitType.Auto);
                registrarProceduresDataGrid.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                registrarProceduresDataGrid.Columns[2].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                registrarProceduresDataGrid.Columns[3].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
            CheckSelection();

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetProcedures();
            CheckSelection();
        }
        private void registrarProceduresMainPart_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (registrarProceduresMainPart.IsEnabled)
                GetProcedures();
        }

        private void registrarProceduresAddProcedure_Click(object sender, RoutedEventArgs e)
        {

            addMode = true;
            OpenProcedureForm();
        }

        private void registrarProceduresAlterProcedure_Click(object sender, RoutedEventArgs e)
        {

            selectedProcedure = (DrugProcedure)registrarProceduresDataGrid.SelectedItem;
            registrarProceduresProcedureFormName.Text = selectedProcedure.name;
            registrarProceduresProcedureFormDescription.Text = selectedProcedure.description;

            addMode = false;
            OpenProcedureForm();

        }
        public void OpenProcedureForm()
        {
            registrarProceduresProcedureForm.Visibility = Visibility.Visible;
            registrarProceduresMainPart.IsEnabled = false;
        }
        public void CloseProcedureForm()
        {
            registrarProceduresMainPart.IsEnabled = true;
            registrarProceduresProcedureForm.Visibility = Visibility.Collapsed;
        }

        private void registrarProceduresProcedureFormAccept_Click(object sender, RoutedEventArgs e)
        {
            if (addMode)
            {
                if (!string.IsNullOrEmpty(registrarProceduresProcedureFormName.Text))
                {
                    DrugProcedure procedure = new DrugProcedure();
                    procedure.id = Guid.NewGuid().ToString();
                    procedure.group = group;
                    procedure.name = registrarProceduresProcedureFormName.Text;
                    procedure.description = registrarProceduresProcedureFormDescription.Text;

                    db.DrugsProcedures.Add(procedure);
                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все обязательные поля.");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(registrarProceduresProcedureFormName.Text))
                {
                    selectedProcedure.name = registrarProceduresProcedureFormName.Text;
                    selectedProcedure.description = registrarProceduresProcedureFormDescription.Text;

                    db.DrugsProcedures.AddOrUpdate(selectedProcedure);
                    db.SaveChanges();

                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все обязательные поля.");
                }
            }
            CloseProcedureForm();
        }
        private void registrarProceduresDeleteProcedure_Click(object sender, RoutedEventArgs e)
        {
            selectedProcedure = (DrugProcedure)registrarProceduresDataGrid.SelectedItem;
            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите безвозвратно удалить процедуру \"{selectedProcedure.name}\"?\nЭто действие нельзя отменить.", "Удаление процедуры", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                db.DrugsProcedures.Attach(selectedProcedure);
                db.Entry(selectedProcedure).State = EntityState.Deleted;
                db.SaveChanges();
                GetProcedures();
            }
        }

        private void registrarProceduresProcedureFormCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseProcedureForm();
        }

        private void registrarProceduresDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            CheckSelection();
        }

        private void registrarProceduresSearchFullName_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetProcedures();
        }

        public void CheckSelection()
        {
            if (registrarProceduresDataGrid.SelectedItem == null)
            {
                registrarProceduresAlterProcedure.IsEnabled = false;
                registrarProceduresDeleteProcedure.IsEnabled = false;
            }
            else
            {
                registrarProceduresAlterProcedure.IsEnabled = true;
                registrarProceduresDeleteProcedure.IsEnabled = true;
            }
        }

    }
}
