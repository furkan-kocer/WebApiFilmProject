using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
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
        [BsonElement("filmDescription")]
        [JsonPropertyName("filmDescription")]
        public string FilmDescription { get; set; }
        [BsonElement("imgUrl")]
        [JsonPropertyName("imgUrl")]
        public string ImgUrl { get; set; }
    }
}
