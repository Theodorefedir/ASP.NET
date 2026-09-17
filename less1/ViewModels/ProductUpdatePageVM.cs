using less1.Models;

namespace less1.ViewModels
{
    public class ProductUpdatePageVM
    {
        public ProductUpdateVM Product { get; set; } = new();
        public IEnumerable<Category> Categories { get; set; } = [];
    }
}
