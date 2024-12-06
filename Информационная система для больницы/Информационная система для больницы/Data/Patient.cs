using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Информационная_система_для_больницы.Data
{
    public class Patient
    {
        public string id { get; set; }
        public string fullName { get; set; }
        public string birthDate { get; set; }
        public string gender { get; set; }
        public string OMC { get; set; }
    }
}
