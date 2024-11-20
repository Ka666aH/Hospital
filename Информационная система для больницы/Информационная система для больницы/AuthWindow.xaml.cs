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
            InitializeComponent();
            db = new Data.AppContext();
            FillUsers();
        }

        public void FillUsers()
        {
            
            List<Employee> employees = db.Employees.ToList();
            foreach (Employee employee in employees)
            {
                //MessageBox.Show(employee.fullName);
                userPicker.Items.Add(employee.fullName);
            }
            //userPicker.ItemsSource = 
        }
    }
}
