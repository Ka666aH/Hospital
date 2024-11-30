using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Информационная_система_для_больницы.Data
{
    class Status
    {
        public string id { get; set; }
        //public string group { get; set; }
        public string statusName { get; set; }
        public string description { get; set; }
    }
}
