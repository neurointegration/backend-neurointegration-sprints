using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto
{
    public class FactTimeDto
    {
        [Range(0, 100, ErrorMessage = "Hours must be between 0 and 100.")]
        public int Hours { get; set; }

        [Range(0, 59, ErrorMessage = "Minutes must be between 0 and 59.")]
        public int Minutes { get; set; }

        [RegularExpression("^#[0-9A-Fa-f]{6}$", ErrorMessage = "Color must be in HEX format, e.g., #75151e.")]
        public String Color { get; set; } = default!;
    }
}
