using MongoDB.Bson;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace FilmProject.DataAccess.Helpers
{
    public class ObjectIdConverter : JsonConverter<ObjectId>
    {
        public override ObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                // If it's an object (e.g., an ObjectId in MongoDB), we need to read it differently
                reader.Read(); // Move to the "value" token inside the object
                var value = reader.GetString();
                return ObjectId.TryParse(value, out var objectId) ? objectId : ObjectId.Empty;
            }

            // If it's a string, just parse it normally
            if (reader.TokenType == JsonTokenType.String)
            {
                return ObjectId.TryParse(reader.GetString(), out var objectId) ? objectId : ObjectId.Empty;
            }

            throw new JsonException("Invalid format for ObjectId.");
        }

        public override void Write(Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
