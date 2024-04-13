using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
	[Table("Slika")]
	public class Slika
	{
        [Key]
        public Guid SlikaID { get; set; }

		[Required(ErrorMessage = "Slika mora imati putanju.")]
		public string SlikaPutanja { get; set; }
    }
}
