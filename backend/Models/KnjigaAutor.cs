using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
	[Table("KnjigaAutor")]
	public class KnjigaAutor
	{

		[JsonIgnore]
		[ForeignKey(nameof(Knjiga))]
        public Guid KnjigaID { get; set; }

		[JsonIgnore]
		public Knjiga Knjiga { get; set; }

		[JsonIgnore]
		[ForeignKey(nameof(Autor))]
		public Guid AutorID { get; set; }

		public Autor Autor { get; set; }
    }
}
