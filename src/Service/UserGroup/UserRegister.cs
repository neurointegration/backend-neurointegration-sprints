using Data;
using System.Security.Cryptography;

namespace Service.UserGroup
{
    public class UserRegisterRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
    public partial class UserService
    {

        public async Task<AppResponse<bool>> UserRegisterAsync(UserRegisterRequest request)
        {
            if ((await userRepository.GetUserByUserNameAsync(request.Email)) != null)
            {
                return new AppResponse<bool>().SetErrorResponse("RegistrationError", $"User with username {request.Email} already exists.");
            }
            var user = new ApplicationUser
            {
                Id = Math.Abs(BitConverter.ToInt64(RandomNumberGenerator.GetBytes(8), 0)),
                UserName = request.Email,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await userRepository.UpsertAsync(user);
            return new AppResponse<bool>().SetSuccessResponse(true);
        }
    }
}
