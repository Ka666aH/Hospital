using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Информационная_система_для_больницы.Data
{
    class PatientCondition
    {
        public string id { get; set; }
        public Registration registrationId { get; set; }
        public string dateTime { get; set; }
        public Indicator indicatorId { get; set; }
        public string value { get; set; }
        public string note { get; set; }
    }
}
