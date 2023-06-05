using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndProject.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string Image { get; set; } = null!;
        [Required, StringLength(85, MinimumLength = 5)]
        public string Title { get; set; } = null!;
        [Required] 
        public string Description { get; set; } = null!;
        [Required]
        public DateTime Starts { get; set; }
        [Required, Range(1,48)]
        public int Duration { get; set; }
        [Required, Range(1,12)]
        public int ClassDuration { get; set; }
        [Required]
        public string SkillLevel { get; set; } = null!;
        [Required]
        public string Language { get; set; } = null!;
        [Required]
        public int Students { get; set; }
        [Required]
        public string Assesments { get; set; } = null!;
        [Required]
        public int Fee { get; set; }
        public ICollection<CourseCategory> Categories { get; set; }

        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }
    }
}
