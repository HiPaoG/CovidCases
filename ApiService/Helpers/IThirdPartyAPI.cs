using ApiService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.Helpers
{
    public interface IThirdPartyAPI
    {
        Task<Report> GetApiData(string iso = null);
    }
}
