using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Nalog
{
	public class UpdateMeDTO
	{
		[Required(ErrorMessage = "Nalog mora imati email.")]
		[EmailAddress(ErrorMessage = "Email nije validan.")]
		public string? Email { get; set; }
		[Required(ErrorMessage = "Nalog mora imati korisnicko ime.")]
		public string? KorisnickoIme { get; set; }
		public bool? TwoFactorEnabled { get; set; }
	}
}
