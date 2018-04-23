using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataIntegrationChallenge.API.Controllers;
using DataIntegrationChallenge.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using MongoDB.Driver;
using NUnit.Framework;

namespace DataIntegrationChallenge.Api.Tests.Integration {
    public class CompaniesControllerTest : BaseIntegrationTest {
        private CompaniesController _controller;

        [SetUp]
        public void SetUp() {
            _controller = new CompaniesController(new CompanyDataContext(MongoDatabase, CollectionName));
        }

        [Test]
        public async Task Get_WithNameAndZip_ShouldReturnCompanies() {
            var companies = await _controller.Get("company", "1234");
            var company = companies.First();
            var expected = new CompanyDto {
                Name = "company",
                AddressZip = "1234",
                Website = "url"
            };
            Assert.That(expected, Is.EqualTo(company).Using(new CompanyEqualityComparer()));
        }

        [Test]
        public async Task Upload_ShouldUpdateDocumentsIgnoringNonExistent() {
            var stream = new MemoryStream(Encoding.ASCII.GetBytes(TestCsv.MergeCsv));
            IFormFile file = new FormFile(stream, 0, stream.Length, "file", "clientData.csv");
            await _controller.Upload(file);

            var companies = await Collection.Find(_ => true).ToListAsync();
            var company = companies.First();
            
            var expected = new CompanyDto {
                Name = "company",
                AddressZip = "1234",
                Website = "new url"
            };
            Assert.That(expected, Is.EqualTo(company).Using(new CompanyEqualityComparer()));
            Assert.AreEqual(1, companies.Count);
        }

        private class CompanyEqualityComparer : IEqualityComparer<CompanyDto> {
            public bool Equals(CompanyDto expected, CompanyDto actual) {
                return expected.Name == actual.Name &&
                       expected.AddressZip == actual.AddressZip &&
                       expected.Website == actual.Website;
            }

            public int GetHashCode(CompanyDto obj) {
                throw new System.NotImplementedException();
            }
        }
    }
}