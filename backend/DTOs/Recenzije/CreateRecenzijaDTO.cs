using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Recenzije
{
	public class CreateRecenzijaDTO
	{
		[Required(ErrorMessage = "Recenzija mora imati ocjenu.")]
		[Range(0, 10, ErrorMessage = "Ocjena mora biti u rasponu 0-10")]
		public decimal? Ocjena { get; set; }

		public string Tekst {  get; set; }

		[Required(ErrorMessage = "Recenzija mora pripadati necemu.")]
		public Guid StvarID { get; set; }
	}
}
