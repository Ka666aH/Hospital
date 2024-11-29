using System;
using System.Collections.Generic;
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

namespace Информационная_система_для_больницы.Pages.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminEmployees.xaml
    /// </summary>
    public partial class AdminEmployees : Page
    {
        Data.AppContext db;
        bool passChange = false;
        string selectedName = null;
        string selectedAccess = null;
        string selectedPassword = null;
        public AdminEmployees()
        {
            InitializeComponent();
        }
        public void GetEmployees()
        {
            db = new Data.AppContext();

            var employess = from em in db.Employees
                                //where (em.fullName.ToLower().Contains(adminEmloyeesSearchFullName.Text.ToLower()) || string.IsNullOrEmpty(adminEmloyeesSearchFullName.Text)) && (em.access == adminEmloyeesSearchAccess.Text || string.IsNullOrEmpty(adminEmloyeesSearchAccess.Text))
                            select new
                            {
                                Name = em.fullName,
                                Password = em.password,
                                Access = em.access
                            };

            if (string.IsNullOrEmpty(adminEmloyeesSearchFullName.Text) && string.IsNullOrEmpty(adminEmloyeesSearchAccess.Text))
                adminEmployeesDataGrid.ItemsSource = employess.ToList();
            if (string.IsNullOrEmpty(adminEmloyeesSearchFullName.Text) && !string.IsNullOrEmpty(adminEmloyeesSearchAccess.Text))
                adminEmployeesDataGrid.ItemsSource = employess.ToList().Where(x => x.Access == adminEmloyeesSearchAccess.Text);
            if (!string.IsNullOrEmpty(adminEmloyeesSearchFullName.Text) && string.IsNullOrEmpty(adminEmloyeesSearchAccess.Text))
                adminEmployeesDataGrid.ItemsSource = employess.ToList().Where(x => x.Name.ToLower().Contains(adminEmloyeesSearchFullName.Text.ToLower()));
            if (!string.IsNullOrEmpty(adminEmloyeesSearchFullName.Text) && !string.IsNullOrEmpty(adminEmloyeesSearchAccess.Text))
                adminEmployeesDataGrid.ItemsSource = employess.ToList().Where(x => x.Name.ToLower().Contains(adminEmloyeesSearchFullName.Text.ToLower()) && x.Access == adminEmloyeesSearchAccess.Text);


            if (adminEmployeesDataGrid.Items.Count != 0)
            {
                adminEmployeesDataGrid.Columns[0].Header = "ФИО";
                adminEmployeesDataGrid.Columns[1].Header = "Пароль";
                adminEmployeesDataGrid.Columns[2].Header = "Должность";
                adminEmployeesDataGrid.Columns[0].Width = new DataGridLength(2, DataGridLengthUnitType.Star);
                adminEmployeesDataGrid.Columns[1].Width = new DataGridLength(2, DataGridLengthUnitType.Auto);
                adminEmployeesDataGrid.Columns[2].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                adminEmployeesDataGrid.SelectedIndex = 0;
            }

        }




        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetEmployees();
        }

        private void adminEmployeesAlterEmloyeePassword_Click(object sender, RoutedEventArgs e)
        {

            adminEmloyeesEmployeeFormPassword.Focus();
            OpenEmployeeForm();
        }

        private void adminEmployeesAlterEmloyeeAccess_Click(object sender, RoutedEventArgs e)
        {
            adminEmloyeesEmployeeFormAccess.Focus();
            OpenEmployeeForm();

        }

        private void adminEmployeesEmployeeFormAccept_Click(object sender, RoutedEventArgs e)
        {
            if(adminEmloyeesEmployeeFormPassword.Text != selectedPassword || adminEmloyeesEmployeeFormAccess.Text != selectedAccess)
            {
                var q = from em in db.Employees
                        where em.access == selectedAccess && em.fullName == selectedName
                        select em;

                Employee employee = q.FirstOrDefault();

                if(!string.IsNullOrEmpty(adminEmloyeesEmployeeFormPassword.Text))
                {
                    employee.password = adminEmloyeesEmployeeFormPassword.Text;
                    employee.access= adminEmloyeesEmployeeFormAccess.Text;
                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Невозможно установить пустой пароль.");
                }
            }

            CloseEmployeeForm();
        }

        private void adminEmployeesEmployeeFormCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseEmployeeForm();
        }
        public void OpenEmployeeForm()
        {
            var selectedEmployee = adminEmployeesDataGrid.SelectedItem;
            selectedName = selectedEmployee.GetType().GetProperty("Name").GetValue(selectedEmployee).ToString();
            selectedAccess = selectedEmployee.GetType().GetProperty("Access").GetValue(selectedEmployee).ToString();
            selectedPassword = selectedEmployee.GetType().GetProperty("Password").GetValue(selectedEmployee).ToString();

            adminEmloyeesEmployeeFormFullName.Text = selectedName;
            adminEmloyeesEmployeeFormAccess.Text = selectedAccess;
            adminEmloyeesEmployeeFormPassword.Text = selectedPassword;

            adminEmployeesEmployeeForm.Visibility = Visibility.Visible;
            adminEmployeesMainPart.IsEnabled = false;
        }
        public void CloseEmployeeForm()
        {
            adminEmployeesMainPart.IsEnabled = true;
            adminEmployeesEmployeeForm.Visibility = Visibility.Collapsed;
        }

        private void adminEmployeesMainPart_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (adminEmployeesMainPart.IsEnabled)
                GetEmployees();
        }

        private void adminEmployeesDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (adminEmployeesDataGrid.SelectedItem == null)
            {
                adminEmployeesAlterEmloyeePassword.IsEnabled = false;
                adminEmployeesAlterEmloyeeAccess.IsEnabled = false;
                
            }
            else
            {
                adminEmployeesAlterEmloyeePassword.IsEnabled = true;
                adminEmployeesAlterEmloyeeAccess.IsEnabled = true;
            }
        }

        private void adminEmloyeesSearchFullName_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetEmployees();
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            ComboBoxItem item = sender as ComboBoxItem;
            adminEmloyeesSearchAccess.Text = item.Content.ToString();
            GetEmployees();
        }
    }
}
