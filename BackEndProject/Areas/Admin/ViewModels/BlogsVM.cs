using System.ComponentModel.DataAnnotations;

namespace BackEndProject.Areas.Admin.ViewModels
{
	public class BlogsVM
	{
		public int Id { get; set; }
		[Required, StringLength(25, MinimumLength = 3)]
		public string Name { get; set; } 
		[Required, StringLength(80, MinimumLength = 5)]
		public string Title { get; set; } 
		[Required]
		public string Description { get; set; } 

		[Required]
		public int Comment { get; set; }
		[Required, StringLength(20, MinimumLength = 9)]
		public string Date { get; set; } 
		[Required]
		public IFormFile? Image { get; set; }
	}
}
