using System.ComponentModel.DataAnnotations;

namespace less1.ViewModels
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(255, ErrorMessage = "Maximum length is 255 characters")]
        public string? Name { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double Price { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }
    }
}
