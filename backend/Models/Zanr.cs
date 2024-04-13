using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
	[Table("Zanr")]
	public class Zanr
	{
		[Key]
        public Guid ZanrID { get; set; }

		[Required(ErrorMessage = "Zanr mora imati naziv.")]
		public string Naziv { get; set; }
    }
}
