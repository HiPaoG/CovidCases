using ApiService.Helpers;
using ApiService.Services;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiService.Shared
{
    public class MainRegistry : Registry
    {
        public MainRegistry()
        {
            For<IReportsService>().Use<ReportsService>();
            For<IThirdPartyAPI>().Use<ThirdPartyAPI>();
        }
    }
}
