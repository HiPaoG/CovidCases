using ApiService.Helpers;
using ApiService.Models;
using ApiService.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CovidCasesUnitTests.ApiService.Services
{
    [TestFixture]
    public class ReportsServiceTests
    {
        protected ReportsService _reportsService;
        protected Mock<IThirdPartyAPI> _mockThirdPartyAPI;
        protected Report mockRegionReport;
        protected int top;
        protected int sumConfirmed;
        protected int sumDeath;
        protected int firstConfirmed;
        protected int firstDeath;
        protected string iso;
        [SetUp]
        public void Setup()
        {
            _mockThirdPartyAPI = new Mock<IThirdPartyAPI>();

            top = 10;
            sumConfirmed = 36;
            sumDeath = 23;
            firstConfirmed = 17; //As ordered desc
            firstDeath = 15;
            iso = "USA";

            mockRegionReport = new Report
            {
                data = new List<CasesInformation>
                {
                    new CasesInformation
                    {
                        region = new Region
                        {
                            iso = "USA",
                            name = "US",
                            province = "South California"
                        },
                        confirmed = 9,
                        deaths = 3
                    },
                    new CasesInformation
                    {
                        region = new Region
                        {
                            iso = "USA",
                            name = "US",
                            province = "South California"
                        },
                        confirmed = 10,
                        deaths = 5
                    },
                    new CasesInformation
                    {
                        region = new Region
                        {
                            iso = "USA",
                            name = "US",
                            province = "Idaho"
                        },
                        confirmed = 17,
                        deaths = 15
                    }
                }
            };

            _mockThirdPartyAPI.Setup(s => s.GetApiData(null)).Returns(Task.FromResult(mockRegionReport));
            _mockThirdPartyAPI.Setup(s => s.GetApiData(iso)).Returns(Task.FromResult(mockRegionReport));

            _reportsService = new ReportsService(_mockThirdPartyAPI.Object);
        }

        [Test]
        public void ReportsServicesGetRegionsReport_returns_regionResponse()
        {
            var result = _reportsService.GetRegionsReport(top).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(List<RegionResponse>), result.GetType());
        }

        [Test]
        public void ReportsServicesGetRegionsReport_returns_confirmedAndDeathsSum()
        {
            var result = (List<RegionResponse>)_reportsService.GetRegionsReport(top).Result;
            
            Assert.AreEqual(sumConfirmed, result[0].confirmed);
            Assert.AreEqual(sumDeath, result[0].deaths);
        }

        [Test]
        public void ReportsServicesGetProvinceReport_returns_regionResponse()
        {
            var result = _reportsService.GetProvinceReport(iso, top).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(List<RegionResponse>), result.GetType());
        }

        [Test]
        public void ReportsServicesGetProvinceReport_returns_confirmedAndDeathsValues()
        {
            var result = (List<RegionResponse>)_reportsService.GetProvinceReport(iso, top).Result;

            Assert.AreEqual(firstConfirmed, result[0].confirmed);
            Assert.AreEqual(firstDeath, result[0].deaths);
        }
    }
}
