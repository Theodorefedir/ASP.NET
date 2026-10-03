namespace less1.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public int Count { get; set; } = 1;
    }
}
