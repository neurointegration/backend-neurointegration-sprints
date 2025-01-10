using Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ITrainerCommentService
    {
        Task<CommentResponse?> GetCommentAsync(Guid trainerId, Guid clientId);
        Task<CommentResponse> CreateOrUpdateCommentAsync(Guid trainerId, UpdateCommentRequest request);
    }
}
