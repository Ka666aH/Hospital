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
        public string patientId { get; set; }
        public virtual Patient patient { get; set; }
        public string reason { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string doctorId { get; set; }
        public virtual Employee doctor { get; set; }
        public string bedId { get; set; }
        public virtual Bed bed { get; set; }
        //public string statusId { get; set; }
        //public virtual Status status { get; set; }

    }
}
