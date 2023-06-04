namespace BackEndProject.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<CourseCategory> Courses { get; set; }
}
