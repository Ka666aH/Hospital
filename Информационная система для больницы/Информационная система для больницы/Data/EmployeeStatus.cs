using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Информационная_система_для_больницы.Data
{
    public class EmployeeStatus
    {
        public string id { get; set; }
        public string employeeId { get; set; }
        public virtual Employee employee { get; set; }
        public string statusId { get; set; }
        public virtual Status status  { get; set; }
        public string start{ get; set; }
        public string end{ get; set; }
    }
}
