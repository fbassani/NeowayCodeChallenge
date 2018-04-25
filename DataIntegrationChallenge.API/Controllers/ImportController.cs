using System.Threading.Tasks;
using DataIntegrationChallenge.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataIntegrationChallenge.API.Controllers {
    [Route("api/[controller]")]
    public class ImportController : FileReceiverController {
        private readonly ICompanyDataContext _companyDataContext;
        
        public ImportController(ICompanyDataContext companyDataContext) {
            _companyDataContext = companyDataContext;
            
        }
        
        public async Task<IActionResult> Post(IFormFile file) {
            return await ReceiveFile(file, _companyDataContext.Import);            
        }

    }
}