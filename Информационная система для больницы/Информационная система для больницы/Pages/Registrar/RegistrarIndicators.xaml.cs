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
    /// Логика взаимодействия для RegistrarIndicators.xaml
    /// </summary>
    public partial class RegistrarIndicators : Page
    {
        Data.AppContext db;
        bool addMode = false;
        Indicator selectedIndicator = null;
        public RegistrarIndicators()
        {
            InitializeComponent();
        }
        public void GetIndicators()
        {
            db = new Data.AppContext();

            var indicators = from d in db.Indicators
                             select d;

            if (string.IsNullOrEmpty(registrarIndicatorsSearchName.Text))
                registrarIndicatorsDataGrid.ItemsSource = indicators.ToList();
            else
                registrarIndicatorsDataGrid.ItemsSource = indicators.ToList().Where(x => x.name.ToLower().Contains(registrarIndicatorsSearchName.Text.ToLower()));

            if (registrarIndicatorsDataGrid.Items.Count != 0)
            {
                registrarIndicatorsDataGrid.SelectedIndex = 0;

                registrarIndicatorsDataGrid.Columns[0].Header = "Идентификатор";
                registrarIndicatorsDataGrid.Columns[1].Header = "Название";
                registrarIndicatorsDataGrid.Columns[2].Header = "Описание";

                registrarIndicatorsDataGrid.Columns[0].Visibility = Visibility.Collapsed;

                registrarIndicatorsDataGrid.Columns[0].Width = new DataGridLength(2, DataGridLengthUnitType.Auto);
                registrarIndicatorsDataGrid.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                registrarIndicatorsDataGrid.Columns[2].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
            //else
            //{
            //    registrarIndicatorsDataGrid.Columns[0].Visibility = Visibility.Collapsed;
            //    registrarIndicatorsDataGrid.Columns[1].Visibility = Visibility.Collapsed;
            //    registrarIndicatorsDataGrid.Columns[2].Visibility = Visibility.Collapsed;
            //}
            CheckSelection();

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetIndicators();
            CheckSelection();
        }
        private void registrarIndicatorsMainPart_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            GetIndicators();
        }

        private void registrarIndicatorsAddIndicator_Click(object sender, RoutedEventArgs e)
        {
            registrarIndicatorsIndicatorFormName.Text = string.Empty;
            registrarIndicatorsIndicatorFormDescription.Text = string.Empty;

            addMode = true;
            OpenIndicatorForm();
        }

        private void registrarIndicatorsAlterIndicator_Click(object sender, RoutedEventArgs e)
        {

            selectedIndicator = (Indicator)registrarIndicatorsDataGrid.SelectedItem;
            registrarIndicatorsIndicatorFormName.Text = selectedIndicator.name;
            registrarIndicatorsIndicatorFormDescription.Text = selectedIndicator.description;

            addMode = false;
            OpenIndicatorForm();

        }
        public void OpenIndicatorForm()
        {
            registrarIndicatorsIndicatorForm.Visibility = Visibility.Visible;
            registrarIndicatorsMainPart.IsEnabled = false;
        }
        public void CloseIndicatorForm()
        {
            registrarIndicatorsMainPart.IsEnabled = true;
            registrarIndicatorsIndicatorForm.Visibility = Visibility.Collapsed;
        }

        private void registrarIndicatorsIndicatorFormAccept_Click(object sender, RoutedEventArgs e)
        {
            if (addMode)
            {
                if (!string.IsNullOrEmpty(registrarIndicatorsIndicatorFormName.Text))
                {
                    Indicator indicator = new Indicator();
                    indicator.id = Guid.NewGuid().ToString();
                    indicator.name = registrarIndicatorsIndicatorFormName.Text;
                    indicator.description = registrarIndicatorsIndicatorFormDescription.Text;

                    db.Indicators.Add(indicator);
                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все обязательные поля.");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(registrarIndicatorsIndicatorFormName.Text))
                {
                    selectedIndicator.name = registrarIndicatorsIndicatorFormName.Text;
                    selectedIndicator.description = registrarIndicatorsIndicatorFormDescription.Text;

                    db.Indicators.AddOrUpdate(selectedIndicator);
                    db.SaveChanges();

                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все обязательные поля.");
                }
            }
            CloseIndicatorForm();
        }
        private void registrarIndicatorsDeleteIndicator_Click(object sender, RoutedEventArgs e)
        {
            selectedIndicator = (Indicator)registrarIndicatorsDataGrid.SelectedItem;
            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите безвозвратно удалить показатель \"{selectedIndicator.name}\"?\nЭто действие нельзя отменить.", "Удаление показателя", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                using (var transation = db.Database.BeginTransaction())
                {
                    db.Indicators.Attach(selectedIndicator);
                db.Entry(selectedIndicator).State = EntityState.Deleted;

                foreach (var ci in db.CollectingIndicators.Where(x => x.indicatorId == selectedIndicator.id).ToList())
                {
                    db.CollectingIndicators.Attach(ci);
                    db.Entry(ci).State = EntityState.Deleted;

                    foreach (var pc in db.PatientConditions.Where(x => x.collectingIndicatorId == ci.id))
                    {
                        db.PatientConditions.Attach(pc);
                        db.Entry(pc).State = EntityState.Deleted;
                    }
                }
                    db.SaveChanges();
                    transation.Commit();
                    GetIndicators();
                }
            }
        }

        private void registrarIndicatorsIndicatorFormCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseIndicatorForm();
        }

        private void registrarIndicatorsDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (registrarIndicatorsMainPart.IsEnabled)
                CheckSelection();
        }

        private void registrarIndicatorsSearchFullName_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetIndicators();
        }

        public void CheckSelection()
        {
            if (registrarIndicatorsDataGrid.SelectedItem == null)
            {
                registrarIndicatorsAlterIndicator.IsEnabled = false;
                registrarIndicatorsDeleteIndicator.IsEnabled = false;
            }
            else
            {
                registrarIndicatorsAlterIndicator.IsEnabled = true;
                registrarIndicatorsDeleteIndicator.IsEnabled = true;
            }
        }

    }
}
