using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Service.Dto
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TimeStatus
    {
        Decline,
        Continue,
        Modify
    }

    public class FactTimeDto
    {
        [Range(0, 100, ErrorMessage = "Hours must be between 0 and 100.")]
        public int Hours { get; set; }

        [Range(0, 59, ErrorMessage = "Minutes must be between 0 and 59.")]
        public int Minutes { get; set; }

        public TimeStatus? Status { get; set; }
    }
}
