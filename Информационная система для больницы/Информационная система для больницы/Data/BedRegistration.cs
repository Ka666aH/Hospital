using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Информационная_система_для_больницы.Data
{
    class BedRegistration
    {
        public string id { get; set; }
        public string bedId { get; set; }
        public virtual Bed bed { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }
}
