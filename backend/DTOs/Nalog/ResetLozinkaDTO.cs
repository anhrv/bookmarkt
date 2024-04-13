using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Nalog
{
	public class ResetLozinkaDTO
	{
		[Required(ErrorMessage = "Morate unijeti novu lozinku.")]
		[MinLength(8, ErrorMessage = "Lozinka mora imati minimalno 8 karaktera.")]
		public string NovaLozinka { get; set; }

		[Required(ErrorMessage = "Morate unijeti potvrdu nove lozinke.")]
		[Compare("NovaLozinka", ErrorMessage = "Niste tačno potvrdili novu lozinku.")]
		public string NovaLozinkaPotvrda { get; set; }
	}
}
