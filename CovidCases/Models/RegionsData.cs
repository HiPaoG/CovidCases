using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidCases.Models
{
    public class RegionsData
    {
        public Dictionary<string,string> topRegions { get; set; }
        public List<RegionCasesInformation> regionCasesInformation { get; set; }
    }
}
