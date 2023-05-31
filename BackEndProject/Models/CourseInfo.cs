namespace BackEndProject.Models;

public class CourseInfo
{
    public int Id { get; set; }
    public DateTime Starts { get; set; }
    public int Duration { get; set; }
    public int ClassDuration { get; set; }
    public string SkillLevel { get; set; } = null!;
    public string Language { get; set; } = null!;
    public int Students { get; set; }
    public string Assesments { get; set; } = null!; 
    public int Fee { get; set; }
}
