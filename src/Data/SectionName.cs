using System.Text.Json.Serialization;

namespace Data
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SectionName
    {
        Life,
        Drive,
        Fun
    }
}
