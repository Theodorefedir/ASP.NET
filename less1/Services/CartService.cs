using less1.Extensions;
using less1.Models;
using less1.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace less1.Services
{
    public class CartService
    {
        private const string _key = "d993c15400936e316a27606dd3dd99469e21cf9cb6865db4309ed1b6d1351210";
        private readonly HttpContext _context;
        private readonly AppDbContext _dbContext;

        public CartService(IHttpContextAccessor accessor, AppDbContext dbContext)
        {
            if (accessor.HttpContext == null)
            {
                throw new ArgumentNullException("Http context is null");
            }

            _context = accessor.HttpContext;
            _dbContext = dbContext;
        }
        
        public List<CartItemVM> GetItems()
        {
            var items = _context.Session.Get<List<CartItemVM>>(_key);

            return items ?? [];
        }

        public bool IsInCart(int productId)
        {
            var items = GetItems();
            return items.Any(i => i.ProductId == productId);
        }

        public int Count()
        {
            var items = GetItems();
            return items.Sum(i => i.Count);
        }

        public async void Add(int productId, int count = 1)
        {
            if (!IsInCart(productId))
            {
                var items = GetItems();
                items.Add(new CartItemVM { ProductId = productId, Count = count });
                _context.Session.Set(_key, items);
            }
        }

        public void Clear()
        {
            _context.Session.Set<List<CartItem>>(_key, []);
        }

        public void Remove(int productId)
        {
            if (IsInCart(productId))
            {
                var items = GetItems();
                items = items.Where(i => i.ProductId != productId).ToList();
                _context.Session.Set(_key, items);
            }
        }

        private void SaveItems(List<CartItemVM> items)
        {
            var json = JsonSerializer.Serialize(items);
            _context.Session.SetString(_key, json);
        }

        public void Increase(int productId)
        {
            var items = GetItems();
            var item = items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) return;

            item.Count++;
            SaveItems(items);
        }

        public void Decrease(int productId)
        {
            var items = GetItems();
            var item = items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) return;

            item.Count--;

            if (item.Count < 1)
                items.Remove(item);

            SaveItems(items);
        }

        public double GetTotal()
        {
            var items = GetItems();
            if (!items.Any()) return 0;

            var ids = items.Select(i => i.ProductId).ToList();
            var products = _dbContext.Products
                .Where(p => ids.Contains(p.Id))
                .ToDictionary(p => p.Id, p => p.Price);

            double total = 0;
            foreach (var item in items)
            {
                if (products.TryGetValue(item.ProductId, out var price))
                    total += price * item.Count;
            }
            return total;
        }
    }
}
