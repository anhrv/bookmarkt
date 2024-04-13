using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Nalog
{
	public class LoginDTO
	{
		//[Required(ErrorMessage = "Nalog mora imati email.")]
		//[EmailAddress(ErrorMessage = "Email nije u validnom formatu.")]
		public string Email { get; set; }

		//[Required(ErrorMessage = "Nalog mora imati lozinku.")]
		public string Lozinka { get; set; }
    }
}
