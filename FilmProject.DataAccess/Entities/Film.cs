using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FilmProject.DataAccess.Entities
{
    public class Film : BaseEntity
    {
        [BsonElement("filmName")]
        [JsonPropertyName("filmName")]
        public string FilmName { get; set; }

        [BsonElement("price")]
        [JsonPropertyName("price")]
        public float Price { get; set; }

        [BsonElement("filmCode")]
        [JsonPropertyName("filmCode")]
        public string FilmCode { get; set; }
    }
}
