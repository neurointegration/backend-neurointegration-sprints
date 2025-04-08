using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Data
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TimeStatus
    {
        Decline,
        Continue,
        Modify
    }

    public class FactTime
    {
        [Range(0, 100, ErrorMessage = "Hours must be between 0 and 100.")]
        public int Hours { get; set; }

        [Range(0, 59, ErrorMessage = "Minutes must be between 0 and 59.")]
        public int Minutes { get; set; }

        public TimeStatus? Status { get; set; }
    }
}
