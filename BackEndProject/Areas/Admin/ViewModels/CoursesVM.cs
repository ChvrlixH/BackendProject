using System.ComponentModel.DataAnnotations;

namespace BackEndProject.Areas.Admin.ViewModels
{
    public class CoursesVM
    {
        public int Id { get; set; }
        [Required]
        public IFormFile? Image { get; set; }
        [Required, StringLength(25, MinimumLength = 5)]
        public string Title { get; set; } 
        [Required, StringLength(150, MinimumLength = 20)]
        public string Description { get; set; }
    }
}
