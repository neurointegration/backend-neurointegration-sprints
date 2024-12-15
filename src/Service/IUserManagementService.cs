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
        Task<bool> AssignRolesAsync(Guid userId, RoleOption roleOption);
        Task<bool> UpdateUserPropertiesAsync(Guid userId, UserUpdateRequest request);
        Task<UserResponse?> GetUserByIdAsync(Guid userId);
        Task<IList<String>> GetUserRolesAsync(Guid userId);
    }
}
