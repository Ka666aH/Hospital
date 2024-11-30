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
        public string registrationId {  get; set; }
        public virtual Registration registration { get; set; }
        public string doctorId { get; set; }
        public virtual Employee doctor { get; set; }
        public string drugProcedureId { get; set; }
        public virtual DrugProcedure drugProcedure { get; set; }
        public string note { get; set; }
        //public string statusId { get; set; }
        //public virtual Status status { get; set; }

    }
}
