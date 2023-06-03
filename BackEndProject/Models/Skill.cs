using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndProject.Models;

public class Skill
{
    public int Id { get; set; }
    [Required]
    public int Language { get; set; }
    [Required]
    public int TeamLeader { get; set; }
    [Required]
    public int Development { get; set; }
    [Required]
    public int Design { get; set; }
    [Required]
    public int Innovation { get; set; }
    [Required]
    public int Communication { get; set; }
    [Required]
    public int TeacherId { get; set; }
    [Required]
    public Teacher Teacher { get; set; } = null!;
}   
