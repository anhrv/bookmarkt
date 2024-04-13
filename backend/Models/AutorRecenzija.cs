using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.Models
{
	[Table("AutorRecenzija")]
	public class AutorRecenzija
	{
		[Key]
		public Guid AutorRecenzijaID { get; set; }

		[Required(ErrorMessage = "Recenzija mora imati ocjenu.")]
		[Range(0, 10, ErrorMessage = "Ocjena mora biti u rasponu 0-10")]
		public decimal? Ocjena { get; set; }

		public string Tekst { get; set; }

		[ForeignKey(nameof(Autor))]
		[JsonIgnore]
		public Guid AutorID { get; set; }

		public Autor Autor { get; set; }

		[ForeignKey(nameof(Nalog))]
		[JsonIgnore]
		public Guid NalogID { get; set; }

		public Nalog Nalog { get; set; }
	}
}
