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
	public class izdavacRecenzijaController : CustomController
	{
		private readonly ApplicationDbContext _db;
		private readonly CurrentUserService _currentUser;

		public izdavacRecenzijaController(ApplicationDbContext db, CurrentUserService user)
		{
			_db = db;
			_currentUser = user;
		}

		[HttpGet("byIzdavac/{id}")]
		public async Task<ActionResult<IEnumerable<IzdavacRecenzija>>> GetIzdavacRecenzijeByIzdavacID([FromRoute] Guid id)
		{
			var izdavacRecenzije = await _db.IzdavacRecenzija
												.Where(temp => temp.IzdavacID == id)
												.Include(temp => temp.Nalog)
												.ThenInclude(n => n.Uloga)
												.ToListAsync();
			return Ok(izdavacRecenzije);
		}

		[HttpGet("byNalog")]
		[Authorize]
		public async Task<ActionResult<IEnumerable<IzdavacRecenzija>>> GetIzdavacRecenzijeByNalogID([FromRoute] Guid id)
		{
			var nalog = await _currentUser.Get();
			if (nalog == null)
			{
				return NotFound("Nalog ne postoji.");
			}

			var izdavacRecenzije = await _db.IzdavacRecenzija
												.Where(temp => temp.NalogID == nalog.NalogID)
												.Include(temp => temp.Izdavac)
												.ThenInclude(a => a.Slika)
												.ToListAsync();
			return Ok(izdavacRecenzije);
		}


		[HttpGet("{id}")]
		public async Task<ActionResult<IzdavacRecenzija>> GetIzdavacRecenzijaByID([FromRoute] Guid id)
		{
			var izdavacRecenzija = await _db.IzdavacRecenzija
												.Include(temp => temp.Izdavac)
												.Include(temp => temp.Nalog)
												.FirstOrDefaultAsync(temp => temp.IzdavacRecenzijaID == id);

			if (izdavacRecenzija == null)
			{
				return NotFound("Recenzija ne postoji.");
			}

			return Ok(izdavacRecenzija);
		}


		[HttpPut("{id}")]
		[Authorize]
		public async Task<ActionResult<IzdavacRecenzija>> UpdateIzdavacRecenzija([FromRoute] Guid id, [FromBody] UpdateRecenzijaDTO izdavacRecenzijaDTO)
		{
			var nalog = await _currentUser.Get();
			if (nalog == null)
			{
				return NotFound("Nalog ne postoji.");
			}

			var postojecaIzdavacRecenzija = await _db.IzdavacRecenzija.FindAsync(id);
			if (postojecaIzdavacRecenzija == null)
			{
				return NotFound("Recenzija ne postoji.");
			}

			if (postojecaIzdavacRecenzija.NalogID != nalog.NalogID)
			{
				return Unauthorized("Nemate ovlaštenja za ovu akciju.");
			}

			postojecaIzdavacRecenzija.Ocjena = izdavacRecenzijaDTO.Ocjena ?? postojecaIzdavacRecenzija.Ocjena;
			postojecaIzdavacRecenzija.Tekst = izdavacRecenzijaDTO.Tekst ?? postojecaIzdavacRecenzija.Tekst;

			await _db.SaveChangesAsync();
			await _db.Entry(postojecaIzdavacRecenzija)
						.Reference(a => a.Nalog)
						.LoadAsync();
			await _db.Entry(postojecaIzdavacRecenzija)
						.Reference(a => a.Izdavac)
						.LoadAsync();
			return Ok(postojecaIzdavacRecenzija);
		}

		[HttpPost]
		[Authorize]
		public async Task<ActionResult<IzdavacRecenzija>> CreateIzdavacRecenzija([FromBody] CreateRecenzijaDTO izdavacRecenzijaDTO)
		{
			var nalog = await _currentUser.Get();
			if (nalog == null)
			{
				return NotFound("Nalog ne postoji.");
			}

			var izdavacRecenzija = new IzdavacRecenzija
			{
				Ocjena = izdavacRecenzijaDTO.Ocjena,
				Tekst = izdavacRecenzijaDTO.Tekst,
				IzdavacID = izdavacRecenzijaDTO.StvarID,
				NalogID = nalog.NalogID
			};

			_db.IzdavacRecenzija.Add(izdavacRecenzija);
			await _db.SaveChangesAsync();
			await _db.Entry(izdavacRecenzija)
						.Reference(a => a.Nalog)
						.LoadAsync();
			await _db.Entry(izdavacRecenzija)
						.Reference(a => a.Izdavac)
						.LoadAsync();
			return CreatedAtAction("GetIzdavacRecenzijaByID",new { id = izdavacRecenzija.IzdavacRecenzijaID }, izdavacRecenzija);
		}

		[HttpDelete("{id}")]
		[Authorize]
		public async Task<IActionResult> DeleteIzdavacRecenzija([FromRoute] Guid id)
		{
			var nalog = await _currentUser.Get();
			if (nalog == null)
			{
				return NotFound("Nalog ne postoji.");
			}

			var izdavacRecenzija = await _db.IzdavacRecenzija.FindAsync(id);
			if (izdavacRecenzija == null)
			{
				return NotFound("Recenzija ne postoji.");
			}

			if(izdavacRecenzija.NalogID != nalog.NalogID)
			{
				return Unauthorized("Nemate ovlaštenja za ovu akciju.");
			}

			_db.IzdavacRecenzija.Remove(izdavacRecenzija);
			await _db.SaveChangesAsync();

			return NoContent();
		}
	}
}
