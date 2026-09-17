using less1.Models;
using less1.Repositories;
using less1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace less1.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductRepository _productRepository;
        private readonly CategoryRepository _categoryRepository;

        public ProductController(ProductRepository productRepository, CategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index(string? category)
        {
            var products = await _productRepository.Products.ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id) {
            var product = await _productRepository.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) {
                return View();
            }
            return View(product);
        }
        
        [HttpGet]
        public async Task<IActionResult> Create() {
            var vm = new ProductCreatePageVM {
                Categories = await _categoryRepository.Categories.ToListAsync()
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreatePageVM pageVm) {
            if (!ModelState.IsValid) {
                pageVm.Categories = await _categoryRepository.Categories.ToListAsync();
                return View(pageVm);
            }

            var result = await _productRepository.CreateAsync(pageVm.Product);

            if (result != null)
            {
                ModelState.AddModelError("Product.Name", result);
                pageVm.Categories = await _categoryRepository.Categories.ToListAsync();
                return View(pageVm);
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id) {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null){
                return RedirectToAction("Index");
            }
            //if(
            var vm = new ProductUpdatePageVM{
                Product = new ProductUpdateVM{
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    CategoryId = product.CategoryId
                },Categories = await _categoryRepository.Categories.ToListAsync()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductUpdatePageVM pageVm) {
            if (!ModelState.IsValid){
                pageVm.Categories = await _categoryRepository.Categories.ToListAsync();
                return View(pageVm);
            }
            var result = await _productRepository.UpdateAsync(pageVm.Product);
            if (result != null){
                ModelState.AddModelError("Product.Name", result);
                pageVm.Categories = await _categoryRepository.Categories.ToListAsync();
                return View(pageVm);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
