using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataIntegrationChallenge.API.Data {
    public interface ICompanyDataContext {
        Task<List<CompanyDto>> FindAll();
        Task<List<CompanyDto>> FindByNameAndZip(string name, string zip);
        Task<CompanyDto> FindById(string id);
        Task Merge(IEnumerable<CompanyDto> newData);
    }
}