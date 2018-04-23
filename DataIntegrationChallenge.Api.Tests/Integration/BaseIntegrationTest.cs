using System.Linq;
using DataIntegrationChallenge.API.Data;
using DataIntegrationChallenge.API.Helpers;
using MongoDB.Driver;
using NUnit.Framework;

namespace DataIntegrationChallenge.Api.Tests.Integration {
    public class BaseIntegrationTest {
        private MongoClient _mongoClient;
        private const string Database = "DataIntegrationChallengeTests";
        private const string ConnectionString = "mongodb://localhost:27017";
        protected const string CollectionName = "testCollection";

        protected IMongoDatabase MongoDatabase { get; private set; }
        protected IMongoCollection<CompanyDto> Collection { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetUp() {
            _mongoClient = new MongoClient(ConnectionString);
            MongoDatabase = _mongoClient.GetDatabase(Database);

            Collection = MongoDatabase.GetCollection<CompanyDto>(CollectionName);
        }

        [SetUp]
        public void SetUp() {
            MongoDatabase.DropCollection(CollectionName);
            MongoDatabase.CreateCollection(CollectionName);
            var companies = CsvReader.Read(TestCsv.Content).ToList();
            foreach (var company in companies) {
                // insertmany was failing silently
                Collection.InsertOne(company);
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() {
            _mongoClient.DropDatabase(Database);
        }
    }
}