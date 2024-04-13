using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Nalog
{
	public class UpdateLozinkaDTO
	{
        [Required(ErrorMessage = "Morate unijeti trenutnu lozinku.")]
        public string TrenutnaLozinka { get; set; }

		[Required(ErrorMessage = "Morate unijeti novu lozinku.")]
		[MinLength(8, ErrorMessage = "Lozinka mora imati minimalno 8 karaktera.")]
		public string NovaLozinka { get; set; }

		[Required(ErrorMessage = "Morate unijeti potvrdu nove lozinke.")]
		[Compare("NovaLozinka",ErrorMessage = "Niste potvrdili novu lozinku.")]
		public string NovaLozinkaPotvrda { get; set; }
    }
}
