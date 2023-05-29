using BackEndProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackEndProject.Database;

public class AppDb : IdentityDbContext<AppUser>
{
	public AppDb(DbContextOptions<AppDb> options) : base(options) { }

	public DbSet<CourseInfo> CoursesInfo { get; set; } = null!;
	public DbSet<Course> Courses { get; set; } = null!;

}
