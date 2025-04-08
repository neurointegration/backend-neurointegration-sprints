using System.Security.Claims;

namespace Service.UserGroup
{
    public partial class UserService
    {
        public async Task<AppResponse<bool>> UserLogoutAsync(ClaimsPrincipal user)
        {
            if (user.Identity?.IsAuthenticated ?? false)
            {
                var userId = long.Parse(user.Claims.First(x => x.Type == "Id").Value);
                var appUser = userRepository.GetUserByIdAsync(userId);
                if (appUser != null)
                {
                    // TODO Сделать удаление рефреш токена.
                }
                return new AppResponse<bool>().SetSuccessResponse(true);
            }
            return new AppResponse<bool>().SetSuccessResponse(true);
        }
    }
}