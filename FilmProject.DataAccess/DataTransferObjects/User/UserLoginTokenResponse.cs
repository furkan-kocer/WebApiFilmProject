using FilmProject.DataAccess.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FilmProject.DataAccess.DataTransferObjects.User
{
    public class UserLoginTokenResponse
    {
        [BsonId]
        public ObjectId userID { get; set; }
        public string? phoneNumber { get; set; }
        public string email { get; set; }
        public Roles Role { get; set; }
    }
}
