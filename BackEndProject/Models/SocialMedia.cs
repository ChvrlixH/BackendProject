using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndProject.Models;

public class SocialMedia
{
    public int Id { get; set; }
    [Required, StringLength(20, MinimumLength = 4)]
    public string Name { get; set; } = null!;
    [Required]
    [ForeignKey(nameof(Teacher))]
    public int SocialMediaId { get; set; }
    [Required]
    public Teacher Teacher { get; set; } = null!;
}
