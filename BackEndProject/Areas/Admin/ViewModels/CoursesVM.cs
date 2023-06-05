using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndProject.Areas.Admin.ViewModels
{
    public class CoursesVM
    {
        public int Id { get; set; }
        public Course Course { get; set; }
        public Category Category { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public ICollection<CourseCategory> CourseCategories { get; set; }
        public ICollection<Course> Courses { get; set; }
        [Required]
        public string Image { get; set; }
        [Required, StringLength(85, MinimumLength = 5)]
        public string Title { get; set; }
        [Required, StringLength(850, MinimumLength =10)]
        public string Description { get; set; }
        [Required]
        public DateTime Starts { get; set; }
        [Required,Range(1,48)]
        public int Duration { get; set; }
        [Required,Range(1,12)]
        public int ClassDuration { get; set; }
        [Required]
        public string SkillLevel { get; set; }
        [Required]
        public string Language { get; set; }
        [Required]
        public int Students { get; set; }
        [Required]
        public string Assesments { get; set; }
        [Required]
        public int Fee { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }
    }
}
