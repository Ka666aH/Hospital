using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Информационная_система_для_больницы.Data
{
    class EmployeeStatus
    {
        public string id { get; set; }
        public Employee employeeId { get; set; }
        public Status statusId  { get; set; }
        public string start{ get; set; }
        public string end{ get; set; }
    }
}
