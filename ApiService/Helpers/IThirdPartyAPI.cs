using ApiService.Models;
using System.Threading.Tasks;

namespace ApiService.Helpers
{
    public interface IThirdPartyAPI
    {
        Task<Report> GetApiData(string iso = null);
    }
}
