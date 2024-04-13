using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
	public class ApplicationDbContext : DbContext
	{	
		public ApplicationDbContext()
		{ 		
		}
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Autor> Autor { get; set; }
		public DbSet<Slika> Slika { get; set; }
		public DbSet<Izdavac> Izdavac { get; set; }
		public DbSet<Knjiga> Knjiga { get; set; }
		public DbSet<KnjigaAutor> KnjigaAutor { get; set; }
		public DbSet<Zanr> Zanr { get; set; }
		public DbSet<KnjigaRecenzija> KnjigaRecenzija { get; set; }
		public DbSet<AutorRecenzija> AutorRecenzija { get; set; }
		public DbSet<IzdavacRecenzija> IzdavacRecenzija { get; set; }
		public DbSet<Nalog> Nalog { get; set; }
		public DbSet<Uloga> Uloga { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<KnjigaAutor>().HasKey(ka => new { ka.KnjigaID, ka.AutorID });
		}
	}
}
