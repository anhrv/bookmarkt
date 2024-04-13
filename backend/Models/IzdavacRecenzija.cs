using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.Models
{
	[Table("IzdavacRecenzija")]
	public class IzdavacRecenzija
	{
		[Key]
		public Guid IzdavacRecenzijaID { get; set; }

		[Required(ErrorMessage = "Recenzija mora imati ocjenu.")]
		[Range(0, 10, ErrorMessage = "Ocjena mora biti u rasponu 0-10")]
		public decimal? Ocjena { get; set; }

		public string Tekst { get; set; }

		[ForeignKey(nameof(Izdavac))]
		[JsonIgnore]
		public Guid IzdavacID { get; set; }

		public Izdavac Izdavac { get; set; }

		[ForeignKey(nameof(Nalog))]
		[JsonIgnore]
		public Guid NalogID { get; set; }

		public Nalog Nalog { get; set; }
	}
}
