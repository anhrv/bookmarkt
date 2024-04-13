using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Nalog
{
	public class ForgotLozinkaDTO
	{
		//[Required(ErrorMessage = "Morate unijeti email.")]
		//[EmailAddress(ErrorMessage = "Email nije validan.")]
        public string Email { get; set; }
    }
}
