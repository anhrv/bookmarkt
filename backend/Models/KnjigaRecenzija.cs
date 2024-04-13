using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
	[Table("KnjigaRecenzija")]
	public class KnjigaRecenzija
	{
        [Key]
        public Guid KnjigaRecenzijaID { get; set; }

		[Required(ErrorMessage = "Recenzija mora imati ocjenu.")]
		[Range(0,10,ErrorMessage = "Ocjena mora biti u rasponu 0-10")]
		public decimal? Ocjena { get; set; }

        public string Tekst {  get; set; }

        [ForeignKey(nameof(Knjiga))]
		[JsonIgnore]
		public Guid KnjigaID { get; set; }

        public Knjiga Knjiga { get; set; }

        [ForeignKey(nameof(Nalog))]
		[JsonIgnore]
		public Guid NalogID { get; set; }

        public Nalog Nalog { get; set; }
    }
}
