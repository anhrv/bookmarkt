using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace backend.Services
{
	public class SlikaService
	{
		private readonly ApplicationDbContext _db;

		public SlikaService(ApplicationDbContext db)
		{
			_db = db;
		}

		public async Task<Guid> putanjaToID(string slikaPutanja)
		{
			Guid slikaID;
			var slika = await _db.Slika.FirstOrDefaultAsync(temp => temp.SlikaPutanja == slikaPutanja);
			if (slika == null)
			{
				var novaSlika = new Slika
				{
					SlikaPutanja = slikaPutanja,
				};

				_db.Slika.Add(novaSlika);
				await _db.SaveChangesAsync();
				slikaID = novaSlika.SlikaID;
			}
			else
			{
				slikaID = slika.SlikaID;
			}
			return slikaID;
		}
	}
}
