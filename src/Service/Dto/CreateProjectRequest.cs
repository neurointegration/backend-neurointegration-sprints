using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto
{
    public class CreateProjectRequest
    {
        public Guid SprintId { get; set; }
        public string Title { get; set; } = default!;
        public SectionName SectionName { get; set; }
        public Dictionary<String, PlanningTimeDto>? PlanningTimes { get; set; }
        public Dictionary<String, FactTimeDto>? FactTimes { get; set; }
    }
}
