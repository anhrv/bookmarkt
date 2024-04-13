using backend.Data;
using backend.DTOs.Nalog;
using backend.Services;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
	public class nalogController : CustomController
	{
		private readonly ApplicationDbContext _db;
		private readonly CurrentUserService _currentUser;

		public nalogController(ApplicationDbContext db, CurrentUserService user)
		{
			_db = db;
			_currentUser = user;
		}

		[HttpGet("me")]
		[Authorize]
		public async Task<ActionResult<Nalog>> GetMe()
		{
			var nalog = await _currentUser.Get();
			if (nalog == null)
			{
				return NotFound("Nalog ne postoji.");
			}
			return Ok(nalog);
		}

		[HttpPut("updateMe")]
		[Authorize]
		public async Task<ActionResult<Nalog>> UpdateMe(UpdateMeDTO nalogDTO)
		{
			var nalog = await _currentUser.Get();

			if(nalog==null)
			{
				return NotFound("Nalog ne postoji.");
			}

			var poruka = "";
			Nalog nalogPoEmailu = null;
			Nalog nalogPoImenu = null;
			if(nalog.Email!=nalogDTO.Email)
			{
				nalogPoEmailu = await _db.Nalog.FirstOrDefaultAsync(temp => temp.Email == nalogDTO.Email);
				if (nalogPoEmailu != null)
					poruka += "Korisnik sa tim emailom već postoji.\n";
			}
			if (nalog.KorisnickoIme != nalogDTO.KorisnickoIme)
			{
				nalogPoImenu = await _db.Nalog.FirstOrDefaultAsync(temp => temp.KorisnickoIme == nalogDTO.KorisnickoIme);
				if (nalogPoImenu != null)
					poruka += "Korisnik sa tim imenom već postoji.\n";
			}
			if (nalogPoEmailu != null || nalogPoImenu != null)
				return BadRequest(poruka);

			nalog.KorisnickoIme = nalogDTO.KorisnickoIme ?? nalog.KorisnickoIme;
			nalog.Email = nalogDTO.Email ?? nalog.Email;
			nalog.TwoFactorEnabled = nalogDTO.TwoFactorEnabled ?? nalog.TwoFactorEnabled;

			await _db.SaveChangesAsync();
			return Ok(nalog);
		}

		[HttpDelete("deleteMe")]
		[Authorize]
		public async Task<IActionResult> DeleteMe(DeleteMeDTO deleteMeDTO)
		{
			var nalog = await _currentUser.Get();

			if (nalog == null)
			{
				return NotFound("Nalog ne postoji.");
			}

			if(!BCrypt.Net.BCrypt.Verify(deleteMeDTO.Lozinka, nalog.Lozinka))
			{
				return BadRequest("Pogrešna lozinka.");
			}

			//var knjigaRecenzije = await _db.KnjigaRecenzija.Where(temp => temp.NalogID == nalog.NalogID).ToListAsync();
			//var autorRecenzije = await _db.AutorRecenzija.Where(temp => temp.NalogID == nalog.NalogID).ToListAsync();
			//var izdavacRecenzije = await _db.IzdavacRecenzija.Where(temp => temp.NalogID == nalog.NalogID).ToListAsync();

			//foreach(var rec in knjigaRecenzije)
			//{
			//	_db.KnjigaRecenzija.Remove(rec);
			//}
			//foreach (var rec in autorRecenzije)
			//{
			//	_db.AutorRecenzija.Remove(rec);
			//}
			//foreach (var rec in izdavacRecenzije)
			//{
			//	_db.IzdavacRecenzija.Remove(rec);
			//}

			_db.Nalog.Remove(nalog);

			await _db.SaveChangesAsync();
			return NoContent();
		}
	}
}
