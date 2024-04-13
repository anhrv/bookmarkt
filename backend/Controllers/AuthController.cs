using backend.Data;
using backend.DTOs.Nalog;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using NuGet.Protocol;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace backend.Controllers
{
	public class authController : CustomController
	{
		private readonly ApplicationDbContext _db;
		private readonly IConfiguration _config;
		private readonly CurrentUserService _currentUser;
		private readonly EmailService _email;

		public authController(ApplicationDbContext db, IConfiguration config, CurrentUserService currentUser, EmailService email)
		{
			_db = db;
			_config = config;
			_currentUser = currentUser;
			_email = email;
		}

		[HttpPost("registracija")]
		public async Task<IActionResult> Registracija([FromBody] RegistracijaDTO korisnik)
		{
			var poruka = "";
			var nalogPoEmailu = await _db.Nalog.FirstOrDefaultAsync(temp => temp.Email == korisnik.Email);
			if (nalogPoEmailu != null)
				poruka += "Korisnik sa tim emailom već postoji. Unesite drugi email!\n";
			var nalogPoImenu = await _db.Nalog.FirstOrDefaultAsync(temp => temp.KorisnickoIme == korisnik.KorisnickoIme);
			if (nalogPoImenu != null)
				poruka += "Korisnik sa tim imenom već postoji. Unesite drugo ime!\n";
			if (nalogPoEmailu != null || nalogPoImenu != null)
				return BadRequest(poruka);

			var nalog = new Nalog
			{
				KorisnickoIme = korisnik.KorisnickoIme,
				Email = korisnik.Email,
				Lozinka = BCrypt.Net.BCrypt.HashPassword(korisnik.Lozinka),
				DatumRegistracije = DateTime.Now,
				PosljednjiLogin = DateTime.Now,
				UlogaID = await _db.Uloga.Where(temp => temp.Naziv == "Korisnik").Select(temp => temp.UlogaID).FirstOrDefaultAsync(),
				TwoFactorEnabled = false,
				TwoFactorCode = null,
				TwoFactorExpiration = null,
				TwoFactorTries = null,
			};

			_db.Nalog.Add(nalog);
			await _db.SaveChangesAsync();
			await _db.Entry(nalog)
						.Reference(a => a.Uloga)
						.LoadAsync();

			string token = CreateToken(nalog).ToJson();

			return Ok(token);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDTO korisnik)
		{
			var nalog = await _db.Nalog.Where(temp => temp.Email == korisnik.Email).Include(n => n.Uloga).FirstOrDefaultAsync();

			if (nalog == null || !BCrypt.Net.BCrypt.Verify(korisnik.Lozinka, nalog.Lozinka))
			{
				return BadRequest("Pogrešan email ili lozinka.");
			}

			if (!nalog.TwoFactorEnabled)
			{
				nalog.PosljednjiLogin = DateTime.Now;
				await _db.SaveChangesAsync();

				string token = CreateToken(nalog);

				var result = new loginResponseDTO
				{
					is2faEnabled = false,
					token = token,
				};
				return Ok(result);
			}
			else
			{
				var code = GenerateTwoFactorCode();

				nalog.TwoFactorCode = code;
				nalog.TwoFactorTries = 3;
				nalog.TwoFactorExpiration = DateTime.Now.AddMinutes(5);

				await _db.SaveChangesAsync();

				var naslov = "2FA kod";
				var poruka = $"<p style=\"color:#3b9656;font-family: Arial, Helvetica, sans-serif; font-size: large; font-weight: bold;\">2FA kod</p><p style=\"color:#474f62; font-family: Arial, Helvetica, sans-serif; font-size: medium;\">Vaš 2FA kod: <b>{code}</b></p>";
				_email.Send(nalog.Email, naslov, poruka);

				var result = new loginResponseDTO
				{
					is2faEnabled = true,
					token = nalog.NalogID.ToString(),
				};
				return Ok(result);
			}
		}

		[HttpPost("login2fa")]
		public async Task<IActionResult> Login2FA([FromBody] Login2faDTO login2faDTO)
		{
			var guid = new Guid();
			var isGuid = Guid.TryParse(login2faDTO.NalogId, out guid);

			if(!isGuid)
			{
				return Unauthorized("Desila se greška.");
			}

			var nalog = await _db.Nalog.Where(temp => temp.NalogID == guid).Include(n => n.Uloga).FirstOrDefaultAsync();

			if (nalog == null)
			{
				return Unauthorized("Desila se greška.");
			}

			if(nalog.TwoFactorExpiration < DateTime.Now)
			{
				nalog.TwoFactorCode = null;
				nalog.TwoFactorExpiration = null;
				nalog.TwoFactorTries = null;
				await _db.SaveChangesAsync();

				return Unauthorized("Kod je istekao. Pokušajte opet!");
			}

			if(nalog.TwoFactorCode != login2faDTO.TwoFactorCode)
			{
				nalog.TwoFactorTries--;
				if(nalog.TwoFactorTries == 0)
				{
					nalog.TwoFactorCode = null;
					nalog.TwoFactorExpiration = null;
					nalog.TwoFactorTries = null;
					await _db.SaveChangesAsync();
					return Unauthorized("Potrošili ste pokušaje. Pokušajte ponovo.");
				}
				await _db.SaveChangesAsync();
				return BadRequest("Pogrešan kod. Pokušajte ponovo.");
			}
			else
			{
				nalog.PosljednjiLogin = DateTime.Now;
				nalog.TwoFactorCode = null;
				nalog.TwoFactorExpiration = null;
				nalog.TwoFactorTries = null;
				await _db.SaveChangesAsync();

				string token = CreateToken(nalog).ToJson();

				return Ok(token);
			}
		}

		[HttpPut("updateLozinka")]
		[Authorize]
		public async Task<ActionResult<Nalog>> UpdateLozinka(UpdateLozinkaDTO lozinkaDTO)
		{
			var nalog = await _currentUser.Get();

			if (nalog == null)
			{
				return NotFound("Nalog ne postoji.");
			}

			if (!BCrypt.Net.BCrypt.Verify(lozinkaDTO.TrenutnaLozinka, nalog.Lozinka))
			{
				return BadRequest("Pogrešna lozinka.");
			}

			nalog.Lozinka = BCrypt.Net.BCrypt.HashPassword(lozinkaDTO.NovaLozinka);

			await _db.SaveChangesAsync();

			return Ok();
		}

		[HttpPost("forgotLozinka")]
		public async Task<IActionResult> ForgotLozinka([FromBody] ForgotLozinkaDTO forgotLozinkaDTO)
		{
			var nalog = await _db.Nalog.FirstOrDefaultAsync(temp => temp.Email == forgotLozinkaDTO.Email);
			if (nalog == null)
			{
				return NotFound("Nalog ne postoji.");
			}

			var resetToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
			nalog.ResetLozinkaToken = HashWithSHA256(resetToken);
			nalog.ResetTokenIstice = DateTime.Now.AddMinutes(15);

			await _db.SaveChangesAsync();

			var resetURL = $"{_config.GetConnectionString("client")}/reset-password/{resetToken}";

			var naslov = "Reset lozinke";
			var poruka = $"<p style=\"color:#3b9656;font-family: Arial, Helvetica, sans-serif; font-size: large; font-weight: bold;\">Reset lozinke</p><p style=\"color:#474f62; font-family: Arial, Helvetica, sans-serif; font-size: medium;\">Možete promijeniti lozinku na sljedećem linku (u narednih 15 minuta):</p><a style=\"font-family: Arial, Helvetica, sans-serif;\" href=\"{resetURL}\\\">{resetURL}</a>";

			//send email
			_email.Send(nalog.Email, naslov, poruka);

			return Ok();
		}

		[HttpPost("resetLozinka/{token}")]
		public async Task<IActionResult> ResetLozinka([FromRoute] string token, [FromBody] ResetLozinkaDTO resetLozinkaDTO)
		{
			var hashedToken = HashWithSHA256(token);

			var nalog = await _db.Nalog.Include(temp => temp.Uloga).FirstOrDefaultAsync(temp => temp.ResetLozinkaToken == hashedToken);
			if (nalog == null || nalog.ResetTokenIstice < DateTime.Now)
			{
				return NotFound("Token nije validan.");
			}

			nalog.Lozinka = BCrypt.Net.BCrypt.HashPassword(resetLozinkaDTO.NovaLozinka);
			nalog.ResetLozinkaToken = null;
			nalog.ResetTokenIstice = null;

			await _db.SaveChangesAsync();

			string loginToken = CreateToken(nalog).ToJson();

			return Ok(loginToken);
		}

		private string CreateToken(Nalog nalog)
		{
			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier, nalog.NalogID.ToString()),
				new Claim(ClaimTypes.Role, nalog.Uloga.Naziv.ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(_config["Jwt:Issuer"],
			  _config["Jwt:Audience"],
			  claims,
			  expires: DateTime.Now.AddDays(1),
			  signingCredentials: creds);

			var jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
		}

		private string HashWithSHA256(string input)
		{
			using (var sha256 = SHA256.Create())
			{
				byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

				return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
			}
		}

		private string GenerateTwoFactorCode()
		{
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var stringChars = new char[6];
			var random = new Random();

			for (int i = 0; i < stringChars.Length; i++)
			{
				stringChars[i] = chars[random.Next(chars.Length)];
			}

			var finalString = new String(stringChars);

			return finalString;
		}
	}
}