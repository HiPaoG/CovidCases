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
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.ComponentModel;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.StaticFiles;
using CovidCases.Helpers;

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
            ViewBag.iso = "";
            return View(regionsData);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormCollection collection)
        {
            var topRegions = JsonConvert.DeserializeObject<Dictionary<string, string>>(collection["topRegions"]);
            var iso = collection["iso"];
            if (String.IsNullOrEmpty(iso))
                return RedirectToAction("Index");
            else
            {
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
                ViewBag.iso = iso;
                return View(regionsData);
            }
        }

        [HttpGet]
        public async Task<FileResult> Export(ExportType type, string iso)
        {
            var fileProvider = new FileExtensionContentTypeProvider();
            string extension = type.GetStringValue();
            var fileName = "CovidCasesReport" + DateTime.Now.ToString("ddMMyyyyhhmm");
            byte[] document = String.IsNullOrEmpty(iso) ? await ExportRegionsData(type) : await ExportProvincesData(type, iso);
            fileProvider.TryGetContentType(extension, out string contentType);
            return File(document, contentType, fileName+extension);
        }

        public async Task<byte[]> ExportRegionsData(ExportType type)
        {
            byte[] document = null;
            var regionResponse = await _reportsService.GetRegionsReport(10);
            var regionsData = regionResponse.Select(d => new RegionsExportData
            {
                region = d.name,
                confirmed = d.confirmed.ToString("N0"),
                deaths = d.deaths.ToString("N0")
            }).ToList();
            switch (type)
            {
                case ExportType.XML:
                    document = GenerateXML(regionsData);
                    break;
                case ExportType.JSON:
                    document = GenerateJSON(regionsData);
                    break;
                case ExportType.CSV:
                    document = GenerateCSV(regionsData);
                    break;
            }
            return document;
        }

        public async Task<byte[]> ExportProvincesData(ExportType type, string iso)
        {
            byte[] document = null;
            var provinceResponse = await _reportsService.GetProvinceReport(iso, 10);
            var provincesData = provinceResponse.Select(d => new ProvinceExportData
            {
                region = d.name,
                province = d.province,
                confirmed = d.confirmed.ToString("N0"),
                deaths = d.deaths.ToString("N0")
            }).ToList();
            switch (type)
            {
                case ExportType.XML:
                    document = GenerateXML(provincesData);
                    break;
                case ExportType.JSON:
                    document = GenerateJSON(provincesData);
                    break;
                case ExportType.CSV:
                    document = GenerateCSV(provincesData);
                    break;
            }
            return document;
        }

        private byte[] GenerateXML<T>(List<T> data)
        {
            XmlSerializer xs;
            MemoryStream ms = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(ms, Encoding.UTF8);
            xs = new XmlSerializer(typeof(List<T>));
            xs.Serialize(xmlTextWriter, data);
            ms = (MemoryStream)xmlTextWriter.BaseStream;
            ms.Position = 0;
            return ms.ToArray();
        }

        private byte[] GenerateJSON<T>(List<T> data)
        {
            string dataJson = JsonConvert.SerializeObject(data);
            string result = Convert.ToBase64String(Encoding.UTF8.GetBytes(dataJson));
            return Convert.FromBase64String(result);
        }

        private byte[] GenerateCSV<T>(List<T> data)
        {
            byte[] result;
            using (var mem = new MemoryStream())
            using (var writer = new StreamWriter(mem))
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteHeader<T>();
                csvWriter.NextRecord();
                csvWriter.WriteRecords(data);
                writer.Flush();
                result = mem.ToArray();
            }
            return result;
        }
        public IActionResult Privacy()
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
