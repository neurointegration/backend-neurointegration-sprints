using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class TrainerComment
    {
        public Guid Id { get; set; }

        public string CommentText { get; set; } = default!;

        public Guid ClientId { get; set; }
        public ApplicationUser Client { get; set; } = default!;

        public Guid TrainerId { get; set; }
        public ApplicationUser Trainer { get; set; } = default!;
    }   
}
