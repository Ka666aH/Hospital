using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Логика взаимодействия для RegistrarEmployees.xaml
    /// </summary>
    public partial class RegistrarEmployees : Page
    {
        Data.AppContext db;
        bool addMode = false;
        string selectedName = null;
        string selectedAccess = null;
        public RegistrarEmployees()
        {

            InitializeComponent();
        }

        public void GetEmployees()
        {
            db = new Data.AppContext();

            var _employess = db.Employees.ToList();


            var employess = from em in _employess
                                //where em.access != "Администратор"
                            where (string.IsNullOrEmpty(registrarEmloyeesSearchFullName.Text) || em.fullName.ToLower().Contains(registrarEmloyeesSearchFullName.Text.ToLower()))&&(string.IsNullOrEmpty(registrarEmloyeesSearchAccess.Text) || em.access.ToLower().Contains(registrarEmloyeesSearchAccess.Text.ToLower()))
                            select new
                            {
                                Name = em.fullName,
                                Access = em.access
                            };

            if(string.IsNullOrEmpty(registrarEmloyeesSearchFullName.Text) && string.IsNullOrEmpty(registrarEmloyeesSearchAccess.Text))
                registrarEmployeesDataGrid.ItemsSource = employess.ToList();
            if (string.IsNullOrEmpty(registrarEmloyeesSearchFullName.Text) && !string.IsNullOrEmpty(registrarEmloyeesSearchAccess.Text))
                registrarEmployeesDataGrid.ItemsSource = employess.ToList().Where(x => x.Access == registrarEmloyeesSearchAccess.Text);
            if (!string.IsNullOrEmpty(registrarEmloyeesSearchFullName.Text) && string.IsNullOrEmpty(registrarEmloyeesSearchAccess.Text))
                registrarEmployeesDataGrid.ItemsSource = employess.ToList().Where(x => x.Name.ToLower().Contains(registrarEmloyeesSearchFullName.Text.ToLower()));
            if (!string.IsNullOrEmpty(registrarEmloyeesSearchFullName.Text) && !string.IsNullOrEmpty(registrarEmloyeesSearchAccess.Text))
                registrarEmployeesDataGrid.ItemsSource = employess.ToList().Where(x => x.Name.ToLower().Contains(registrarEmloyeesSearchFullName.Text.ToLower()) && x.Access == registrarEmloyeesSearchAccess.Text);


            if(registrarEmployeesDataGrid.Items.Count != 0)
            {
                registrarEmployeesDataGrid.Columns[0].Header = "ФИО";
                registrarEmployeesDataGrid.Columns[1].Header = "Должность";
                registrarEmployeesDataGrid.Columns[0].Width = new DataGridLength(2, DataGridLengthUnitType.Star);
                registrarEmployeesDataGrid.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                registrarEmployeesDataGrid.SelectedItem = 0;
            }
            
        }




        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetEmployees();
        }

        private void registrarEmployeesAddEmloyee_Click(object sender, RoutedEventArgs e)
        {

            registrarEmloyeesEmployeeFormAccess.Text = "Нет";
            addMode = true;
            OpenEmployeeForm();
        }

        private void registrarEmployeesAlterEmloyee_Click(object sender, RoutedEventArgs e)
        {

            var selectedEmployee = registrarEmployeesDataGrid.SelectedItem;
            selectedName = selectedEmployee.GetType().GetProperty("Name").GetValue(selectedEmployee).ToString();
            selectedAccess = selectedEmployee.GetType().GetProperty("Access").GetValue(selectedEmployee).ToString();

            registrarEmloyeesEmployeeFormFullName.Text = selectedName;
            registrarEmloyeesEmployeeFormAccess.Text = selectedAccess;

            addMode = false;
            OpenEmployeeForm();

        }

        private void registrarEmployeesEmployeeFormAccept_Click(object sender, RoutedEventArgs e)
        {
            if(addMode)
            {

                if(!string.IsNullOrEmpty(registrarEmloyeesEmployeeFormFullName.Text) && !string.IsNullOrEmpty(registrarEmloyeesEmployeeFormAccess.Text))
                {
                        Employee employee = new Employee();
                        employee.id = Guid.NewGuid().ToString();
                        employee.fullName = registrarEmloyeesEmployeeFormFullName.Text;
                        employee.access = "Нет";
                        employee.password = "1";
                    

                    db.Employees.Add(employee);
                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все поля.");
                }

            }
            else
            {
                
                var q = from em in db.Employees
                        where em.access == selectedAccess && em.fullName == selectedName
                        select em;

                if (!string.IsNullOrEmpty(registrarEmloyeesEmployeeFormFullName.Text) && !string.IsNullOrEmpty(registrarEmloyeesEmployeeFormAccess.Text))
                {
                    
                            Employee employee = q.FirstOrDefault();

                            if (registrarEmloyeesEmployeeFormFullName.Text != selectedName)
                            {
                                employee.fullName = registrarEmloyeesEmployeeFormFullName.Text;
                                db.SaveChanges();
                            }

                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все поля.");
                }
            }
            CloseEmployeeForm();
        }

        private void registrarEmployeesEmployeeFormCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseEmployeeForm();
        }
        public void OpenEmployeeForm()
        {
            registrarEmployeesEmployeeForm.Visibility = Visibility.Visible;
            registrarEmployeesMainPart.IsEnabled = false;
        }
        public void CloseEmployeeForm()
        {
            registrarEmployeesMainPart.IsEnabled = true;
            registrarEmployeesEmployeeForm.Visibility = Visibility.Collapsed;
        }

        private void registrarEmployeesMainPart_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (registrarEmployeesMainPart.IsEnabled)
                GetEmployees();
        }

        private void registrarEmployeesDeleteEmloyee_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = registrarEmployeesDataGrid.SelectedItem;
            selectedName = selectedEmployee.GetType().GetProperty("Name").GetValue(selectedEmployee).ToString();
            selectedAccess = selectedEmployee.GetType().GetProperty("Access").GetValue(selectedEmployee).ToString();
            if(!((selectedAccess == "Администратор" && db.Employees.Where(x => x.access == "Администратор").Count()<2)||((selectedAccess == "Регистратор" && db.Employees.Where(x => x.access == "Регистратор").Count() < 2))))
            {
            var q = from em in db.Employees
                    where em.access == selectedAccess && em.fullName == selectedName
                    select em;
            Employee employee = q.FirstOrDefault();

            
            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите безвозвратно удалить сотрудника \"{selectedName}\"?\nЭто действие нельзя отменить.", "Удаление сотрудника", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
            {
                db.Employees.Attach(employee);
                db.Entry(employee).State = EntityState.Deleted;
                db.SaveChanges();
                GetEmployees();
            }

            }
            else
            {
                MessageBox.Show("Невозможно удалить последнего сотрудника с этим уровнем доступа.");
            }


        }

        private void registrarEmployeesDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if(registrarEmployeesDataGrid.SelectedItem == null)
            {
                registrarEmployeesAlterEmloyee.IsEnabled = false;
                registrarEmployeesDeleteEmloyee.IsEnabled = false;
            }
            else
            {
                registrarEmployeesAlterEmloyee.IsEnabled = true;
                registrarEmployeesDeleteEmloyee.IsEnabled = true;
            }
        }

        private void registrarEmloyeesSearchFullName_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetEmployees();
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            ComboBoxItem item  = sender as ComboBoxItem;
            registrarEmloyeesSearchAccess.Text = item.Content.ToString();
            GetEmployees();
        }
    }
}
