using ApiService.Models;
using ApiService.Shared;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiService.Helpers
{
    public class ThirdPartyAPI : IThirdPartyAPI
    {
        private IOptions<Configurations> _options;
        public ThirdPartyAPI(IOptions<Configurations> options)
        {
            _options = options;
        }
        public async Task<Report> GetApiData(string iso = null)
        {
            string param = iso != null ? "?iso=" + iso : "";
            var jsonResponse = new Report();
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_options.Value.CovidApiUrl}reports" + param),
                Headers ={
                    { "x-rapidapi-key", _options.Value.CovidApiKey },
                    { "x-rapidapi-host", _options.Value.CovidApiHost },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                jsonResponse = JsonConvert.DeserializeObject<Report>(body);
            }
            return jsonResponse;
        }
    }
}
