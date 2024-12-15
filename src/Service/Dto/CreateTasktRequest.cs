﻿using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto
{
    public class CreateTaskRequest
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; } = default!;
        public SectionName SectionName { get; set; }
        public Dictionary<int, PlanningTimeDto>? PlanningTimes { get; set; }
        public Dictionary<int, FactTimeDto>? FactTimes { get; set; }
    }
}