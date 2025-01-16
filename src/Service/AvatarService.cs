using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using AspNetCore.Yandex.ObjectStorage;
using Microsoft.AspNetCore.Identity;
using Data;

namespace Service
{
    public class AvatarService
    {
        private readonly IYandexStorageService _objectStoreService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly string _bucketName;

        public AvatarService(IConfiguration configuration, IYandexStorageService objectStoreService, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _bucketName = configuration["YandexObjectStorage:Bucket"] ?? throw new ArgumentNullException("AWS:BucketName");
            _objectStoreService = objectStoreService;
            _userManager = userManager;
        }

        public async Task<string> UploadAvatarAsync(string userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file");

            var fileKey = $"avatars/{userId}/{file.FileName}";
            using var stream = file.OpenReadStream();
            var response = await _objectStoreService.ObjectService.PutAsync(stream, fileKey);
            var url = $"https://storage.yandexcloud.net/{_bucketName}/{fileKey}";
            var user = await _userManager.FindByIdAsync(userId);
            user!.PhotoUrl = url;
            await _userManager.UpdateAsync(user);
            return url;
        }
    }
}
