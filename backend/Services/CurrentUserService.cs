using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace backend.Services
{
	public class CurrentUserService
	{
		private readonly IHttpContextAccessor _context;
		private readonly ApplicationDbContext _db;

		public CurrentUserService(IHttpContextAccessor context,ApplicationDbContext db)
		{
			_context = context;
			_db = db;
		}

		public async Task<Nalog> Get()
		{
			var context = _context.HttpContext;

			var identity = context.User.Identity as ClaimsIdentity;

			if (identity != null)
			{
				var userClaims = identity.Claims;
				var id = Guid.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value);
				var nalog = await _db.Nalog.Include(n => n.Uloga).FirstOrDefaultAsync(temp => temp.NalogID==id);
				return nalog;
			}
			return null;
		}
	}
}
