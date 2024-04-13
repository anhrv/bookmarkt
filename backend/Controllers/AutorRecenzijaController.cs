using backend.Data;
using backend.DTOs.Recenzije;
using backend.Services;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace backend.Controllers
{
	public class autorRecenzijaController : CustomController
	{
		private readonly ApplicationDbContext _db;
		private readonly CurrentUserService _currentUser;

		public autorRecenzijaController(ApplicationDbContext db, CurrentUserService user)
		{
			_db = db;
			_currentUser = user;
		}

		[HttpGet("byAutor/{id}")]
		public async Task<ActionResult<IEnumerable<AutorRecenzija>>> GetAutorRecenzijeByAutorID([FromRoute] Guid id)
		{
			var autorRecenzije = await _db.AutorRecenzija
												.Where(temp => temp.AutorID == id)
												.Include(temp => temp.Nalog)
												.ThenInclude(n => n.Uloga)
												.ToListAsync();
			return Ok(autorRecenzije);
		}

		[HttpGet("byNalog")]
		[Authorize]
		public async Task<ActionResult<IEnumerable<AutorRecenzija>>> GetAutorRecenzijeByNalogID()
		{
			var nalog = await _currentUser.Get();
			if (nalog == null)
			{
				return NotFound("Nalog ne postoji.");
			}

			var autorRecenzije = await _db.AutorRecenzija
												.Where(temp => temp.NalogID == nalog.NalogID)
												.Include(temp => temp.Autor)
												.ThenInclude(a => a.Slika)
												.ToListAsync();
			return Ok(autorRecenzije);
		}


		[HttpGet("{id}")]
		public async Task<ActionResult<AutorRecenzija>> GetAutorRecenzijaByID([FromRoute] Guid id)
		{
			var autorRecenzija = await _db.AutorRecenzija
												.Include(temp => temp.Autor)
												.Include(temp => temp.Nalog)
												.FirstOrDefaultAsync(temp => temp.AutorRecenzijaID == id);

			if (autorRecenzija == null)
			{
				return NotFound("Recenzija ne postoji.");
			}

			return Ok(autorRecenzija);
		}


		[HttpPut("{id}")]
		[Authorize]
		public async Task<ActionResult<AutorRecenzija>> UpdateAutorRecenzija([FromRoute] Guid id, [FromBody] UpdateRecenzijaDTO autorRecenzijaDTO)
		{
			var nalog = await _currentUser.Get();
			if (nalog == null)
			{
				return NotFound("Nalog ne postoji.");
			}

			var postojecaAutorRecenzija = await _db.AutorRecenzija.FindAsync(id);
			if (postojecaAutorRecenzija == null)
			{
				return NotFound("Recenzija ne postoji.");
			}

			if (postojecaAutorRecenzija.NalogID != nalog.NalogID)
			{
				return Unauthorized("Nemate ovaštenja za ovu akciju.");
			}

			postojecaAutorRecenzija.Ocjena = autorRecenzijaDTO.Ocjena ?? postojecaAutorRecenzija.Ocjena;
			postojecaAutorRecenzija.Tekst = autorRecenzijaDTO.Tekst ?? postojecaAutorRecenzija.Tekst;

			await _db.SaveChangesAsync();
			await _db.Entry(postojecaAutorRecenzija)
						.Reference(a => a.Nalog)
						.LoadAsync();
			await _db.Entry(postojecaAutorRecenzija)
						.Reference(a => a.Autor)
						.LoadAsync();
			return Ok(postojecaAutorRecenzija);
		}

		[HttpPost]
		[Authorize]
		public async Task<ActionResult<AutorRecenzija>> CreateAutorRecenzija([FromBody] CreateRecenzijaDTO autorRecenzijaDTO)
		{
			var nalog = await _currentUser.Get();
			if (nalog == null)
			{
				return NotFound("Nalog ne postoji.");
			}

			var autorRecenzija = new AutorRecenzija
			{
				Ocjena = autorRecenzijaDTO.Ocjena,
				Tekst = autorRecenzijaDTO.Tekst,
				AutorID = autorRecenzijaDTO.StvarID,
				NalogID = nalog.NalogID
			};

			_db.AutorRecenzija.Add(autorRecenzija);
			await _db.SaveChangesAsync();
			await _db.Entry(autorRecenzija)
						.Reference(a => a.Nalog)
						.LoadAsync();
			await _db.Entry(autorRecenzija)
						.Reference(a => a.Autor)
						.LoadAsync();
			return CreatedAtAction("GetAutorRecenzijaByID",new { id = autorRecenzija.AutorRecenzijaID },autorRecenzija);
		}

		[HttpDelete("{id}")]
		[Authorize]
		public async Task<IActionResult> DeleteAutorRecenzija([FromRoute] Guid id)
		{
			var nalog = await _currentUser.Get();
			if (nalog == null)
			{
				return NotFound("Nalog ne postoji.");
			}

			var autorRecenzija = await _db.AutorRecenzija.FindAsync(id);
			if (autorRecenzija == null)
			{
				return NotFound("Recenzija ne postoji.");
			}

			if (autorRecenzija.NalogID != nalog.NalogID)
			{
				return Unauthorized("Nemate ovaštenja za ovu akciju.");
			}

			_db.AutorRecenzija.Remove(autorRecenzija);
			await _db.SaveChangesAsync();

			return NoContent();
		}
	}
}
