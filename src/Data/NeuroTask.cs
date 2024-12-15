using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class NeuroTask
    {
        public Guid Id { get; set; }
        public SectionName SectionName { get; set; }
        public string Title { get; set; } = default!;
        public string? PlanningTimes { get; set; }
        public string? FactTimes { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = default!;
    }
}
