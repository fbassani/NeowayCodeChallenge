using Autofac;
using DataIntegrationChallenge.API.Data;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace DataIntegrationChallenge.API.Modules {
    public class MongoDbModule : Module {
        private readonly IConfigurationRoot _configurationRoot;
        public MongoDbModule(IConfigurationRoot configurationRoot) {
            _configurationRoot = configurationRoot;
        }

        protected override void Load(ContainerBuilder builder) {
            base.Load(builder);
            var connectionString = _configurationRoot["MongoDb:Url"];
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(_configurationRoot["MongoDb:Database"]);

            builder.Register((c, p) => client).As<IMongoClient>();
            builder.Register((c, p) => db).As<IMongoDatabase>();

            builder.RegisterType<CompanyDataContext>()
                .WithParameter(new TypedParameter(typeof(string), _configurationRoot["MongoDb:collection"]))
                .As<ICompanyDataContext>();
        }
    }
}