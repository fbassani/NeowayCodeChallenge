using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace DataIntegrationChallenge.API.Data {
    public class CompanyDataContext : ICompanyDataContext {
        private readonly IMongoCollection<CompanyDto> _collection;

        public CompanyDataContext(IMongoDatabase mongoDatabase, string collectionName) {
            _collection = mongoDatabase.GetCollection<CompanyDto>(collectionName);
        }

        public async Task<List<CompanyDto>> FindByNameAndZip(string name, string zip) {
            var builder = Builders<CompanyDto>.Filter;
            var filter = builder.Regex(c => c.Name, new BsonRegularExpression($"/.*{name}.*/i")) &
                         builder.Eq(c => c.AddressZip, zip);

            var cursor = await _collection.FindAsync(filter);
            return await cursor.ToListAsync();
        }

        public async Task Merge(IEnumerable<CompanyDto> newData) {
            var operations = new List<UpdateManyModel<CompanyDto>>();
            var updateBuilder = Builders<CompanyDto>.Update;
            var filterBuilder = Builders<CompanyDto>.Filter;

            foreach (var dto in newData) {
                var update = updateBuilder.Set(d => d.Website, dto.Website);
                var filter = filterBuilder.Regex(d => d.Name, new BsonRegularExpression($"/^({dto.Name})$/i")) &
                             filterBuilder.Eq(d => d.AddressZip, dto.AddressZip);
                operations.Add(new UpdateManyModel<CompanyDto>(filter, update));
            }
            await _collection.BulkWriteAsync(operations);
        }

        public async Task Import(IEnumerable<CompanyDto> companies) {
            await _collection.Indexes.CreateOneAsync(Builders<CompanyDto>.IndexKeys.Ascending(c => c.Name));
            await _collection.InsertManyAsync(companies);
        }
    }
}    