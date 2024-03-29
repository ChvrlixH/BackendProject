﻿using System.ComponentModel.DataAnnotations;

namespace BackEndProject.ViewModels
{
    public class LoginVM
    {
        [Required, MaxLength(30)]
        public string? Username { get; set; }
        [Required, DataType(DataType.Password)]
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
