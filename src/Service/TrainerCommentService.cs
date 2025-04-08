using Data;
using Data.Repositories;
using Service.Dto;

namespace Service
{
    public class TrainerCommentService : ITrainerCommentService
    {
        private readonly ITrainerRepository _trainerRepository;

        public TrainerCommentService(ITrainerRepository trainerRepository)
        {
            _trainerRepository = trainerRepository;
        }

        public async Task<CommentResponse?> GetCommentAsync(long trainerId, long clientId)
        {
            var comment = await _trainerRepository.GetCommentAsync(trainerId, clientId);
            return comment == null ? null : new CommentResponse
            {
                UserId = comment.ClientId,
                CommentText = comment.CommentText ?? string.Empty
            };
        }

        public async Task<CommentResponse> CreateOrUpdateCommentAsync(long trainerId, UpdateCommentRequest request)
        {
            var comment = new TrainerComment
            {
                TrainerId = trainerId,
                ClientId = request.UserId,
                CommentText = request.CommentText
            };
            await _trainerRepository.UpdateCommentAsync(comment);

            return new CommentResponse
            {
                UserId = comment.ClientId,
                CommentText = comment.CommentText
            };
        }
    }
}
