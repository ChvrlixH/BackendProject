namespace BackEndProject.ViewModels;

public class HomeVM
{
    public IEnumerable<Course> Courses { get; set; }
    public IEnumerable<Event> Events { get; set; }
    public IEnumerable<Blog> Blogs { get; set; }
}
