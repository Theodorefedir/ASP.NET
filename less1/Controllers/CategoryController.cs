using less1.Models;
using less1.Repositories;
using less1.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace less1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryRepository _repository;
        

        public CategoryController(CategoryRepository repository) { 
            _repository = repository;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categories = _repository.Categories;
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //DateTime? dt = obj as DateTime?;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _repository.CreateAsync(vm);

            if (result != null)
            {
                ModelState.AddModelError("Name", result);
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
            {
                return RedirectToAction("Index");
            }

            var vm = new CategoryUpdateVM
            {
                Id = id,
                Name = category.Name,
                Description = category.Description,
                //Image = category.Image
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _repository.UpdateAsync(vm);

            if (result != null)
            {
                ModelState.AddModelError("Name", result);
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);

            return RedirectToAction("Index");
        }
    }
}
