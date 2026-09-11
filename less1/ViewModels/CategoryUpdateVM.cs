using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace less1.ViewModels
{
    public class CategoryUpdateVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва є обов'язковою")]
        [MaxLength(100, ErrorMessage = "Максимальна довжина 100 символів")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}
