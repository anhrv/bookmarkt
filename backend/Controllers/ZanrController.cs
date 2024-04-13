using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
	public class zanrController : CustomController
	{
		private readonly ApplicationDbContext _db;

		public zanrController(ApplicationDbContext db, SlikaService slikaService)
		{
			_db = db;
		}

		[HttpGet]
		[Authorize(Roles="Uposlenik")]
		public async Task<ActionResult<IEnumerable<Zanr>>> GetZanrovi()
		{
			var zanrovi = await _db.Zanr.ToListAsync();
			return Ok(zanrovi);
		}
	}
}
