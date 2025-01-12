using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service;
using Api.Extensions;
using Service.Exceptions;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MeController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;

        public MeController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpPost("trainer")]
        public async Task<IActionResult> AssignTrainer([FromBody] AssignTrainerRequest request)
        {
            var userId = User.GetUserId();

            try
            {
                await _userManagementService.AssignTrainerAsync(userId, request.TrainerUsername);
                return Ok("Trainer assigned successfully.");
            }
            catch (TrainerNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (TrainerNotInBotException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("trainer")]
        public async Task<IActionResult> GetTrainer()
        {
            var userId = User.GetUserId();
            var trainer = await _userManagementService.GetTrainerAsync(userId);
            if (trainer == null)
                return NotFound("No trainer assigned to the current user.");
            return Ok(trainer);
        }

        [HttpGet]
        public async Task<ActionResult<UserResponse>> GetCurrentUser()
        {
            var userId = User.GetUserId();
            var user = await _userManagementService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UserUpdateRequest request)
        {
            var userId = User.GetUserId();
            var result = await _userManagementService.UpdateUserPropertiesAsync(userId, request);
            if (!result)
                return NotFound();

            return Ok();
        }

        [HttpGet("roles")]
        public async Task<ActionResult<IList<String>>> GetCurrentUserRoles()
        {
            var userId = User.GetUserId();
            var roles = await _userManagementService.GetUserRolesAsync(userId);
            return Ok(roles);
        }

        [HttpPost("roles")]
        public async Task<IActionResult> AssignCurrentUserRoles([FromBody] AssignRolesRequest request)
        {
            var userId = User.GetUserId();
            var result = await _userManagementService.AssignRolesAsync(userId, request.RoleOption);
            if (!result)
                return NotFound();

            return Ok();
        }
    }

}
