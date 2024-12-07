using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Информационная_система_для_больницы.Data
{
    public class Schedule
    {
        public string id { get; set; }
        public string appointmentId { get; set; }
        public virtual Appointment appointment { get; set; }
        public string dateTime { get; set; }
        public string note { get; set; }
        public string status { get; set; }
        //public string statusId { get; set; }
        //public virtual Status status{ get; set; }
    }
}
