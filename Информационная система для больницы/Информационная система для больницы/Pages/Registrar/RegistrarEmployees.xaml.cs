using System;
using System.Collections.Generic;
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
        string fired = "926BA4B4-4704-48DD-B286-CB2313B90368";
        public RegistrarEmployees()
        {

            InitializeComponent();
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            db = new Data.AppContext();

            var employeeData = from emp in db.Employees
                               join empStatus in db.EmployeesStatuses on emp.id equals empStatus.employeeId into empStatusGroup
                               from empStatus in empStatusGroup.DefaultIfEmpty() // Левое соединение
                               join status in db.Statuses on empStatus.statusId equals status.id into statusGroup 
                               from status in statusGroup.DefaultIfEmpty() // Левое соединение
                               where status.id != fired && empStatus.end == null                                                    //Переделать запрос, дкмаю нужен ещё один. из одного сотрудники из другого последний статус
                               select new
                               {
                                   Name = emp.fullName,
                                   Access = emp.access,
                                   Status = status != null ? status.statusName : null,
                                   Start = empStatus != null ? empStatus.start : null,
                                   End = empStatus != null ? empStatus.end : null
                               };

            registrarEmployeesDataGrid.ItemsSource = employeeData.ToList();
            registrarEmployeesDataGrid.Columns[0].Header = "ФИО";
            registrarEmployeesDataGrid.Columns[1].Header = "Должность";
            registrarEmployeesDataGrid.Columns[2].Header = "Статус";
            registrarEmployeesDataGrid.Columns[3].Header = "Дата начала статуса";
            registrarEmployeesDataGrid.Columns[4].Header = "Дата окончания статуса";



            //var q = from s in db.Statuses
            //        where s.@group == "employee"
            //        select s.statusName;

            //List<string> statuses = q.ToList();

            //List < EmployeeViewModel > employeesVM = new List<EmployeeViewModel>();
            //foreach (var item in employeeData)
            //{
            //    employeesVM.Add(new EmployeeViewModel(item.Name,item.Access,item.Status,statuses,item.Start,item.End));
            //}


            //foreach (var item in employeeData)
            //{
            //    MessageBox.Show(item.Name);
            //    MessageBox.Show(item.Access.ToString());
            //    MessageBox.Show(item.Status);
            //    MessageBox.Show(item.Start);
            //    MessageBox.Show(item.End);
            //}


        }

        private void registrarEmployeesAddEmloyee_Click(object sender, RoutedEventArgs e)
        {
            OpenEmployeeForm();

            registrarEmloyeesEmployeeFormStatusLabel.Visibility = Visibility.Collapsed;
            registrarEmloyeesEmployeeFormStatusStartLabel.Visibility = Visibility.Collapsed;
            registrarEmloyeesEmployeeFormStatusEndLabel.Visibility = Visibility.Collapsed;
            registrarEmloyeesEmployeeFormStatus.Visibility = Visibility.Collapsed;
            registrarEmloyeesEmployeeFormStatusStart.Visibility = Visibility.Collapsed;
            registrarEmloyeesEmployeeFormStatusEnd.Visibility = Visibility.Collapsed;

            registrarEmployeesEmployeeForm.Height = 140;

            addMode = true;
        }

        private void registrarEmployeesAlterEmloyee_Click(object sender, RoutedEventArgs e)
        {
            OpenEmployeeForm();

            registrarEmloyeesEmployeeFormStatusLabel.Visibility = Visibility.Visible;
            registrarEmloyeesEmployeeFormStatusStartLabel.Visibility = Visibility.Visible;
            registrarEmloyeesEmployeeFormStatusEndLabel.Visibility = Visibility.Visible;
            registrarEmloyeesEmployeeFormStatus.Visibility = Visibility.Visible;
            registrarEmloyeesEmployeeFormStatusStart.Visibility = Visibility.Visible;
            registrarEmloyeesEmployeeFormStatusEnd.Visibility = Visibility.Visible;

            registrarEmployeesEmployeeForm.Height = 250;

            addMode = false;

            //Если статус есть в таблице то registrarEmloyeesEmployeeFormStatus ридонли
        }

        private void registrarEmployeesEmployeeFormAccept_Click(object sender, RoutedEventArgs e)
        {
            if(addMode)
            {

                if(!string.IsNullOrEmpty(registrarEmloyeesEmployeeFormFullName.Text) && !string.IsNullOrEmpty(registrarEmloyeesEmployeeFormAccess.Text))
                {
                    if(Convert.ToDateTime(registrarEmloyeesEmployeeFormStatusStart.Text) < Convert.ToDateTime(registrarEmloyeesEmployeeFormStatusEnd))
                    {
                        Employee employee = new Employee();
                        employee.id = Guid.NewGuid().ToString();
                        employee.fullName = registrarEmloyeesEmployeeFormFullName.Text;
                        employee.access = 0;
                        if (registrarEmloyeesEmployeeFormAccess.Text == "Администратор")
                            employee.access = 1;
                        if (registrarEmloyeesEmployeeFormAccess.Text == "Регистратор")
                            employee.access = 2;
                        if (registrarEmloyeesEmployeeFormAccess.Text == "Врач")
                            employee.access = 3;
                        if (registrarEmloyeesEmployeeFormAccess.Text == "Медицинский персонал")
                            employee.access = 4;
                        if (employee.access == 0)
                            MessageBox.Show("Должность указана неверно");
                        
                        //db.Employees.Add(employee);
                        //db.SaveChanges();

                    }
                    else
                    {
                        MessageBox.Show("Дата окончания действия статуса не может быть раньше даты начала действия статуса.");
                    }
                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все обязательные поля.");
                }

            }
            else
            {
                //Получение полей
                string fullName = string.Empty;
                string access = string.Empty;
                string status = string.Empty;
                string statusStart = string.Empty;
                string statusEnd = string.Empty;

                if (!string.IsNullOrEmpty(registrarEmloyeesEmployeeFormFullName.Text) && !string.IsNullOrEmpty(registrarEmloyeesEmployeeFormAccess.Text))
                {
                    if(!string.IsNullOrEmpty(registrarEmloyeesEmployeeFormStatusStart.Text) == !string.IsNullOrEmpty(registrarEmloyeesEmployeeFormStatus.Text))
                    {
                        if(!string.IsNullOrEmpty(registrarEmloyeesEmployeeFormStatusStart.Text) && !string.IsNullOrEmpty(registrarEmloyeesEmployeeFormStatus.Text) && string.IsNullOrEmpty(registrarEmloyeesEmployeeFormStatusEnd.Text))
                        {
                            Employee employee = db.Employees.Find();

                            if (registrarEmloyeesEmployeeFormFullName.Text != fullName)
                            {
                                employee.fullName = registrarEmloyeesEmployeeFormFullName.Text;
                            }
                            if (registrarEmloyeesEmployeeFormAccess.Text != access)
                            {
                                access = registrarEmloyeesEmployeeFormAccess.Text;
                            }

                            // Добавление связанной записи в EmployeeStatuss
                            
                        }
                        else
                        {
                            MessageBox.Show("Невозможно задать дату окончания действия статуса без указания статуса и даты начала действия статуса");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Необходимо указать статус и дату начала его действия");
                    }
                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все обязательные поля.");
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
        //public class EmployeeViewModel
        //{
        //    public string fullName { get; set; }
        //    public string access { get; set; }
        //    public ComboBox status { get; set; }
        //    public string start { get; set; }
        //    public string end { get; set; }

        //    public EmployeeViewModel(){}
        //    public EmployeeViewModel(string _fullname,int _access, string _status,List<string> _statuses, string _start,string _end)
        //    {
        //        fullName = _fullname;
                
        //        switch(_access)
        //        {
        //            case 1: access = "Администратор"; break;
        //            case 2: access = "Регистратор"; break;
        //            case 3: access = "Врач"; break;
        //            case 4: access = "Медицинский персонал"; break;
        //        }

        //        status = new ComboBox();
        //        foreach (var item in _statuses)
        //        {
        //            status.Items.Add(item);
        //        }
        //        status.Text = _status;

        //        start = _start;
        //        end = _end;
        //    }
        //}

    }
}
