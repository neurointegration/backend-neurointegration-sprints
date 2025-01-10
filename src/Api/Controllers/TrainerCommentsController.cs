using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service;
using Api.Extensions;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/Trainer/{userId}/comment")]
    [Authorize(Roles = "Trainer")]
    public class TrainerCommentsController : ControllerBase
    {
        private readonly ITrainerCommentService _trainerCommentService;

        public TrainerCommentsController(ITrainerCommentService trainerCommentService)
        {
            _trainerCommentService = trainerCommentService;
        }

        [HttpGet]
        public async Task<ActionResult<CommentResponse>> GetComment(Guid userId)
        {
            var trainerId = User.GetUserId();

            var comment = await _trainerCommentService.GetCommentAsync(trainerId, userId);

            if (comment == null)
                return NotFound();

            return Ok(comment);
        }

        [HttpPost]
        public async Task<ActionResult<CommentResponse>> CreateOrUpdateComment(
            Guid userId,
            [FromBody] UpdateCommentRequest request)
        {
            var trainerId = User.GetUserId();
            var updatedComment = await _trainerCommentService.CreateOrUpdateCommentAsync(trainerId, request);
            return Ok(updatedComment);
        }
    }
}
