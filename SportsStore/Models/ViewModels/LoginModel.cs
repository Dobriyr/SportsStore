using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models.ViewModels
{
	public class LoginModel
	{
		[Required]
		public string? Name { get; set; }
		public string? Password { get; set; }
		public string ReturnUrl { get; set; } = "/";
	}
}
