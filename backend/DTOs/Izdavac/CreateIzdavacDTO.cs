using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
	public class CreateIzdavacDTO
	{
		[Required(ErrorMessage = "Izdavac mora imati naziv.")]
		public string Naziv { get; set; }

		[Required(ErrorMessage = "Izdavac mora imati email.")]
		[EmailAddress(ErrorMessage = "Email nije validan.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Izdavac mora imati broj telefona.")]
		public string Telefon { get; set; }

		[Required(ErrorMessage = "Izdavac mora imati adresu.")]
		public string Adresa { get; set; }

		[Required(ErrorMessage = "Izdavac mora imati sliku.")]
		public string SlikaPutanja { get; set; }
	}
}
