using System.Text.Json;
using System.Text.Json.Serialization;

namespace xemtest
{



    public class IntPtrConverter : JsonConverter<IntPtr>
    {

        public override bool CanConvert(Type type)
        {
            return typeof(IntPtr).IsAssignableFrom(type);
        }

        public override IntPtr Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            if (!reader.Read()
                    || reader.TokenType != JsonTokenType.PropertyName
                    || reader.GetString() != "TypeDiscriminator")
            {
                throw new JsonException();
            }

            if (!reader.Read() || reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            IntPtr res = IntPtr.Zero;

            string intptrname = reader.GetString();
            if (intptrname == "IntPtr")
            {
                if (!reader.Read() || reader.GetString() != "TypeValue")
                {
                    throw new JsonException();
                }
                if (!reader.Read() || reader.TokenType != JsonTokenType.String)
                {
                    throw new JsonException();
                }
                res = IntPtr.Parse(reader.GetString());

            }


            if (!reader.Read() || reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }

            return res;
        }

        public override void Write(
            Utf8JsonWriter writer,
            IntPtr value,
            JsonSerializerOptions options)
        {
            writer.WriteStartObject();


            writer.WriteString("TypeDiscriminator", "IntPtr");
            writer.WritePropertyName("TypeValue");
            JsonSerializer.Serialize(writer, value.ToString());


            writer.WriteEndObject();
        }
    }
}