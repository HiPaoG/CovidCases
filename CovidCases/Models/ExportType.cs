using CovidCases.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidCases.Models
{
    public enum ExportType
    {
        [StringValue(".xml")]
        XML,
        [StringValue(".json")]
        JSON,
        [StringValue(".csv")]
        CSV
    }
}
