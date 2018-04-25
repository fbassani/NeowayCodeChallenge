using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DataIntegrationChallenge.API.Data;
using DataIntegrationChallenge.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataIntegrationChallenge.API.Controllers {
    public abstract class FileReceiverController {
        protected static async Task<IActionResult> ReceiveFile(IFormFile file, Func<IEnumerable<CompanyDto>, Task> dataAction) {
            if (file == null) {
                return new BadRequestResult();
            }
            try {
                var companies = await GetCompaniesFromFile(file);
                await dataAction(companies);
            }
            catch (CsvException) {
                return new BadRequestResult();
            }

            return new OkResult();
        }

        private static async Task<IEnumerable<CompanyDto>> GetCompaniesFromFile(IFormFile file) {
            string content;
            using (var reader = new StreamReader(file.OpenReadStream())) {
                content = await reader.ReadToEndAsync();
            }

            return CsvReader.Read(content);
        }
    }
}