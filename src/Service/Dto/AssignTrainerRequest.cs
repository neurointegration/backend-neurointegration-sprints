using System.ComponentModel.DataAnnotations;

namespace Service.Dto
{
    public class AssignTrainerRequest
    {
        [RegularExpression(@"^@.+", ErrorMessage = "TrainerUsername must start with '@'.")]
        public string TrainerUsername { get; set; } = default!;
    }
}
