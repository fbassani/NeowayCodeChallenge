using System.Collections.Generic;
using System.Linq;
using DataIntegrationChallenge.API.Data;
using DataIntegrationChallenge.API.Helpers;
using NUnit.Framework;

namespace DataIntegrationChallenge.Api.Tests.Helpers {
    public class CsvReaderTests {
        private IEnumerable<CompanyDto> _dtos;
        private CompanyDto _dto;

        [SetUp]
        public void SetUp() {
            _dtos = CsvReader.Read(TestCsv.Content);
            _dto = _dtos.First();
        }
        
        [Test]
        public void Read_ShouldMapName() {
            Assert.AreEqual("company", _dto.Name);
        }
        
        [Test]
        public void Read_ShouldMapZip() {
            Assert.AreEqual("1234", _dto.AddressZip);
        }
        
        [Test]
        public void Read_ShouldMapWebsite() {
            Assert.AreEqual("url", _dto.Website);
        }
        
        [TestCase("website", 0)]
        [TestCase("name", 1)]
        [TestCase("addressZip", 2)]
        public void FindColumnIndex_WithOtherColumnOrder_ShouldFindColumns(string column, int expectedIndex) {
            var index = CsvReader.FindColumnIndex(TestCsv.DifferentColumnOrderHeader, column);
            Assert.AreEqual(expectedIndex, index);
        }

    }
}