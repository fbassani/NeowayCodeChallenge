using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DataIntegrationChallenge.API.Controllers;
using DataIntegrationChallenge.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DataIntegrationChallenge.Api.Tests.Controllers {
    public class CompaniesControllerTests {
        private Mock<ICompanyDataContext> _dataContextMock;
        private CompaniesController _controller;
        private Mock<IFormFile> _file;
        private const string Name = "name";
        private const string Zip = "123";
        private const string Id = "abc123";
        
        [SetUp]
        public void SetUp() {
            _dataContextMock = new Mock<ICompanyDataContext>();
            _controller = new CompaniesController(_dataContextMock.Object);
            _file = new Mock<IFormFile>();
            _file.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(Encoding.ASCII.GetBytes(TestCsv.Content)));
        }
        
        [Test]
        public async Task Get_WithNameAndZip_ShouldSearchByNameAndZip() {
            await _controller.Get(Name, Zip);
            _dataContextMock.Verify(d => d.FindByNameAndZip(Name, Zip));
        }
        
        [Test]
        public async Task Get_WithId_ShouldSearchById() {
            await _controller.Get(Id);
            _dataContextMock.Verify(d => d.FindById(Id));
        }
        
        [Test]
        public async Task Upload_WithFile_ShouldMergeData() {
            await _controller.Upload(_file.Object);
            _dataContextMock.Verify(d => d.Merge(It.IsAny<IEnumerable<CompanyDto>>()));
        }
        
        [Test]
        public async Task Upload_WithFile_ShouldReturnOkResult() {
            var result = await _controller.Upload(_file.Object);
            Assert.IsInstanceOf<OkResult>(result);
        }
        
        [Test]
        public async Task Upload_WithouFile_ShouldReturnBadRequest() {
            var result = await _controller.Upload(null);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
    }
}