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
	public class knjigaRecenzijaController : CustomController
	{
		private readonly ApplicationDbContext _db;
		private readonly CurrentUserService _currentUser;

		public knjigaRecenzijaController(ApplicationDbContext db, CurrentUserService user)
		{
			_db = db;
			_currentUser = user;
		}

		[HttpGet("byKnjiga/{id}")]
		public async Task<ActionResult<IEnumerable<KnjigaRecenzija>>> GetKnjigaRecenzijeByKnjigaID([FromRoute] Guid id)
		{
			var knjigaRecenzije = await _db.KnjigaRecenzija
												.Where(temp => temp.KnjigaID == id)
												.Include(temp => temp.Nalog)
												.ThenInclude(n => n.Uloga)
												.ToListAsync();
			return Ok(knjigaRecenzije);
		}

		[HttpGet("byNalog")]
		[Authorize]
		public async Task<ActionResult<IEnumerable<KnjigaRecenzija>>> GetKnjigaRecenzijeByNalogID([FromRoute] Guid id)
		{
			var nalog = await _currentUser.Get();
			if (nalog == null)
			{
				return NotFound("Nalog ne postoji.");
			}

			var knjigaRecenzije = await _db.KnjigaRecenzija
												.Where(temp => temp.NalogID == nalog.NalogID)
												.Include(temp => temp.Knjiga)
												.Include(temp => temp.Knjiga.Slika)
												.Include(temp => temp.Knjiga.Zanr)
												.Include(temp => temp.Knjiga.Izdavac)
												.Include(temp => temp.Knjiga.Autori)
												.ThenInclude(ka => ka.Autor)
												.ToListAsync();
			return Ok(knjigaRecenzije);
		}


		[HttpGet("{id}")]
		public async Task<ActionResult<KnjigaRecenzija>> GetKnjigaRecenzijaByID([FromRoute] Guid id)
		{
			var knjigaRecenzija = await _db.KnjigaRecenzija
												.Include(temp => temp.Knjiga)
												.Include(temp => temp.Nalog)
												.FirstOrDefaultAsync(temp => temp.KnjigaRecenzijaID == id);

			if (knjigaRecenzija == null)
			{
				return NotFound("Recenzija ne postoji");
			}

			return Ok(knjigaRecenzija);
		}


		[HttpPut("{id}")]
		[Authorize]
		public async Task<ActionResult<KnjigaRecenzija>> UpdateKnjigaRecenzija([FromRoute] Guid id, [FromBody] UpdateRecenzijaDTO knjigaRecenzijaDTO)
		{
			var nalog = await _currentUser.Get();
			if (nalog == null)
			{
				return NotFound("Nalog ne postoji.");
			}

			var postojecaKnjigaRecenzija = await _db.KnjigaRecenzija.FindAsync(id);
			if (postojecaKnjigaRecenzija == null)
			{
				return NotFound("Recenzija ne postoji.");
			}

			if (postojecaKnjigaRecenzija.NalogID != nalog.NalogID)
			{
				return Unauthorized("Nemate ovlaštenja za ovu akciju.");
			}

			postojecaKnjigaRecenzija.Ocjena = knjigaRecenzijaDTO.Ocjena ?? postojecaKnjigaRecenzija.Ocjena;
			postojecaKnjigaRecenzija.Tekst = knjigaRecenzijaDTO.Tekst ?? postojecaKnjigaRecenzija.Tekst;

			await _db.SaveChangesAsync();
			await _db.Entry(postojecaKnjigaRecenzija)
						.Reference(a => a.Nalog)
						.LoadAsync();
			await _db.Entry(postojecaKnjigaRecenzija)
						.Reference(a => a.Knjiga)
						.LoadAsync();
			return Ok(postojecaKnjigaRecenzija);
		}

		[HttpPost]
		[Authorize]
		public async Task<ActionResult<KnjigaRecenzija>> CreateKnjigaRecenzija([FromBody] CreateRecenzijaDTO knjigaRecenzijaDTO)
		{
			var nalog = await _currentUser.Get();
			if (nalog == null)
			{
				return NotFound("Nalog ne postoji.");
			}

			var knjigaRecenzija = new KnjigaRecenzija
			{
				Ocjena = knjigaRecenzijaDTO.Ocjena,
				Tekst = knjigaRecenzijaDTO.Tekst,
				KnjigaID = knjigaRecenzijaDTO.StvarID,
				NalogID = nalog.NalogID
			};

			_db.KnjigaRecenzija.Add(knjigaRecenzija);
			await _db.SaveChangesAsync();
			await _db.Entry(knjigaRecenzija)
						.Reference(a => a.Nalog)
						.LoadAsync();
			await _db.Entry(knjigaRecenzija)
						.Reference(a => a.Knjiga)
						.LoadAsync();
			return CreatedAtAction("GetKnjigaRecenzijaByID", new { id = knjigaRecenzija.KnjigaRecenzijaID }, knjigaRecenzija);
		}

		[HttpDelete("{id}")]
		[Authorize]
		public async Task<IActionResult> DeleteKnjigaRecenzija([FromRoute] Guid id)
		{
			var nalog = await _currentUser.Get();
			if (nalog == null)
			{
				return NotFound("Nalog ne postoji.");
			}

			var knjigaRecenzija = await _db.KnjigaRecenzija.FindAsync(id);
			if (knjigaRecenzija == null)
			{
				return NotFound("Recenzija ne postoji.");
			}

			if (knjigaRecenzija.NalogID != nalog.NalogID)
			{
				return Unauthorized("Nemate ovlaštenja za ovu akciju.");
			}

			_db.KnjigaRecenzija.Remove(knjigaRecenzija);
			await _db.SaveChangesAsync();

			return NoContent();
		}
	}
}
