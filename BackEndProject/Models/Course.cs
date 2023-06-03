﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndProject.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string Image { get; set; } = null!;
        [Required, StringLength(25, MinimumLength = 5)]
        public string Title { get; set; } = null!;
        [Required, StringLength(250, MinimumLength = 20)] 
        public string Description { get; set; } = null!;
        public virtual CourseInfo CourseInfo { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }
    }
}
