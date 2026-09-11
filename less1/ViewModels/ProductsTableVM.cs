using less1.Models;
using Microsoft.AspNetCore.Mvc;

namespace less1.ViewModels
{
    public class ProductsTableVM
    {
        public IEnumerable<Product> Products { get; set; } = [];
        public IEnumerable<Category> Categories { get; set; } = [];
    }
}
