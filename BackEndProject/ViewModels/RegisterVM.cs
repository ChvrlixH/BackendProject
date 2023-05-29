using System.ComponentModel.DataAnnotations;

namespace BackEndProject.ViewModels
{
    public class RegisterVM
    {
        [Required, MaxLength(35)]
        public string? Fullname { get; set; }
        [Required, MaxLength(25)]
        public string? Username { get; set; }
        [Required, MaxLength(256), DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }
    }
}
