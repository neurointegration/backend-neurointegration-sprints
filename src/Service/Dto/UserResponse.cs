using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto
{
    public class UserResponse
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? AboutMe { get; set; }
        public string? PhotoUrl { get; set; }
        public bool IsOnboardingComplete { get; set; }
        public int SprintWeeksCount { get; set; }
    }
}
