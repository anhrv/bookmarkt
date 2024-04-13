using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Nalog
{
	public class Login2faDTO
	{
        public string NalogId { get; set; }
        [Required(ErrorMessage = "Morate unijeti kod.")]
        public string TwoFactorCode { get; set; }
    }
}
