using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace less1.Extensions
{
    public static class ControllerExtensions
    {
        public static string? GetUserId(this Controller controller)
        {
            if (controller.User.Identity != null && controller.User.Identity.IsAuthenticated)
            {
                var userId = controller.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != null)
                {
                    return userId;
                }
            }

            return null;
        }
    }
}
