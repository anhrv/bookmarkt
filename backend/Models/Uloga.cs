using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
	[Table("Uloga")]
	public class Uloga
	{
        [Key]
        public Guid UlogaID { get; set; }

		[Required(ErrorMessage = "Uloga mora imati naziv.")]
		public string Naziv { get; set; }
    }
}
