using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
	[Table("Autor")]
	public class Autor
	{
		[Key]
        public Guid AutorID { get; set; }

		[Required(ErrorMessage = "Autor mora imati ime.")]
		public string Ime { get; set; }

		public string Prezime { get; set; }

		public DateTime? DatumRodjenja { get; set; }

		public string Drzava { get; set; }

		public string Biografija { get; set; }

		[JsonIgnore]
		[ForeignKey(nameof(Slika))]
		public Guid SlikaID { get; set; }

		public Slika Slika { get; set; }
	}
}
