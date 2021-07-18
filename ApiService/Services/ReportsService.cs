using ApiService.Helpers;
using ApiService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.Services
{
    public class ReportsService : IReportsService
    {
        private IThirdPartyAPI _thirdPartyAPI;
        public ReportsService(IThirdPartyAPI thirdPartyAPI)
        {
            _thirdPartyAPI = thirdPartyAPI;
        }
        public async Task<IEnumerable<RegionResponse>> GetRegionsReport(int top)
        {
            var apiData = await _thirdPartyAPI.GetApiData();

            var groupByRegion = apiData.data.GroupBy(r => r.region.iso)
                .Select(g => new CasesInformation
                {
                    confirmed = g.Sum(c => c.confirmed),
                    deaths = g.Sum(s => s.deaths),
                    region = new Region
                    {
                        iso = g.Key,
                        name = g.First().region.name
                    }
                });
            var orderedData = groupByRegion.OrderByDescending(c => c.confirmed).Take(top);
            var returnData = orderedData.Select(o => new RegionResponse()
            {
                iso = o.region.iso,
                name = o.region.name,
                confirmed = o.confirmed,
                deaths = o.deaths
            }).ToList();
            return returnData;
        }

        public async Task<IEnumerable<RegionResponse>> GetProvinceReport(string iso, int top)
        {
            var apiData = await _thirdPartyAPI.GetApiData(iso);

            var orderedData = apiData.data.OrderByDescending(c => c.confirmed).Take(top);
            var returnData = orderedData.Select(o => new RegionResponse()
            {
                iso = o.region.iso,
                name = o.region.name,
                province = o.region.province,
                confirmed = o.confirmed,
                deaths = o.deaths
            }).ToList();
            return returnData;
        }
    }
}
