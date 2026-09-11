using less1.Models;
using less1.Services;
using less1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace less1.Repositories
{
    public class CategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _environment;
        private readonly string _imagesPath;
        public IQueryable<Category> Categories => _context.Categories.AsNoTracking();
        public CategoryRepository(AppDbContext context, IWebHostEnvironment environment, ImageService imageService)
        {
            _context = context;
            _environment = environment;
            _imageService = imageService;

            string root = _environment.WebRootPath;
            _imagesPath = Path.Combine(root, "images", "categories");
        }
        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<bool> IsExistsAsync(string name, int id = 0)
        {
            return await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == name.ToLower() && c.Id != id);
        }
        public async Task<string?> CreateAsync(CategoryCreateVM vm)
        {
            bool res = await IsExistsAsync(vm.Name!);

            if (res)
            {
                return $"Category '{vm.Name}' is already exist";
            }

            var model = new Category
            {
                Name = vm.Name!,
                Description = vm.Description
            };

            // save image
            if (vm.Image != null)
            {
                model.Image = await _imageService.SaveImageAsync(vm.Image, _imagesPath);
            }

            await _context.Categories.AddAsync(model);
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<string?> UpdateAsync(CategoryUpdateVM vm)
        {
            bool res = await IsExistsAsync(vm.Name!, vm.Id);

            if (res)
            {
                return $"Категорія '{vm.Name}' вже існує";
            }

            var category = await GetByIdAsync(vm.Id);

            if (category == null)
            {
                return $"Категорія з id '{vm.Id}' не існує";
            }

            category.Description = vm.Description;
            category.Name = vm.Name!;
            //category.Image = vm.Image;

            if (vm.Image != null)
            {
                if (category.Image != null)
                {
                    string imagePath = Path.Combine(_imagesPath, category.Image);
                    _imageService.DeleteImage(imagePath);
                }

                category.Image = await _imageService.SaveImageAsync(vm.Image, _imagesPath);
            }

            await _context.SaveChangesAsync();

            return null;
        }
        public async Task DeleteAsync(int id)
        {
            var category = await GetByIdAsync(id);

            if (category != null)
            {
                if (category.Image != null) {
                    string imagePath = Path.Combine(_imagesPath, category.Image);
                    _imageService.DeleteImage(imagePath);
                }
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
