using Microsoft.AspNetCore.Identity;

namespace less1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }

        public List<CartItem> CartItems { get; set; } = [];
    }
}
