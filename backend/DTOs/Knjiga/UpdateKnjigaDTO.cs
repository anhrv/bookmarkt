using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Knjiga
{
	public class UpdateKnjigaDTO
	{
		[Required(ErrorMessage = "Knjiga mora imati naslov.")]
		public string? Naslov { get; set; }

		[Required(ErrorMessage = "Knjiga mora imati ISBN.")]
		[RegularExpression("[0-9]*[-| ][0-9]*[-| ][0-9]*[-| ][0-9]*[-| ][0-9]*", ErrorMessage = "ISBN nije validan")]
		public string? ISBN { get; set; }

		public string? Opis { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Minimalan broj stranica je 1.")]
		public int? BrojStranica { get; set; }

		[Required(ErrorMessage = "Knjiga mora imati cijenu.")]
		[Range(0, int.MaxValue, ErrorMessage = "Minimalna cijena je 0.")]
		public decimal? Cijena { get; set; }

		public DateTime? DatumIzdavanja { get; set; }

		[Required(ErrorMessage = "Knjiga mora imati broj na stanju.")]
		[Range(0, int.MaxValue, ErrorMessage = "Minimalan broj na stanju je 0.")]
		public int? NaStanju { get; set; }

		[Required(ErrorMessage = "Knjiga mora imati sliku.")]
		public string? SlikaPutanja { get; set; }

		public Guid? IzdavacID { get; set; }

		public Guid? ZanrID { get; set; }

		public List<Guid>? AutorIDs { get; set; }
	}
}
