using Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvatarController : ControllerBase
    {
        private readonly AvatarService _avatarService;

        public AvatarController(AvatarService avatarService)
        {
            _avatarService = avatarService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            try
            {
                CheckImage(file);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }

            var userId = User.GetUserId();
            var avatarUrl = await _avatarService.UploadAvatarAsync(userId.ToString(), file);

            return Ok(new { Url = avatarUrl });
        }

        private void CheckImage(IFormFile file)
        {
            var maxFileSize = 5242880; // 5 mb
            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif" };

            if (file == null || file.Length == 0)
                throw new ArgumentException("The file is missing or empty.");

            if (file.Length > maxFileSize)
                throw new ArgumentException($"The file size exceeds {maxFileSize / 1024 / 1024} MB.");

            if (!allowedMimeTypes.Contains(file.ContentType))
                throw new ArgumentException("Invalid file type. Only images (JPEG, PNG, GIF) are allowed.");
        }
    }
}
