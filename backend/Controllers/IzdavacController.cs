using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    public class izdavacController : CustomController
	{
		private readonly ApplicationDbContext _db;
		private readonly SlikaService _slikaService;

		public izdavacController(ApplicationDbContext db, SlikaService slikaService)
		{
			_db = db;
			_slikaService = slikaService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Izdavac>>> GetIzdavaci()
		{
			var izdavaci = await _db.Izdavac.Include(temp => temp.Slika).ToListAsync();
			return Ok(izdavaci);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Izdavac>> GetIzdavacByID([FromRoute] Guid id)
		{
			var izdavac = await _db.Izdavac.Include(temp => temp.Slika).FirstOrDefaultAsync(temp => temp.IzdavacID == id);

			if (izdavac == null)
			{
				return NotFound("Izdavac ne postoji.");
			}

			return Ok(izdavac);
		}


		[HttpPut("{id}")]
		[Authorize(Roles = "Uposlenik")]
		public async Task<ActionResult<Izdavac>> UpdateIzdavac([FromRoute] Guid id, [FromBody] UpdateIzdavacDTO izdavacDTO)
		{
			var postojeciIzdavac = await _db.Izdavac.FindAsync(id);
			if (postojeciIzdavac == null)
			{
				return NotFound("Izdavac ne postoji");
			}

			Guid? slikaID = await _slikaService.putanjaToID(izdavacDTO.SlikaPutanja);

			postojeciIzdavac.Naziv = izdavacDTO.Naziv ?? postojeciIzdavac.Naziv;
			postojeciIzdavac.Email = izdavacDTO.Email ?? postojeciIzdavac.Email;
			postojeciIzdavac.Telefon = izdavacDTO.Telefon ?? postojeciIzdavac.Telefon;
			postojeciIzdavac.Adresa = izdavacDTO.Adresa ?? postojeciIzdavac.Adresa;
			postojeciIzdavac.SlikaID = slikaID ?? postojeciIzdavac.SlikaID;

			await _db.SaveChangesAsync();
			await _db.Entry(postojeciIzdavac)
						.Reference(a => a.Slika)
						.LoadAsync();
			return Ok(postojeciIzdavac);
		}

		[HttpPost]
		[Authorize(Roles = "Uposlenik")]
		public async Task<ActionResult<Izdavac>> CreateIzdavac([FromBody] CreateIzdavacDTO izdavacDTO)
		{
			var izdavac = new Izdavac
			{
				Naziv = izdavacDTO.Naziv,
				Email = izdavacDTO.Email,
				Telefon = izdavacDTO.Telefon,
				Adresa = izdavacDTO.Adresa,
				SlikaID = await _slikaService.putanjaToID(izdavacDTO.SlikaPutanja)
			};

			_db.Izdavac.Add(izdavac);
			await _db.SaveChangesAsync();
			await _db.Entry(izdavac)
						.Reference(a => a.Slika)
						.LoadAsync();
			return CreatedAtAction("GetIzdavacByID",new { id = izdavac.IzdavacID }, izdavac);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Uposlenik")]
		public async Task<IActionResult> DeleteIzdavac([FromRoute] Guid id)
		{
			var izdavac = await _db.Izdavac.Include(temp => temp.Slika).FirstOrDefaultAsync(temp => temp.IzdavacID == id);
			if (izdavac == null)
			{
				return NotFound("Izdavac ne postoji.");
			}

			var slika = await _db.Slika.FirstOrDefaultAsync(temp => (temp.SlikaPutanja == izdavac.Slika.SlikaPutanja && temp.SlikaPutanja != "/assets/images/default.jpg"));
			if (slika != null)
			{
				_db.Slika.Remove(slika);
			}
		
			//var izdavacRecenzije = await _db.IzdavacRecenzija.Where(temp => temp.IzdavacID == izdavac.IzdavacID).ToListAsync();

			//foreach (var rec in izdavacRecenzije)
			//{
			//	_db.IzdavacRecenzija.Remove(rec);
			//}

			_db.Izdavac.Remove(izdavac);
			await _db.SaveChangesAsync();

			return NoContent();
		}
	}
}
