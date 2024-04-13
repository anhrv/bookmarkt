using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Nalog
{
	public class DeleteMeDTO
	{
		[Required(ErrorMessage = "Morate potvrditi lozinku.")]
		public string Lozinka { get; set; }
	}
}
