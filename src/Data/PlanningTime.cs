using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class PlanningTime
    {
        [Range(0, 100, ErrorMessage = "Hours must be between 0 and 100.")]
        public int Hours { get; set; }

        [Range(0, 59, ErrorMessage = "Minutes must be between 0 and 59.")]
        public int Minutes { get; set; }
    }
}
