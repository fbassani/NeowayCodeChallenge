using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataIntegrationChallenge.API.Data {
    public class CompanyDto {
        [BsonId, BsonRepresentation(BsonType.ObjectId), BsonElement("_id")]
        public string Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("addressZip")]
        public string AddressZip { get; set; }
        [BsonElement("website")]
        public string Website { get; set; }
    }
}