using System.ComponentModel.DataAnnotations;

namespace BackEndProject.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required, DataType(DataType.EmailAddress), MaxLength(256)]
        public string Email { get; set; }
    }
}
