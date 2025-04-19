using Data.Repositories;
using Service.Dto;

namespace Service
{
    public class BotTrainerResponse
    {
        public string Username { get; set; } = default!;
    }

    public class UserManagementService : IUserManagementService
    {
        private readonly ITrainerRepository _trainerRepository;
        private readonly IUserRepository _userRepository;

        public UserManagementService(ITrainerRepository trainerRepository, IUserRepository userRepository)
        {
            _trainerRepository = trainerRepository;
            _userRepository = userRepository;
        }

        public Task AssignTrainerAsync(long userId, string trainerUsername)
        {
            throw new NotImplementedException("This method is outdated");
        }

        public async Task<ClientResponse?> GetTrainerAsync(long userId)
        {
            var trainer = await _trainerRepository.GetTrainerAsync(userId);
            if (trainer == null)
                return null;

            return new ClientResponse
            {
                Id = trainer.Id,
                Username = trainer.UserName,
                FirstName = trainer.Name,
                AboutMe = trainer.AboutMe,
                PhotoUrl = trainer.PhotoUrl
            };
        }

        public async Task<bool> AssignRolesAsync(long userId, RoleOption roleOption)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return false;

            if (roleOption == RoleOption.TrainerAndClient)
            {
                user.IAmCoach = true;
            }
            else if (roleOption == RoleOption.ClientOnly)
            {
                user.IAmCoach = false;
            }
            await _userRepository.UpsertAsync(user);

            return true;
        }

        public async Task<bool> UpdateUserPropertiesAsync(long userId, UserUpdateRequest request)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return false;

            user.Name = request.FirstName ?? user.Name;
            user.AboutMe = request.AboutMe ?? user.AboutMe;
            user.IsOnboardingComplete = request.IsOnboardingComplete ?? user.IsOnboardingComplete;
            user.Onboarding = request.Onboarding ?? user.Onboarding;
            user.SprintWeeksCount = request.SprintWeeksCount ?? user.SprintWeeksCount;

            await _userRepository.UpsertAsync(user);
            return true;
        }

        public async Task<UserResponse?> GetUserByIdAsync(long userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
                return null;

            return new UserResponse
            {
                Id = user.Id,
                FirstName = user.Name,
                AboutMe = user.AboutMe,
                PhotoUrl = user.PhotoUrl,
                IsOnboardingComplete = user.IsOnboardingComplete,
                Onboarding = user.Onboarding,
                SprintWeeksCount = user.SprintWeeksCount
            };
        }

        public async Task<IList<String>> GetUserRolesAsync(long userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            var roles = new List<string>();
            roles.Add("Client");
            if (user != null && user.IAmCoach == true)
            {
                roles.Add("Trainer");
            }

            return roles;
        }
    }
}
