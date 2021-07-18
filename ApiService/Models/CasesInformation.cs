using System;
using System.Collections.Generic;
using System.Text;

namespace ApiService.Models
{
    public class CasesInformation
    {
        public int confirmed { get; set; }
        public int deaths { get; set; }
        public Region region { get; set; }
    }
}
