using less1.Models;
using Microsoft.EntityFrameworkCore;

namespace less1.Repositories
{
    public class ProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Product> Products => _context.Products.AsNoTracking();
    }
}
