using System.ComponentModel.DataAnnotations;

namespace BackEndProject.Areas.Admin.ViewModels;

public class TeacherVM
{
    public int Id { get; set; }
    [Required]
    public string Image { get; set; } 
    [Required, StringLength(28, MinimumLength = 3)]
    public string Name { get; set; } 
    [Required, StringLength(55, MinimumLength = 5)]
    public string Profession { get; set; } 
    [Required, StringLength(256, MinimumLength = 5)]
    public string Description { get; set; } 
    [Required, StringLength(55, MinimumLength = 5)]
    public string Degree { get; set; } 
    [Required, StringLength(50, MinimumLength = 5)]
    public string Experience { get; set; } 
    [Required, StringLength(60, MinimumLength = 5)]
    public string Hobbies { get; set; } 
    [Required, StringLength(80, MinimumLength = 5)]
    public string Faculty { get; set; } 
    [Required, MaxLength(256)]
    public string Mail { get; set; } 
    [Required]
    public int Number { get; set; }
    [Required]
    public int SkillId { get; set; }
    [Required]
    public int SocialMediaId { get; set; }

    public ICollection<Skill> Skills { get; set; } = null!;
    public ICollection<SocialMedia> SocialMedias { get; set; } = null!;


}
