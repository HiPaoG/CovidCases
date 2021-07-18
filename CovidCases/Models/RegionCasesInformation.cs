using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidCases.Models
{
    public class RegionCasesInformation
    {
        public string iso { get; set; }
        public string name { get; set; }
        public string province { get; set; }
        public int confirmed { get; set; }
        public int deaths { get; set; }
    }
}
