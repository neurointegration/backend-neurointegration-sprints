using Data;
using Microsoft.EntityFrameworkCore;
using Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TrainerCommentService : ITrainerCommentService
    {
        private readonly ApplicationDbContext _context;

        public TrainerCommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CommentResponse?> GetCommentAsync(Guid trainerId, Guid clientId)
        {
            var comment = await _context.TrainerComments
                .AsNoTracking()
                .FirstOrDefaultAsync(tc => tc.TrainerId == trainerId && tc.ClientId == clientId);

            return comment == null ? null : new CommentResponse
            {
                UserId = comment.ClientId,
                CommentText = comment.CommentText
            };
        }

        public async Task<CommentResponse> CreateOrUpdateCommentAsync(Guid trainerId, UpdateCommentRequest request)
        {
            var comment = await _context.TrainerComments
                .FirstOrDefaultAsync(tc => tc.TrainerId == trainerId && tc.ClientId == request.UserId);

            if (comment == null)
            {
                comment = new TrainerComment
                {
                    Id = Guid.NewGuid(),
                    TrainerId = trainerId,
                    ClientId = request.UserId,
                    CommentText = request.CommentText
                };
                _context.TrainerComments.Add(comment);
            }
            else
            {
                comment.CommentText = request.CommentText;
                _context.TrainerComments.Update(comment);
            }

            await _context.SaveChangesAsync();

            return new CommentResponse
            {
                UserId = comment.ClientId,
                CommentText = comment.CommentText
            };
        }
    }
}
