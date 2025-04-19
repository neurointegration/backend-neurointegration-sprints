using System.Text.Json.Serialization;

namespace Data
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SprintScenarioType
    {
        EveningStandUp,
        Reflection,
        Status
    }
}
