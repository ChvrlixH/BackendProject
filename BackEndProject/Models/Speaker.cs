using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndProject.Models;

public class Speaker
{
    public int Id { get; set; }
    [Required]
    public string Image { get; set; }
    [Required, StringLength(30, MinimumLength = 3)]
    public string Fullname { get; set; } = null!;
    [Required, StringLength(40, MinimumLength = 3)]
    public string Profession { get; set; } = null!;
    [NotMapped]
    [Required]
    public IFormFile Photo { get; set; }
    public ICollection<EventSpeaker> Events { get; set;}
}
