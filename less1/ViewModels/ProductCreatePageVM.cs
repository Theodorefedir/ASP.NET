using less1.Models;

namespace less1.ViewModels
{
    public class ProductCreatePageVM
    {
        public ProductCreateVM Product { get; set; } = new();
        public IEnumerable<Category> Categories { get; set; } = [];
    }
}
