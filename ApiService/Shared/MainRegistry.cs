using ApiService.Helpers;
using ApiService.Services;
using StructureMap;

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
