using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Информационная_система_для_больницы.Data
{
    class Registration
    {
        public string id { get; set; }
        public Patient patientId { get; set; }
        public string reason { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public Employee doctorId { get; set; }
        public Bed bedId { get; set; }
        public Status statusId { get; set; }

    }
}
