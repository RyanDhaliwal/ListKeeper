using System.Security.Claims;

namespace ListKeeper.ApiService.Helpers
{

        /// <summary> 
        /// Helper class for working with current user context 
        /// </summary> 
        public class CurrentUserHelper
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public CurrentUserHelper(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            }
       

        /// <summary> 
        /// Gets the current user's ID from the JWT token claims 
        /// </summary> 
        public int? GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return null;

            if (int.TryParse(userIdClaim, out var userId))
                return userId;

            return null;
        }

        /// <summary> 
        /// Gets the current user's role from the JWT token claims 
        /// </summary> 
        public string? GetCurrentUserRole()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
        }

        /// <summary> 
        /// Checks if the current user is an admin 
        /// </summary> 
        public bool IsCurrentUserAdmin()
        {
            var role = GetCurrentUserRole();
            return string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary> 
        /// Gets the current user's username from the JWT token claims 
        /// </summary> 
        public string? GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}
