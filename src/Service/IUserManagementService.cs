using Data;
using Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IUserManagementService
    {
        Task<bool> AssignRolesAsync(long userId, RoleOption roleOption);
        Task<bool> UpdateUserPropertiesAsync(long userId, UserUpdateRequest request);
        Task<UserResponse?> GetUserByIdAsync(long userId);
        Task<IList<String>> GetUserRolesAsync(long userId);
        Task AssignTrainerAsync(long userId, string trainerUsername);
        Task<ClientResponse?> GetTrainerAsync(long userId);
    }
}
