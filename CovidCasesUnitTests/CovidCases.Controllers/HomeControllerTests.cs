using ApiService.Models;
using ApiService.Services;
using CovidCases.Controllers;
using CovidCases.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CovidCasesUnitTests.CovidCases.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        protected HomeController _controller;
        protected Mock<IReportsService> _mockReportsService;
        protected IEnumerable<RegionResponse> mockRegionResponse;
        protected IEnumerable<RegionResponse> mockProvinceResponse;
        protected IFormCollection mockFormCollection;
        protected IFormCollection mockFormCollectionEmptyIso;
        protected Dictionary<string, string> mockTopRegions;
        protected int top;
        protected string iso;

        [SetUp]
        public void Setup()
        {
            _mockReportsService = new Mock<IReportsService>();

            top = 10;
            iso = "USA";

            mockTopRegions = new Dictionary<string, string> { { "USA", "US" }, { "FRA", "France" }, { "BRA", "Brazil" } };
            mockFormCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "topRegions", JsonConvert.SerializeObject(mockTopRegions) },
                { "iso", iso }
            });
            mockFormCollectionEmptyIso = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "topRegions", JsonConvert.SerializeObject(mockTopRegions) },
                { "iso", "" }
            });

            mockRegionResponse = new List<RegionResponse>
            {
                new RegionResponse
                {
                    iso = "USA",
                    name = "US",
                    confirmed = 1900,
                    deaths = 1300
                },
                new RegionResponse
                {
                    iso = "FRA",
                    name = "France",
                    confirmed = 1800,
                    deaths = 500
                },
                new RegionResponse
                {
                    iso = "BRA",
                    name = "Brazil",
                    confirmed = 1700,
                    deaths = 150
                }
            };

            mockProvinceResponse = new List<RegionResponse>
            {
                new RegionResponse
                {
                    iso = "USA",
                    name = "US",
                    province = "South California",
                    confirmed = 19,
                    deaths = 13
                },
                new RegionResponse
                {
                    iso = "USA",
                    name = "US",
                    province = "South California",
                    confirmed = 18,
                    deaths = 5
                },
                new RegionResponse
                {
                    iso = "USA",
                    name = "US",
                    province = "Idaho",
                    confirmed = 17,
                    deaths = 15
                }
            };

            _mockReportsService.Setup(s => s.GetRegionsReport(top)).Returns(Task.FromResult(mockRegionResponse));
            _mockReportsService.Setup(s => s.GetProvinceReport(iso, top)).Returns(Task.FromResult(mockProvinceResponse));

            _controller = new HomeController(_mockReportsService.Object);
        }

        [Test]
        public void Index_returns_regionsData()
        {
            ViewResult result = _controller.Index().Result as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(RegionsData), result.Model.GetType());
        }

        [Test]
        public void Index_returns_viewBagIsoEmpty()
        {
            ViewResult result = _controller.Index().Result as ViewResult;

            Assert.AreEqual("", result.ViewData["iso"]);
        }

        [Test]
        public void Index_POST_returns_regionsData()
        {
            ViewResult result = _controller.Index().Result as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(RegionsData), result.Model.GetType());
        }

        [Test]
        public void Index_POST_returns_viewBagIsoNotEmpty()
        {
            ViewResult result = _controller.Index(mockFormCollection).Result as ViewResult;

            Assert.AreEqual("USA", result.ViewData["iso"]);
        }

        [Test]
        public void Index_POST_returns_redirectToIndexIfIsoEmpty()
        {
            var result = _controller.Index(mockFormCollectionEmptyIso).Result;
            var redirectToAction = (RedirectToActionResult)result;

            Assert.AreEqual("Index", redirectToAction.ActionName);
        }
    }
}
