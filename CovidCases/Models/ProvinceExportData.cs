using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidCases.Models
{
    public class ProvinceExportData
    {
        public string region { get; set; }
        public string province { get; set; }
        public string confirmed { get; set; }
        public string deaths { get; set; }
    }
}
