using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndProject.Models;

public class Skill
{
    public int Id { get; set; }
    [Required, StringLength(25, MinimumLength = 3)]
    public string Name { get; set; } = null!;
    [Required, Range(1,100)]
    public int Rate { get; set; }
    [Required]
    [ForeignKey(nameof(Teacher))]
    public int SkillId { get; set; }
    [Required]
    public Teacher Teacher { get; set; } = null!;
}
