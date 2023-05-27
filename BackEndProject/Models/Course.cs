﻿using System.ComponentModel.DataAnnotations;

namespace BackEndProject.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string Image { get; set; }
        [Required, StringLength(25, MinimumLength = 5)]
        public string Title { get; set; }
        [Required, StringLength(150, MinimumLength = 20)]
        public string Description { get; set; }
    }
}
