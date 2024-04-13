using backend.Data;
using backend.DTOs.Knjiga;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
	public class knjigaController : CustomController
	{
		private readonly ApplicationDbContext _db;
		private readonly SlikaService _slikaService;

		public knjigaController(ApplicationDbContext db,
								SlikaService slikaService)
		{
			_db = db;
			_slikaService = slikaService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Knjiga>>> GetKnjige()
		{
			var knjige = await _db.Knjiga
									.Include(temp => temp.Slika)
									.Include(temp => temp.Zanr)
									.Include(temp => temp.Izdavac)
									.Include(temp => temp.Autori)
									.ThenInclude(temp => temp.Autor)
									.ToListAsync();
			return Ok(knjige);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Knjiga>> GetKnjigaByID([FromRoute] Guid id)
		{
			var knjiga = await _db.Knjiga
									.Include(temp => temp.Slika)
									.Include(temp => temp.Zanr)
									.Include(temp => temp.Izdavac)
									.Include(temp => temp.Autori)
									.ThenInclude(temp => temp.Autor)
									.FirstOrDefaultAsync(temp => temp.KnjigaID == id);

			if (knjiga == null)
			{
				return NotFound("Knjiga ne postoji.");
			}

			return Ok(knjiga);
		}


		[HttpPut("{id}")]
		[Authorize(Roles = "Uposlenik")]
		public async Task<ActionResult<Knjiga>> UpdateKnjiga([FromRoute] Guid id, [FromBody] UpdateKnjigaDTO knjigaDTO)
		{
			var knjiga = await _db.Knjiga.FindAsync(id);
			if (knjiga == null)
			{
				return NotFound("Knjiga ne postoji.");
			}

			Guid? slikaID = await _slikaService.putanjaToID(knjigaDTO.SlikaPutanja);

			knjiga.Naslov = knjigaDTO.Naslov ?? knjiga.Naslov;
			knjiga.ISBN = knjigaDTO.ISBN ?? knjiga.ISBN;
			knjiga.Opis = knjigaDTO.Opis;
			knjiga.DatumIzdavanja = knjigaDTO.DatumIzdavanja;
			knjiga.BrojStranica = knjigaDTO.BrojStranica;
			knjiga.Cijena = knjigaDTO.Cijena ?? knjiga.Cijena;
			knjiga.NaStanju = knjigaDTO.NaStanju ?? knjiga.NaStanju;
			knjiga.SlikaID = slikaID ?? knjiga.SlikaID;
			knjiga.IzdavacID = knjigaDTO.IzdavacID;
			knjiga.ZanrID = knjigaDTO.ZanrID;


			if (knjigaDTO.AutorIDs != null)
			{
				var knjigeAutori = await _db.KnjigaAutor.Where(ka => ka.KnjigaID == knjiga.KnjigaID).ToListAsync();
				foreach (var knjigaAutor in knjigeAutori)
				{
					_db.KnjigaAutor.Remove(knjigaAutor);
				}

				foreach (var autorID in knjigaDTO.AutorIDs)
				{
					var autor = _db.Autor.Find(autorID);
					if (autor == null)
					{
						return NotFound();
					}
					_db.KnjigaAutor.Add(new KnjigaAutor { Autor = autor, Knjiga = knjiga });
				}
			}

			await _db.SaveChangesAsync();
			await _db.Entry(knjiga)
						.Reference(a => a.Slika)
						.LoadAsync();
			await _db.Entry(knjiga)
						.Reference(a => a.Zanr)
						.LoadAsync();
			await _db.Entry(knjiga)
						.Reference(a => a.Izdavac)
						.LoadAsync();
			knjiga.Autori = await _db.KnjigaAutor.Where(x => x.KnjigaID == knjiga.KnjigaID).Include(temp=>temp.Autor).ToListAsync();
			return Ok(knjiga);
		}

		[HttpPost]
		[Authorize(Roles = "Uposlenik")]
		public async Task<ActionResult<Knjiga>> CreateKnjiga([FromBody] CreateKnjigaDTO knjigaDTO)
		{
			var knjiga = new Knjiga
			{
				Naslov = knjigaDTO.Naslov,
				ISBN = knjigaDTO.ISBN,
				Opis = knjigaDTO.Opis,
				BrojStranica = knjigaDTO.BrojStranica,
				DatumIzdavanja = knjigaDTO.DatumIzdavanja,
				Cijena = knjigaDTO.Cijena,
				NaStanju = knjigaDTO.NaStanju,
				SlikaID = await _slikaService.putanjaToID(knjigaDTO.SlikaPutanja),
				IzdavacID = knjigaDTO.IzdavacID,
				ZanrID = knjigaDTO.ZanrID,
			};
			_db.Knjiga.Add(knjiga);
			foreach (var autorID in knjigaDTO.AutorIDs)
			{
				var autor = _db.Autor.Find(autorID);
				if (autor == null)
				{
					return NotFound("Autor ne postoji.");
				}
				_db.KnjigaAutor.Add(new KnjigaAutor { Autor = autor, Knjiga = knjiga });
			}
			await _db.SaveChangesAsync();
			await _db.Entry(knjiga)
						.Reference(a => a.Slika)
						.LoadAsync();
			await _db.Entry(knjiga)
						.Reference(a => a.Zanr)
						.LoadAsync();
			await _db.Entry(knjiga)
						.Reference(a => a.Izdavac)
						.LoadAsync();
			knjiga.Autori = await _db.KnjigaAutor.Where(x => x.KnjigaID == knjiga.KnjigaID).ToListAsync();
			return CreatedAtAction("GetKnjigaByID",new { id = knjiga.KnjigaID }, knjiga);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Uposlenik")]
		public async Task<IActionResult> DeleteKnjiga([FromRoute] Guid id)
		{
			var knjiga = await _db.Knjiga.Include(temp=>temp.Slika).FirstOrDefaultAsync(temp=> temp.KnjigaID==id);
			if (knjiga == null)
			{
				return NotFound("Knjiga ne postoji.");
			}

			var slika = await _db.Slika.FirstOrDefaultAsync(temp => (temp.SlikaPutanja == knjiga.Slika.SlikaPutanja && temp.SlikaPutanja != "/assets/images/default.jpg"));
			if (slika != null)
			{
				_db.Slika.Remove(slika);
			}

			//var knjigaRecenzije = await _db.KnjigaRecenzija.Where(temp => temp.KnjigaID == knjiga.KnjigaID).ToListAsync();

			//foreach (var rec in knjigaRecenzije)
			//{
			//	_db.KnjigaRecenzija.Remove(rec);
			//}

			//var knjigeAutori = await _db.KnjigaAutor.Where(ka => ka.KnjigaID == knjiga.KnjigaID).ToListAsync();
			//foreach (var knjigaAutor in knjigeAutori)
			//{
			//	_db.KnjigaAutor.Remove(knjigaAutor);
			//}

			_db.Knjiga.Remove(knjiga);
			await _db.SaveChangesAsync();

			return NoContent();
		}
	}
}
