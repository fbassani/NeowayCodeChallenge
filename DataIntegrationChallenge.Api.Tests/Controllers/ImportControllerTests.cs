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
    public class ImportControllerTests {
        private Mock<ICompanyDataContext> _dataContextMock;
        private ImportController _controller;
        private Mock<IFormFile> _file;
        
        [SetUp]
        public void SetUp() {
            _dataContextMock = new Mock<ICompanyDataContext>();
            _controller = new ImportController(_dataContextMock.Object);
            _file = new Mock<IFormFile>();
            _file.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(Encoding.ASCII.GetBytes(TestCsv.Content)));
        }
        
        [Test]
        public async Task Post_WithFile_ShouldReturnOkResult() {
            var result = await _controller.Post(_file.Object);
            Assert.IsInstanceOf<OkResult>(result);
        }
        
        [Test]
        public async Task Post_WithouFile_ShouldReturnBadRequest() {
            var result = await _controller.Post(null);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
    }
}