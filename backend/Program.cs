using backend.Data;
using backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace backend
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", false)
				.Build();

			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddTransient<CurrentUserService>();
			builder.Services.AddTransient<EmailService>();
			builder.Services.AddTransient<SlikaService>();

			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(config.GetConnectionString("db")));

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options => {
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = config["Jwt:Issuer"],
						ValidAudience = config["Jwt:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
					};
				});

			builder.Services.AddControllers();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bookmarkt API", Version = "v1" });

				// Configure Swagger to use JWT for authorization
				var securityScheme = new OpenApiSecurityScheme
				{
					Name = "JWT Authentication",
					Description = "Enter JWT Bearer token",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT"
				};

				var securityRequirement = new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "bearerAuth"
							}
						},
						new string[] {}
					}
				};

				c.AddSecurityDefinition("bearerAuth", securityScheme);
				c.AddSecurityRequirement(securityRequirement);
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseCors(
			options => options
			.SetIsOriginAllowed(x => _ = true)
			.AllowAnyMethod()
			.AllowAnyHeader()
			.AllowCredentials()
			);

			app.UseHsts();
			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
