using DAL.Data;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Mappings;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Cinema.API;
using static NZWalks.API.Controllers.UsersController;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container. (Dependency Injection)
builder.Services.AddControllers(); // Tilføjer controllers til DI-containeren
builder.Services.AddEndpointsApiExplorer(); // Tilføjer endpoint explorer, som bruges til at dokumentere API'en
builder.Services.AddSwaggerGen(); // Tilføjer Swagger til API-dokumentation

// Configure Database Context
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

// Register Repositories, for at håndtere dataadgang i applikationen
builder.Services.AddScoped<IUserRepository, SQLUserRepository>(); // SQLUserRepository til at håndtere User data
builder.Services.AddScoped<IMovieRepository, SQLMovieRepository>();
builder.Services.AddScoped<IMovieGenreRepository, SQLMovieGenreRepository>();
builder.Services.AddScoped<IPostalCodeRepository, SQLPostalCodeRepository>();
builder.Services.AddScoped<IGenreRepository, SQLGenreRepository>();
builder.Services.AddScoped<IAddressRepository, SQLAddressRepository>();
builder.Services.AddScoped<ISeatRepository, SQLSeatRepository>();
builder.Services.AddScoped<IShowtimeRepository, SQLShowtimeRepository>();
builder.Services.AddScoped<ITicketRepository, SQLTicketRepository>();
builder.Services.AddScoped<ITheaterRepository, SQLTheaterRepository>();
builder.Services.AddScoped<ITokenService, TokenService>(); // TokenService til at håndtere JWT token generering

// Tilføj AutoMapper, som bruges til at mappe mellem DTO'er og Domain-objekter
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Configure CORS Policy, for at tillade krydsoprindelse (Cross-Origin Requests)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.AllowAnyOrigin() // Tillader alle origins (f.eks. frontend-apps fra forskellige domæner)
               .AllowAnyMethod() // Tillader alle HTTP-metoder (GET, POST, PUT, DELETE)
               .AllowAnyHeader(); // Tillader alle headers
    });
});

// Prevent JSON Serialization Cycles
builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Tilføj konfigurationsfilen appsettings.json, som indeholder opsætninger som connection strings og JWT settings
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Configure JWT settings (for autentificering)
var jwtSettings = new Jwtsettings();
builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings); // Tilføj JWT settings som singleton

// Configure JWT authentication, som bruges til at beskytte API-endpoints med en JWT token
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),


        };

    });

// Tilføjer Autorisation til at validere brugerens rettigheder
builder.Services.AddAuthorization();

var app = builder.Build(); // Opretter applikationen med alle konfigurerede services

// Configure the HTTP request pipeline (middleware) til håndtering af anmodninger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Aktiverer Swagger for API-dokumentation
    app.UseSwaggerUI(); // Vis Swagger UI for interaktiv dokumentation
}
// Enable CORS, for at tillade anmodninger fra andre oprindelser
app.UseCors("AllowSpecificOrigin");

// HTTPS Redirection til sikker kommunikation
app.UseHttpsRedirection();

// Add Authentication Middleware
app.UseAuthentication(); 

// Add Authorization Middleware
app.UseAuthorization();

// Map controllers (aktivér routing til API controllere)
app.MapControllers();

app.Run(); // Kør applikationen
