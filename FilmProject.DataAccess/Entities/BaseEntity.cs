using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FilmProject.DataAccess.Entities
{
    public abstract class BaseEntity
    {
        [BsonId]
        public ObjectId Id { get; init; }
        public DateTime createdDate { get; set; }
        public DateTime updatedDate { get; set; }
        public string status { get; set; }
    }
}
