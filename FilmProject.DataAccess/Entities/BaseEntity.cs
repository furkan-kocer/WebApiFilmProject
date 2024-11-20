using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FilmProject.DataAccess.Entities
{
    public abstract class BaseEntity
    {
        [BsonId]
        public ObjectId Id { get; init; }
        public DateTime createdDate { get; init; }
        public DateTime updatedDate { get; init; }
        public string status { get; set; }
    }
}
