using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndProject.Areas.Admin.ViewModels;

public class EventVM
{
    public Event Event { get; set; }
    public Speaker Speaker { get; set; }
    public IEnumerable<Speaker> Speakers { get; set; }
    public ICollection<EventSpeaker> EventSpeakers { get; set; }
    public ICollection<Event> Events { get; set; }

    public string Image { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string Day { get; set; }
    [Required]
    public string Time { get; set; }
    [Required]
    public string Venue { get; set; }
    [NotMapped]
    [Required]
    public IFormFile Photo { get; set; }
}
