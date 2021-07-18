using ApiService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.Services
{
    public interface IReportsService
    {
        Task<IEnumerable<RegionResponse>> GetRegionsReport(int top);
        Task<IEnumerable<RegionResponse>> GetProvinceReport(string iso, int top);
    }
}
