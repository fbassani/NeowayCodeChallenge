using System.Collections.Generic;
using System.Threading.Tasks;
using DataIntegrationChallenge.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataIntegrationChallenge.API.Controllers {
    [Route("api/[controller]")]
    public class CompaniesController : FileReceiverController {
        private readonly ICompanyDataContext _companyDataContext;

        public CompaniesController(ICompanyDataContext companyDataContext) {
            _companyDataContext = companyDataContext;
        }

        public async Task<List<CompanyDto>> Get(string name, string zip) {
            return await _companyDataContext.FindByNameAndZip(name, zip);
        }

        // This operation is indepontent, thus, PUT should be considered the correct method
        // for uploading the file.
        // However, HTML spec allows GET and POST on forms. If the consuming app uses a webpage
        // POST must also work.
        [HttpPut, HttpPost]
        public async Task<IActionResult> Upload(IFormFile file) {
            return await ReceiveFile(file, _companyDataContext.Merge);
        }
    }
}