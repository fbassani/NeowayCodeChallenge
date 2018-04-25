using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataIntegrationChallenge.API.Data {
    public interface ICompanyDataContext {
        Task<List<CompanyDto>> FindByNameAndZip(string name, string zip);
        Task Merge(IEnumerable<CompanyDto> newData);
        Task Import(IEnumerable<CompanyDto> companies);
    }
}