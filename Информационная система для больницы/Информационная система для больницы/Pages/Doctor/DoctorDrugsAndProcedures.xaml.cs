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

namespace Информационная_система_для_больницы.Pages.Doctor
{
    /// <summary>
    /// Логика взаимодействия для DoctorDrugsAndProcedures.xaml
    /// </summary>
    public partial class DoctorDrugsAndProcedures : Page
    {
        Data.AppContext db;
        DrugProcedure selectedItem = null;
        public DoctorDrugsAndProcedures()
        {
            InitializeComponent();
            doctorDrugsAndProceduresSearchType.Items.Add("Всё");
            doctorDrugsAndProceduresSearchType.Items.Add("Препараты");
            doctorDrugsAndProceduresSearchType.Items.Add("Процедуры");
            doctorDrugsAndProceduresSearchType.SelectedIndex = 0;
        }

        public void GetItems()
        {
            db = new Data.AppContext();

            var drugsProcedures = from dp in db.DrugsProcedures
                                   where
                                   (doctorDrugsAndProceduresSearchType.SelectedIndex == 0 || doctorDrugsAndProceduresSearchType.Text == dp.@group) &&
                                   (string.IsNullOrEmpty(doctorDrugsAndProceduresSearchName.Text) || dp.name.ToLower().Contains(doctorDrugsAndProceduresSearchName.Text.ToLower()))
                                   select dp.name;

            doctorDrugsAndProceduresList.ItemsSource = drugsProcedures.ToList().OrderBy(x => x).ToList();
        }
      
        private void doctorDrugsAndProceduresSearchType_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetItems();
        }

        private void doctorDrugsAndProceduresSearchType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (doctorDrugsAndProceduresSearchType.SelectedValue != null)
                doctorDrugsAndProceduresSearchType.Text = doctorDrugsAndProceduresSearchType.SelectedValue.ToString();
            GetItems();
        }

        private void doctorDrugsAndProceduresList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(doctorDrugsAndProceduresList.SelectedIndex != -1)
            {
                var getItem = from dp in db.DrugsProcedures
                           where dp.name == doctorDrugsAndProceduresList.SelectedValue.ToString()
                           select dp.description;
                doctorDrugsAndProceduresDescription.Text = getItem.FirstOrDefault().ToString();
            }
            else
            {
                doctorDrugsAndProceduresDescription.Text = string.Empty;
            }
            
        }
    }
}
