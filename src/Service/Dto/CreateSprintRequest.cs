using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto
{
    public class CreateSprintRequest
    {
        public int WeeksCount { get; set; }
        public DateOnly BeginDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Dictionary<int, SprintWeekDto> Weeks { get; set; } = default!;
    }
}
