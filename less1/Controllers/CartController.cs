using less1.Extensions;
using less1.Migrations;
using less1.Repositories;
using less1.Services;
using less1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace less1.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly CartRepository _cartRepository;
        private readonly ProductRepository _productRepository;

        public CartController(CartService cartService, CartRepository cartRepository, ProductRepository productRepository)
        {
            _cartService = cartService;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var cartItems = _cartService.GetItems();
            var products = await _productRepository.Products
                .Include(p => p.Category)
                .Where(p => cartItems.Select(ci => ci.ProductId).Contains(p.Id))
                .ToListAsync();

            var viewModels = products
                .Select(p => new CartVM
                {
                    Product = p,
                    Count = cartItems.First(i => i.ProductId == p.Id).Count
                });

            return View(viewModels);
        }

        public async Task<IActionResult> Add(int productId)
        {
            var userId = this.GetUserId();
            if (userId != null)
            {
                await _cartRepository.AddAsync(userId, productId);
            }

            _cartService.Add(productId);


            // Направити користувача туди звідки прийшов
            var referer = Request.Headers.Referer.ToString();

            if (referer != null)
            {
                return Redirect(referer);
            }
            // ==========================================

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Remove(int productId)
        {
            var userId = this.GetUserId();
            if (userId != null)
            {
                await _cartRepository.RemoveAsync(userId, productId);
            }

            _cartService.Remove(productId);

            var referer = Request.Headers.Referer.ToString();

            if (referer != null)
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Clear()
        {
            var userId = this.GetUserId();

            if (userId != null)
            {
                await _cartRepository.ClearAsync(userId);
            }

            _cartService.Clear();

            var referer = Request.Headers.Referer.ToString();

            if (referer != null)
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Increase(int productId)
        {
            _cartService.Increase(productId);
            return RedirectToAction("Index");
        }

        public IActionResult Decrease(int productId)
        {
            _cartService.Decrease(productId);
            return RedirectToAction("Index");
        }
        public IActionResult Checkout()
        {
            _cartService.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
