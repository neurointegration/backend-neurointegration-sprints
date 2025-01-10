
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto
{
    public class UpdateCommentRequest
    {
        public Guid UserId { get; set; }
        public string CommentText { get; set; } = string.Empty;
    }
}
