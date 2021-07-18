using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CovidCases.Models;
using ApiService.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CovidCases.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IReportsService _reportsService;

        public HomeController(ILogger<HomeController> logger, IReportsService reportsService)
        {
            _logger = logger;
            _reportsService = reportsService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _reportsService.GetRegionsReport(10);
            var regionInformation = data.Select(d => new RegionCasesInformation
            {
                iso = d.iso,
                name = d.name,
                confirmed = d.confirmed,
                deaths = d.deaths
            }).ToList();
            var regionsData = new RegionsData
            {
                topRegions = data.ToDictionary(x => x.iso, x => x.name),
                regionCasesInformation = regionInformation
            };
            return View(regionsData);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormCollection collection)
        {
            var topRegions = JsonConvert.DeserializeObject<Dictionary<string, string>>(collection["topRegions"]);
            var iso = collection["iso"];
            var data = await _reportsService.GetProvinceReport(iso, 10);
            var regionInformation = data.Select(d => new RegionCasesInformation
            {
                iso = d.iso,
                name = d.province,
                confirmed = d.confirmed,
                deaths = d.deaths
            }).ToList();
            var regionsData = new RegionsData
            {
                topRegions = topRegions,
                regionCasesInformation = regionInformation
            };
            return View(regionsData);
        }

        public IActionResult Privacy(string iso)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
