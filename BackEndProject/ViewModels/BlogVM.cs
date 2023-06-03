namespace BackEndProject.ViewModels;

public class BlogVM
{
    public int Id { get; set; }
    public List<Blog> blog { get; set; }

    public virtual Blog blogs { get; set; }
}
