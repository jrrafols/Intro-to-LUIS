using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example1.Models
{
    public class Intent
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public int typeId {get; set;}
        public string readableType { get; set; }
        public string customPrebuiltDomainName  { get; set; } 
        public string customPrebuiltModelName { get; set; }
    }
}
