using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
	[Table("Izdavac")]
	public class Izdavac
	{
		[Key]
		public Guid IzdavacID { get; set; }

		[Required(ErrorMessage = "Izdavac mora imati naziv.")]
		public string Naziv { get; set; }

		[Required(ErrorMessage = "Izdavac mora imati email.")]
		[EmailAddress(ErrorMessage = "Email nije validan.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Izdavac mora imati broj telefona.")]
		public string Telefon { get; set; }

		[Required(ErrorMessage = "Izdavac mora imati adresu.")]
		public string Adresa { get; set; }

		[JsonIgnore]
		[Required(ErrorMessage = "Izdavac mora imati sliku.")]
		[ForeignKey(nameof(Slika))]
		public Guid SlikaID { get; set; }

		public Slika Slika { get; set; }
	}
}
