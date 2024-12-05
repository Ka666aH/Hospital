using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Информационная_система_для_больницы.Data;

namespace Информационная_система_для_больницы
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        Data.AppContext db;
        public AuthWindow()
        {
            //MessageBox.Show(Guid.NewGuid().ToString());
            InitializeComponent();
            db = new Data.AppContext();

            //db.Registrations.ToList();
            //db.Indicators.ToList();
            //db.CollectingIndicators.ToList();
            //db.PatientConditions.ToList();

            FillUsers();
        }

        public void FillUsers()
        {

            List<Employee> employees = db.Employees.ToList();
            foreach (Employee employee in employees)
            {
                userPicker.Items.Add(employee.fullName);
            }

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if(Application.Current.MainWindow != this)
                this.DialogResult = false;
            Close();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(userPicker.Text) && !string.IsNullOrEmpty(pass.Password))
            {
                var q1 = from em in db.Employees
                         where em.fullName == userPicker.Text
                         select em;
                Employee employee = q1.FirstOrDefault();

                if (employee.password == pass.Password)
                {
                    if (Application.Current.MainWindow == this)
                    {
                        MainWindow main = new MainWindow();
                        Application.Current.MainWindow = main;
                        Application.Current.MainWindow.Tag = employee.access;
                        this.Close();
                        main.Show();
                    }
                    else
                    {
                        this.DialogResult = true;
                        Application.Current.MainWindow.Tag = employee.access;
                        this.Close();
                    }
                    //this.Close();
                }
                else
                {
                    MessageBox.Show("Неправильное имя пользователя или пароль.");
                }
            }
            else
            {
                MessageBox.Show("Необходимо заполнить все поля.");
            }
        }

        private void userPicker_DropDownClosed(object sender, EventArgs e)
        {
            pass.Focus();
        }
    }
}
