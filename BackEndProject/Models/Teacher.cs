using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndProject.Models;

public class Teacher
{
    public int Id { get; set; }
    [Required]
    public string Image { get; set; } = null!;
    [Required,StringLength(28, MinimumLength =3)]
    public string Fullname { get; set; } = null!;
    [Required,StringLength(55, MinimumLength =5)]
    public string Profession { get; set; } = null!;
    [Required, StringLength(256, MinimumLength = 5)]
    public string Description { get; set; } = null!;
    [Required, StringLength(55, MinimumLength = 5)]
    public string Degree { get; set; } = null!;
    [Required, StringLength(50, MinimumLength = 5)]
    public string Experience { get; set; } = null!;
    [Required, StringLength(60, MinimumLength = 5)]
    public string Hobbies { get; set; } = null!;
    [Required, StringLength(80, MinimumLength = 5)]
    public string Faculty { get; set; } = null!;
    public bool IsDeleted { get; set; } = false;

    public virtual Skill Skills { get; set; } = null!;
    public virtual SocialMedia SocialMedias { get; set; } = null!;

    [NotMapped]
    [Required]
    public IFormFile Photo { get; set; }



}
