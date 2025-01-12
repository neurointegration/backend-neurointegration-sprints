using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Dto;
using Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BotTrainerResponse
    {
        public string Username { get; set; } = default!;
    }

    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserManagementService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task AssignTrainerAsync(Guid userId, string trainerUsername)
        {
            var trainer = await _userManager.FindByNameAsync(trainerUsername.Substring(1));
            if (trainer == null)
            {
                throw new TrainerNotFoundException($"Trainer with username '{trainerUsername.Substring(1)}' is not registered.");
            }

            var botTrainersUrl = _configuration["BotIntegration:TrainersUrl"];
            if (string.IsNullOrEmpty(botTrainersUrl))
            {
                throw new InvalidOperationException("BotIntegration:TrainersUrl is not configured.");
            }

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(botTrainersUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch trainers from bot.");
            }

            var botTrainers = await response.Content.ReadFromJsonAsync<List<BotTrainerResponse>>();
            if (botTrainers == null || !botTrainers.Any(bt => bt.Username == trainerUsername))
            {
                throw new TrainerNotInBotException($"Trainer with username '{trainerUsername}' is not listed in the bot.");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new UserNotFoundException($"User with ID '{userId}' is not found.");
            }

            user.TrainerId = trainer.Id;
            await _userManager.UpdateAsync(user);
        }

        public async Task<TrainerResponse?> GetTrainerAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new UserNotFoundException("User not found.");

            if (user.TrainerId == null)
                return null;

            var trainer = await _userManager.FindByIdAsync(user.TrainerId.ToString()!);
            if (trainer == null)
                return null;

            return new TrainerResponse
            {
                Id = trainer.Id,
                Username = trainer.UserName,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                AboutMe = trainer.AboutMe
            };
        }

        public async Task<bool> AssignRolesAsync(Guid userId, RoleOption roleOption)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return false;

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (roleOption == RoleOption.TrainerAndClient)
            {
                await _userManager.AddToRoleAsync(user, "Client");
                await _userManager.AddToRoleAsync(user, "Trainer");
            }
            else if (roleOption == RoleOption.ClientOnly)
            {
                await _userManager.AddToRoleAsync(user, "Client");
            }

            return true;
        }

        public async Task<bool> UpdateUserPropertiesAsync(Guid userId, UserUpdateRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return false;

            user.FirstName = request.FirstName ?? user.FirstName;
            user.LastName = request.LastName ?? user.LastName;
            user.AboutMe = request.AboutMe ?? user.AboutMe;
            user.IsOnboardingComplete = request.IsOnboardingComplete ?? user.IsOnboardingComplete;
            user.SprintWeeksCount = request.SprintWeeksCount ?? user.SprintWeeksCount;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<UserResponse?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return null;

            return new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AboutMe = user.AboutMe,
                PhotoUrl = user.PhotoUrl,
                IsOnboardingComplete = user.IsOnboardingComplete,
                SprintWeeksCount = user.SprintWeeksCount
            };
        }

        public async Task<IList<String>> GetUserRolesAsync(Guid userId)
        {
            var roles = await _userManager.GetRolesAsync(new ApplicationUser { Id = userId });
            return roles;
        }
    }
}
