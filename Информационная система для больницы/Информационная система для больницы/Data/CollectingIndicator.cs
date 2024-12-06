using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Информационная_система_для_больницы.Data
{
    public class CollectingIndicator
    {
        public string id { get; set; }
        public string registrationId { get; set; }
        public virtual Registration registration { get; set; }
        public string indicatorId { get; set; }
        public virtual Indicator indicator { get; set; }
        //public string collect {  get; set; }
    }
}
