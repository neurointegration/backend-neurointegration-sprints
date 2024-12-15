using System.Security.Claims;

namespace Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst("Id")?.Value;
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token.");

            return Guid.Parse(userIdClaim);
        }
    }
}
