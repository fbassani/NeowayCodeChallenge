using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DataIntegrationChallenge.API.Data;
using DataIntegrationChallenge.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataIntegrationChallenge.API.Controllers {
    [Route("api/[controller]")]
    public class CompaniesController {
        private readonly ICompanyDataContext _companyDataContext;

        public CompaniesController(ICompanyDataContext companyDataContext) {
            _companyDataContext = companyDataContext;
        }

        public async Task<List<CompanyDto>> Get(string name, string zip) {
            return await _companyDataContext.FindByNameAndZip(name, zip);
        }

        [HttpGet("api/company/{id}")]
        public async Task<CompanyDto> Get(string id) {
            return await _companyDataContext.FindById(id);
        }

        // This operation is indepontent, thus, PUT should be considered the correct method
        // for uploading the file.
        // However, HTML spec allows GET and POST on forms. If the consuming app uses a webpage
        // POST must also work.
        [HttpPut, HttpPost]
        public async Task<IActionResult> Upload(IFormFile file) {
            if (file == null) {
                return new BadRequestResult();
            }
            string content;
            using (var reader = new StreamReader(file.OpenReadStream())) {
                content = await reader.ReadToEndAsync();
            }

            try {
                var companies = CsvReader.Read(content);
                await _companyDataContext.Merge(companies);
            }
            catch (CsvException) {
                return new BadRequestResult();
            } 

            return new OkResult();
        }
    }
}