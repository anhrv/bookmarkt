using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
	public class UpdateAutorDTO
	{
		[Required(ErrorMessage = "Autor mora imati ime.")]
		public string? Ime { get; set; }

		public string? Prezime { get; set; }

		public DateTime? DatumRodjenja { get; set; }

		public string? Drzava { get; set; }

		public string? Biografija { get; set; }

		[Required(ErrorMessage = "Autor mora imati sliku.")]
		public string? SlikaPutanja { get; set; }
	}
}
