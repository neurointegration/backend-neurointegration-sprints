namespace Service.UserGroup
{
    public class UserLoginRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
    public class UserLoginResponse
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }
    public partial class UserService
    {
        public async Task<AppResponse<UserLoginResponse>> UserLoginAsync(UserLoginRequest request)
        {
            var user = await userRepository.GetUserByUserNameAsync(request.Email);
            if (user == null)
            {
                return new AppResponse<UserLoginResponse>().SetErrorResponse("email", "username not found");
            }
            else
            {
                var isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
                if (isValid)
                {
                    var token = await GenerateUserToken(user);
                    return new AppResponse<UserLoginResponse>().SetSuccessResponse(token);
                }
                else
                {
                    return new AppResponse<UserLoginResponse>().SetErrorResponse("password", "Wrong password");
                }
            }
        }
    }
}
