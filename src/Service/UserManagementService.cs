using Data;
using Microsoft.AspNetCore.Identity;
using Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagementService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
