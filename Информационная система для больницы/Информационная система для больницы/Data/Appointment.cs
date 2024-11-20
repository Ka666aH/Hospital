using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Информационная_система_для_больницы.Data
{
    class Appointment
    {
        public string id { get; set; }
        public Registration registrationId { get; set; }
        public Employee doctorId { get; set; }
        public DrugProcedure drugProcedureId { get; set; }
        public string note { get; set; }
        public Status statusId { get; set; }

    }
}
