using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Информационная_система_для_больницы.Data
{
    class Schedule
    {
        public string id { get; set; }
        public Appointment appointmentId { get; set; }
        public string dateTime { get; set; }
        public string note { get; set; }
        public Status statusId { get; set; }
    }
}
