using less1.Models;
using less1.Services;
using less1.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace less1.Repositories
{
    public class ProductRepository
    {
        private readonly AppDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _environment;
        private readonly string _imagesPath;

        public ProductRepository(AppDbContext context, IWebHostEnvironment environment, ImageService imageService){           _context = context;
            _environment = environment;
            _imageService = imageService;
            string root = _environment.WebRootPath;
            _imagesPath = Path.Combine(root, "images", "products");
        }
        public IQueryable<Product> Products => _context.Products.AsNoTracking();
        public async Task<Product?> GetByIdAsync(int id) {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<bool> IsExistsAsync(string name, int id = 0)
        {
            return await _context.Products
                .AnyAsync(p => p.Name.ToLower() == name.ToLower() && p.Id != id);
        }
        public async Task<string?> CreateAsync(ProductCreateVM vm)
        {
            bool exists = await IsExistsAsync(vm.Name!);
            if (exists)
            {
                return $"Product '{vm.Name}' already exists";
            }

            var model = new Product
            {
                Name = vm.Name!,
                Price = vm.Price,
                Description = vm.Description,
                CategoryId = vm.CategoryId
            };

            if (vm.Image != null)
            {
                model.Image = await _imageService.SaveImageAsync(vm.Image, _imagesPath);
            }

            await _context.Products.AddAsync(model);
            await _context.SaveChangesAsync();

            return null;
        }
        public async Task<string?> UpdateAsync(ProductUpdateVM vm)
        {
            bool exists = await IsExistsAsync(vm.Name!, vm.Id);
            if (exists)
            {
                return $"Product '{vm.Name}' already exists";
            }

            var product = await GetByIdAsync(vm.Id);
            if (product == null)
            {
                return $"Product with id '{vm.Id}' does not exist";
            }

            product.Name = vm.Name!;
            product.Price = vm.Price;
            product.Description = vm.Description;
            product.CategoryId = vm.CategoryId;

            if (vm.Image != null)
            {
                if (product.Image != null)
                {
                    string imagePath = Path.Combine(_imagesPath, product.Image);
                    _imageService.DeleteImage(imagePath);
                }

                product.Image = await _imageService.SaveImageAsync(vm.Image, _imagesPath);
            }

            await _context.SaveChangesAsync();
            return null;
        }
        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);

            if (product != null)
            {
                if (product.Image != null)
                {
                    string imagePath = Path.Combine(_imagesPath, product.Image);
                    _imageService.DeleteImage(imagePath);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
