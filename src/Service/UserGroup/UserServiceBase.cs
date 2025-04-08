using Data;
using Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace Service.UserGroup
{
    public partial class UserService(IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        TokenSettings tokenSettings,
        IConfiguration configuration,
        ILogger<UserService> logger)
    {
        private readonly IUserRepository userRepository = userRepository;
        private readonly TokenSettings _tokenSettings = tokenSettings;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<UserService> _logger = logger;

        private async Task<UserLoginResponse> GenerateUserToken(ApplicationUser user)
        {
            var accessToken = TokenUtil.GenerateAccessToken(_tokenSettings, user);
            var refreshToken = TokenUtil.GenerateRefreshToken();

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(refreshToken));
            var tokenHash = Convert.ToHexString(hashBytes);

            var dbRefreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                TokenHash = tokenHash,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddSeconds(_tokenSettings.RefreshTokenExpireSeconds)
            };
            await refreshTokenRepository.CreateAsync(dbRefreshToken);
            return new UserLoginResponse() { AccessToken = accessToken, RefreshToken = refreshToken };
        }
    }
}
