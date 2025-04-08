using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using AspNetCore.Yandex.ObjectStorage;
using Data.Repositories;

namespace Service
{
    public class AvatarService
    {
        private readonly IYandexStorageService _objectStoreService;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly string _bucketName;

        public AvatarService(IConfiguration configuration, IYandexStorageService objectStoreService, IUserRepository userRepository)
        {
            _configuration = configuration;
            _bucketName = configuration["YandexObjectStorage:Bucket"] ?? throw new ArgumentNullException("AWS:BucketName");
            _objectStoreService = objectStoreService;
            _userRepository = userRepository;
        }

        public async Task<string> UploadAvatarAsync(long userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file");

            var fileKey = $"avatars/{userId}/{file.FileName}";
            using var stream = file.OpenReadStream();
            var response = await _objectStoreService.ObjectService.PutAsync(stream, fileKey);
            var url = $"https://storage.yandexcloud.net/{_bucketName}/{fileKey}";
            var user = await _userRepository.GetUserByIdAsync(userId);
            user!.PhotoUrl = url;
            await _userRepository.UpsertAsync(user);
            return url;
        }
    }
}
