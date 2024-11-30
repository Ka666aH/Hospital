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
    /// Логика взаимодействия для RegistrarWards.xaml
    /// </summary>
    public partial class RegistrarWards : Page
    {
        Data.AppContext db;
        bool addMode = false;
        Bed selectedBed  =  null;
        public RegistrarWards()
        {
            InitializeComponent();
        }

        public void GetBeds()
        {
            db = new Data.AppContext();

            var beds = from b in db.Beds
                            select b;

            if (string.IsNullOrEmpty(registrarBedsSearchWard.Text) && string.IsNullOrEmpty(registrarBedsSearchType.Text))
                registrarBedsDataGrid.ItemsSource = beds.ToList();
            if (string.IsNullOrEmpty(registrarBedsSearchWard.Text) && !string.IsNullOrEmpty(registrarBedsSearchType.Text))
                registrarBedsDataGrid.ItemsSource = beds.ToList().Where(x => x.type.ToLower().Contains(registrarBedsSearchType.Text.ToLower()));
            if (!string.IsNullOrEmpty(registrarBedsSearchWard.Text) && string.IsNullOrEmpty(registrarBedsSearchType.Text))
                registrarBedsDataGrid.ItemsSource = beds.ToList().Where(x => x.ward.ToLower().Contains(registrarBedsSearchWard.Text.ToLower()));
            if (!string.IsNullOrEmpty(registrarBedsSearchWard.Text) && !string.IsNullOrEmpty(registrarBedsSearchType.Text))
                registrarBedsDataGrid.ItemsSource = beds.ToList().Where(x => x.ward.ToLower().Contains(registrarBedsSearchWard.Text.ToLower()) && x.type.ToLower().Contains(registrarBedsSearchType.Text.ToLower()));


            if (registrarBedsDataGrid.Items.Count != 0)
            {
                registrarBedsDataGrid.Columns[0].Header = "ID";
                registrarBedsDataGrid.Columns[1].Header = "Номер палаты";
                registrarBedsDataGrid.Columns[2].Header = "Номер кровати";
                registrarBedsDataGrid.Columns[3].Header = "Тип кровати";

                registrarBedsDataGrid.Columns[0].Visibility = Visibility.Collapsed;

                

                registrarBedsDataGrid.Columns[0].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                registrarBedsDataGrid.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                registrarBedsDataGrid.Columns[2].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                registrarBedsDataGrid.Columns[3].Width = new DataGridLength(1, DataGridLengthUnitType.Star);

                registrarBedsDataGrid.SelectedIndex = 0;
            }

            registrarBedsSearchWard.ItemsSource = GetWards().Distinct();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetBeds();
        }

        private void registrarBedsAddBed_Click(object sender, RoutedEventArgs e)
        {
            registrarBedsBedFormBed.Text = "";
            registrarBedsBedFormWard.Text = "";
            registrarBedsBedFormType.Text = "";

            addMode = true;
            OpenBedForm();
        }

        private void registrarBedsAlterBed_Click(object sender, RoutedEventArgs e)
        {

            registrarBedsBedFormBed.Text = selectedBed.bed;
            registrarBedsBedFormWard.Text = selectedBed.ward;
            registrarBedsBedFormType.Text = selectedBed.type;

            addMode = false;
            OpenBedForm();

        }

        private void registrarBedsBedFormAccept_Click(object sender, RoutedEventArgs e)
        {
            if (addMode)
            {

                if (!string.IsNullOrEmpty(registrarBedsBedFormBed.Text) && !string.IsNullOrEmpty(registrarBedsBedFormWard.Text))
                {
                    Bed bed = new Bed();
                    bed.id = Guid.NewGuid().ToString();
                    bed.bed  = registrarBedsBedFormBed.Text;
                    bed.ward = registrarBedsBedFormWard.Text;
                    bed.type  = registrarBedsBedFormType.Text;

                    var q = from b in db.Beds
                            select b;
                    if (q.ToList().Where(x => x.ward == bed.ward && x.bed == bed.bed).Count() == 0)
                    {
                        db.Beds.Add(bed);
                        db.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Такая кровать уже существует. Задайте ей другой номер или палату.");
                    }
                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все обязательные поля.");
                }

            }
            else
            {

                if (!string.IsNullOrEmpty(registrarBedsBedFormBed.Text) && !string.IsNullOrEmpty(registrarBedsBedFormWard.Text))
                {

                    selectedBed.bed = registrarBedsBedFormBed.Text;
                    selectedBed.ward = registrarBedsBedFormWard.Text;
                    selectedBed.type = registrarBedsBedFormType.Text;
                    db.Beds.AddOrUpdate(selectedBed);
                        db.SaveChanges();

                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все обязательные поля.");
                }
            }
            CloseBedForm();
        }

        private void registrarBedsBedFormCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseBedForm();
        }
        public void OpenBedForm()
        {
            registrarBedsBedForm.Visibility = Visibility.Visible;
            registrarBedsMainPart.IsEnabled = false;

            registrarBedsBedFormWard.ItemsSource = GetWards().Distinct();
        }
        public void CloseBedForm()
        {
            registrarBedsMainPart.IsEnabled = true;
            registrarBedsBedForm.Visibility = Visibility.Collapsed;
        }

        private void registrarBedsMainPart_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (registrarBedsMainPart.IsEnabled)
            GetBeds();
        }

        private void registrarBedsDeleteBed_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите безвозвратно удалить кровать \"{selectedBed.bed}\" из палаты \"{selectedBed.ward}\"?\nЭто действие нельзя отменить.", "Удаление кровати", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                db.Beds.Attach(selectedBed);
                db.Entry(selectedBed).State = EntityState.Deleted;
                db.SaveChanges();
                GetBeds();
            }

        }

        private void registrarBedsDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
                selectedBed = (Bed)registrarBedsDataGrid.SelectedItem;

            if (registrarBedsDataGrid.SelectedItem == null)
            {
                registrarBedsAlterBed.IsEnabled = false;
                registrarBedsDeleteBed.IsEnabled = false;
            }
            else
            {
                //MessageBox.Show($"{selectedBed.id} {selectedBed.ward} {selectedBed.bed} {selectedBed.type}");
                registrarBedsAlterBed.IsEnabled = true;
                registrarBedsDeleteBed.IsEnabled = true;
            }
        }

        private void registrarBedsSearchType_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetBeds();
        }

        private void registrarBedsSearchWard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(registrarBedsSearchWard.SelectedValue != null)
                registrarBedsSearchWard.Text = registrarBedsSearchWard.SelectedValue.ToString();
            else
                registrarBedsSearchWard.Text = string.Empty;
            GetBeds();
        }

        public List<string> GetWards()
        {
            var wards = from b in db.Beds
                        select b.ward;

            List<string> wardList = new List<string>{string.Empty};

            foreach (var item in wards)
            {
                wardList.Add(item.ToString());
            }

            return wardList;
        }
    }
}
