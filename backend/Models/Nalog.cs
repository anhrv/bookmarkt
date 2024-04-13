using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
	[Table("Nalog")]
	public class Nalog
	{
        [Key]
        public Guid NalogID { get; set; }

		[Required(ErrorMessage = "Nalog mora imati korisnicko ime.")]
		public string KorisnickoIme { get; set; }

		[Required(ErrorMessage = "Nalog mora imati email.")]
		[EmailAddress(ErrorMessage = "Email nije validan.")]
		public string Email { get; set; }

		[JsonIgnore]
		[Required(ErrorMessage = "Nalog mora imati lozinku.")]
		public string Lozinka { get; set; }

		[Required(ErrorMessage = "Nalog mora imati datum registracije.")]
		public DateTime DatumRegistracije { get; set; }

		public DateTime PosljednjiLogin { get; set; }

		[JsonIgnore]
		public string? ResetLozinkaToken { get; set; }

		[JsonIgnore]
		public DateTime? ResetTokenIstice { get; set; }

		[JsonIgnore]
		public string? TwoFactorCode { get; set; }

		[JsonIgnore]
		public DateTime? TwoFactorExpiration { get; set; }

		[JsonIgnore]
		public int? TwoFactorTries { get; set; }

		public bool TwoFactorEnabled { get; set; }

		[JsonIgnore]
		[ForeignKey(nameof(Uloga))]
		public Guid UlogaID { get; set; }

		public Uloga Uloga { get; set; }
	}
}
