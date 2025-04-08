using System.Security.Cryptography;
using System.Text;

namespace Service.UserGroup
{
    public class UserRefreshTokenRequest
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }
    public class UserRefreshTokenResponse
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }
    public partial class UserService
    {
        public async Task<AppResponse<UserRefreshTokenResponse>> UserRefreshTokenAsync(UserRefreshTokenRequest request)
        {
            var principal = TokenUtil.GetPrincipalFromExpiredToken(_tokenSettings, request.AccessToken);
            if (principal == null)
            {
                return new AppResponse<UserRefreshTokenResponse>().SetErrorResponse("id", "User not found");
            }

            var userId = principal.FindFirst("Id")?.Value;
            if (userId == null)
            {
                return new AppResponse<UserRefreshTokenResponse>().SetErrorResponse("id", "User not found");
            }
            else
            {
                var user = await userRepository.GetUserByIdAsync(long.Parse(userId));
                if (user == null)
                {
                    return new AppResponse<UserRefreshTokenResponse>().SetErrorResponse("id", "User not found");
                }
                else
                {
                    using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey));
                    var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.RefreshToken));
                    var tokenHash = Convert.ToHexString(hashBytes);
                    if (!await refreshTokenRepository.IsOkAsync(tokenHash, long.Parse(userId)))
                    {
                        return new AppResponse<UserRefreshTokenResponse>().SetErrorResponse("token", "Refresh token expired");
                    }
                    var token = await GenerateUserToken(user);
                    return new AppResponse<UserRefreshTokenResponse>().SetSuccessResponse(new UserRefreshTokenResponse() { AccessToken = token.AccessToken, RefreshToken = token.RefreshToken });
                }
            }
        }
    }
}
