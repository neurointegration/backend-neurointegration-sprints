using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Project
    {
        public Guid Id { get; set; }
        public SectionName SectionName { get; set; }
        public string Title { get; set; } = default!;
        public string? TotalTime { get; set; }
        public string? PlanningTimes { get; set; }
        public string? FactTimes { get; set; }

        public Guid SprintId { get; set; }
        public Sprint Sprint { get; set; } = default!;
        public IList<NeuroTask> NeuroTasks { get; set; } = default!;
    }
}
