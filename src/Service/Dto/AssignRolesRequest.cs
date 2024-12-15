using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Service.Dto
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoleOption
    {
        ClientOnly,
        TrainerAndClient
    }

    public class AssignRolesRequest
    {
        public RoleOption RoleOption { get; set; }
    }
}
