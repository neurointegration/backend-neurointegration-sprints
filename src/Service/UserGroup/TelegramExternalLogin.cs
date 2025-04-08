using System.Text;
using System.Security.Cryptography;
using System.Data;
using Data;
using Microsoft.Extensions.Logging;

namespace Service.UserGroup
{
    #nullable disable warnings
    public class TelegramLoginRequest
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? PhotoUrl { get; set; }
        public string AuthDate { get; set; }
        public string Hash { get; set; }
    }

    public partial class UserService
    {
        public async Task<AppResponse<UserLoginResponse>> ExternalTelegramLoginAsync(TelegramLoginRequest loginRequest)
        {
            if (!IsValidTelegramSignature(loginRequest))
                return new AppResponse<UserLoginResponse>().SetErrorResponse("signature", "Invalid Telegram signature");
            var loginTime = int.Parse(loginRequest.AuthDate);
            var now = DateTimeOffset.Now.ToUnixTimeSeconds();
            if ((now - loginTime) > 86400)
            {
                return new AppResponse<UserLoginResponse>().SetErrorResponse("Time", "Data is outdated");
            }
            var telegramUser = await userRepository.GetUserByIdAsync(long.Parse(loginRequest.Id));
            if (telegramUser == null)
            {
                telegramUser = new ApplicationUser
                {
                    Id = long.Parse(loginRequest.Id),
                    Name = loginRequest.FirstName,
                    UserName = loginRequest.UserName,
                    PhotoUrl = loginRequest.PhotoUrl
                };
            }
            else
            {
                telegramUser.UserName = loginRequest.UserName;
            }
            await userRepository.UpsertAsync(telegramUser);
            var token = await GenerateUserToken(telegramUser);
            return new AppResponse<UserLoginResponse>().SetSuccessResponse(token);
        }

        private bool IsValidTelegramSignature(TelegramLoginRequest loginRequest)
        {
            StringBuilder dataStringBuilder = new StringBuilder(256);
            dataStringBuilder.Append("auth_date=");
            dataStringBuilder.Append(loginRequest.AuthDate);
            if (!string.IsNullOrEmpty(loginRequest.FirstName))
            {
                dataStringBuilder.Append("\nfirst_name=");
                dataStringBuilder.Append(loginRequest.FirstName);
            }
            dataStringBuilder.Append("\nid=");
            dataStringBuilder.Append(loginRequest.Id);
            if (!string.IsNullOrEmpty(loginRequest.LastName))
            {
                dataStringBuilder.Append("\nlast_name=");
                dataStringBuilder.Append(loginRequest.LastName);
            }
            if (!string.IsNullOrEmpty(loginRequest.PhotoUrl))
            {
                dataStringBuilder.Append("\nphoto_url=");
                dataStringBuilder.Append(loginRequest.PhotoUrl);
            }
            if (!string.IsNullOrEmpty(loginRequest.UserName))
            {
                dataStringBuilder.Append("\nusername=");
                dataStringBuilder.Append(loginRequest.UserName);
            }

            var botToken = _configuration["Telegram:BotToken"];
            if (string.IsNullOrEmpty(botToken))
            {
                _logger.LogError("Telegram BotToken is missing in configuration.");
                throw new InvalidOperationException("Telegram BotToken is not configured.");
            }
            var secretKey = ShaHash(botToken);
            var myHash = HashHmac(secretKey, Encoding.UTF8.GetBytes(dataStringBuilder.ToString()));
            var myHashStr = String.Concat(myHash.Select(i => i.ToString("x2")));
            if (myHashStr == loginRequest.Hash)
                return true;
            return false;
        }

        private byte[] ShaHash(String value)
        {
            using (var hasher = SHA256.Create())
            { return hasher.ComputeHash(Encoding.UTF8.GetBytes(value)); }
        }

        private byte[] HashHmac(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }
    }
}