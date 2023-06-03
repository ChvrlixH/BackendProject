using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndProject.Models;

public class SocialMedia
{
    public int Id { get; set; }
    [Required]
    public int TeacherId { get; set; }

    public string Facebook { get; set; }
    public string Pinterest { get; set; }
    public string Twitter { get; set; }
    public string Instagram { get; set; }
    [Required, MaxLength(256)]
    public string Mail { get; set; } = null!;
    [Required]
    public int Number { get; set; }
    [Required]
    public Teacher Teacher { get; set; } = null!;
}
