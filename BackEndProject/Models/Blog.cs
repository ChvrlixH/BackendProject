using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndProject.Models
{
	public class Blog
	{
		public int Id { get; set; }
		[Required, StringLength(25, MinimumLength = 3)]
		public string Name { get; set; } = null!;
		[Required, StringLength(80, MinimumLength = 5)]
		public string Title { get; set; } = null!;
		[Required]
		public string Description { get; set; } = null!;

		[Required, MaxLength(250)]
		public int Comment { get; set; }
		[Required, StringLength(20, MinimumLength = 9)]
		public string Date { get; set; } = null!;
		[Required]
		public string Image { get; set; } = null!;
	}
}
