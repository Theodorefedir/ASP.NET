using less1.Models;
using Microsoft.EntityFrameworkCore;

namespace less1.Repositories
{
    public class CartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsInCartAsync(string userId, int productId)
        {
            return await _context.CartItems
                .AnyAsync(ci => ci.UserId == userId && ci.ProductId == productId);
        }

        public async Task<CartItem?> GetItemAsync(string userId, int productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);
        }

        public async Task AddAsync(string userId, int productId)
        {
            if (!await IsInCartAsync(userId, productId))
            {
                var item = new CartItem
                {
                    UserId = userId,
                    ProductId = productId
                };
                _context.Add(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearAsync(string userId)
        {
            var items = _context.CartItems
                .Where(i => i.UserId == userId);

            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
        public async Task<int> CountAsync(string userId)
        {
            return await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .SumAsync(c => c.Count);
        }

        public async Task<List<CartItem>> GetItemsAsync(string userId)
        {
            return await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .ToListAsync();
        }

        public async Task RemoveAsync(string userId, int productId)
        {
            var item = await GetItemAsync(userId, productId);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
