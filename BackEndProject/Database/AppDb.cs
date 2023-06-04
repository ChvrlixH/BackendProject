using BackEndProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackEndProject.Database;

public class AppDb : IdentityDbContext<AppUser>
{
	public AppDb(DbContextOptions<AppDb> options) : base(options) { }

	public DbSet<Category> Categories { get; set; } = null!;
	public DbSet<Course> Courses { get; set; } = null!;
	public DbSet<CourseCategory> CoursesCategories { get; set; } = null!;
	public DbSet<Blog> Blogs { get; set; } = null!;
	public DbSet<Teacher> Teachers { get; set; } = null!;
	public DbSet<Skill> Skills { get; set; } = null!;
	public DbSet<SocialMedia> SocialMedias { get; set; } = null!;
	public DbSet<Event> Events { get; set; } = null!;
	public DbSet<Speaker> Speakers { get; set; } = null!;
	public DbSet<EventSpeaker> EventSpeakers { get; set; } = null!;
	//protected override void OnModelCreating(ModelBuilder modelBuilder)
	//{
	//	modelBuilder.Entity<EventSpeaker>().HasKey(es => new { es.EventId, es.SpeakerId });
	//}
}
