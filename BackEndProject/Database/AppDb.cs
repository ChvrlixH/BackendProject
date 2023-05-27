using BackEndProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndProject.Database;

public class AppDb : DbContext
{
	public AppDb(DbContextOptions<AppDb> options) : base(options) { }

	public DbSet<CourseInfo> CoursesInfo { get; set; } = null!;
	public DbSet<Course> Courses { get; set; } = null!;

}
