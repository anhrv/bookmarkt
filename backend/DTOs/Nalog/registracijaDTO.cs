using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Nalog
{
	public class RegistracijaDTO
	{
		[Required(ErrorMessage = "Nalog mora imati korisnicko ime.")]
		public string KorisnickoIme { get; set; }

		[Required(ErrorMessage = "Nalog mora imati email.")]
		[EmailAddress(ErrorMessage = "Email nije validan.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Nalog mora imati lozinku.")]
		[MinLength(8,ErrorMessage = "Lozinka mora imati minimalno 8 karaktera.")]
		public string Lozinka { get; set; }
    }
}
