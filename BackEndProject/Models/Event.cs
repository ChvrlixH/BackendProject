using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndProject.Models;

public class Event
{
    public int Id { get; set; }
    [Required]
    public string Image { get; set; } = null!;
    [Required]
    public string Day { get; set; } = null!;
    [Required, StringLength(25, MinimumLength = 5)]
    public string Title { get; set; } = null!;
    [Required]
    public string Time { get; set; } = null!;
    [Required]
    public string Venue { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
    [NotMapped]     
    [Required]
    public IFormFile Photo { get; set; }

    public ICollection<EventSpeaker> Speakers { get; set; }
}
