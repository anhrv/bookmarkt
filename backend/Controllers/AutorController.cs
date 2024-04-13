using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
	public class autorController : CustomController
	{
		private readonly ApplicationDbContext _db;
		private readonly SlikaService _slikaService;

		public autorController(ApplicationDbContext db, SlikaService slikaService)
		{
			_db = db;
			_slikaService = slikaService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Autor>>> GetAutori()
		{
			var autori = await _db.Autor.Include(temp => temp.Slika).ToListAsync();
			return Ok(autori);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Autor>> GetAutorByID([FromRoute] Guid id)
		{
			var autor = await _db.Autor.Include(temp => temp.Slika).FirstOrDefaultAsync(temp => temp.AutorID == id);

			if (autor == null)
			{
				return NotFound("Autor ne postoji.");
			}
			
			return Ok(autor);
		}


		[HttpPut("{id}")]
		[Authorize(Roles = "Uposlenik")]
		public async Task<ActionResult<Autor>> UpdateAutor([FromRoute] Guid id, [FromBody] UpdateAutorDTO autorDTO)
		{
			var postojeciAutor = await _db.Autor.FindAsync(id);
			if (postojeciAutor == null)
			{
				return NotFound("Autor ne postoji.");
			}

			Guid? slikaID = await _slikaService.putanjaToID(autorDTO.SlikaPutanja);

			postojeciAutor.Ime = autorDTO.Ime ?? postojeciAutor.Ime;
			postojeciAutor.Prezime = autorDTO.Prezime ?? postojeciAutor.Prezime;
			postojeciAutor.DatumRodjenja = autorDTO.DatumRodjenja;
			postojeciAutor.Drzava = autorDTO.Drzava ?? postojeciAutor.Drzava;
			postojeciAutor.Biografija = autorDTO.Biografija ?? postojeciAutor.Biografija;
			postojeciAutor.SlikaID = slikaID ?? postojeciAutor.SlikaID;

			await _db.SaveChangesAsync();
			await _db.Entry(postojeciAutor)
						.Reference(a => a.Slika)
						.LoadAsync();
			return Ok(postojeciAutor);
		}

		[HttpPost]
		[Authorize(Roles = "Uposlenik")]
		public async Task<ActionResult<Autor>> CreateAutor([FromBody] CreateAutorDTO autorDTO)
		{
			var autor = new Autor
			{
				Ime = autorDTO.Ime,
				Prezime = autorDTO.Prezime,
				DatumRodjenja = autorDTO.DatumRodjenja,
				Drzava = autorDTO.Drzava,
				Biografija = autorDTO.Biografija,
				SlikaID = await _slikaService.putanjaToID(autorDTO.SlikaPutanja)
			};

			_db.Autor.Add(autor);
			await _db.SaveChangesAsync();
			await _db.Entry(autor)
						.Reference(a => a.Slika)
						.LoadAsync();
			return CreatedAtAction("GetAutorByID",new {id=autor.AutorID},autor);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Uposlenik")]
		public async Task<IActionResult> DeleteAutor([FromRoute] Guid id)
		{
			var autor = await _db.Autor.Include(temp=>temp.Slika).FirstOrDefaultAsync(temp=>temp.AutorID==id);
			if (autor == null)
			{
				return NotFound("Autor ne postoji.");
			}

			var slika = await _db.Slika.FirstOrDefaultAsync(temp => (temp.SlikaPutanja == autor.Slika.SlikaPutanja && temp.SlikaPutanja != "/assets/images/default.jpg"));
			if (slika != null)
			{
				_db.Slika.Remove(slika);
			}

			//var autorRecenzije = await _db.AutorRecenzija.Where(temp => temp.AutorID == autor.AutorID).ToListAsync();

			//foreach (var rec in autorRecenzije)
			//{
			//	_db.AutorRecenzija.Remove(rec);
			//}

			//var knjigeAutori = await _db.KnjigaAutor.Where(ka => ka.AutorID == autor.AutorID).ToListAsync();
			//foreach (var knjigaAutor in knjigeAutori)
			//{
			//	_db.KnjigaAutor.Remove(knjigaAutor);
			//}

			_db.Autor.Remove(autor);
			await _db.SaveChangesAsync();

			return NoContent();
		}
	}
}
