using AutoMapper; // Importerer AutoMapper-biblioteket til mapping mellem objekter
using DAL.Models.Domain; // Importerer domænemodeller fra data access layer
using DAL.Models.DTO; // Importerer data transfer objects fra data access layer
using DAL.Repositories; // Importerer repository interfaces til databaseoperationer
using Microsoft.AspNetCore.Mvc; // Importerer ASP.NET Core MVC-funktionalitet

namespace Cinema.API.Controllers
{
    // Angiver, at denne controller håndterer HTTP-anmodninger for 'Genres'
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IMapper mapper; // Felt til AutoMapper til mapping mellem DTO og Domain Model
        private readonly IGenreRepository genreRepository; // Felt til håndtering af databaseoperationer for 'Genre'

        // Constructor: Initialiserer mapper og genreRepository med dependency injection
        public GenresController(IMapper mapper, IGenreRepository genreRepository)
        {
            this.mapper = mapper; // Tildeler værdien af mapper-parameteren til feltet
            this.genreRepository = genreRepository; // Tildeler værdien af genreRepository-parameteren til feltet
        }

        // CREATE Genre
        // HTTP POST: /api/genres
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddGenreRequestDto addGenreRequestDto)
        {
            if (addGenreRequestDto == null)
            {
                // Returnerer en fejl, hvis ingen data blev sendt i body
                return BadRequest("Genre data is required.");
            }

            // Mapper DTO (AddGenreRequestDto) til Domain Model (Genre)
            var genreDomainModel = mapper.Map<Genre>(addGenreRequestDto);

            // Opretter en ny genre i databasen
            var createdGenre = await genreRepository.CreateAsync(genreDomainModel);

            if (createdGenre == null)
            {
                // Returnerer en fejl, hvis der opstod et problem under oprettelsen
                return StatusCode(500, "A problem occurred while creating the genre.");
            }

            // Mapper den oprettede genre (Domain Model) til DTO og returnerer 201 Created
            return CreatedAtAction(nameof(GetById), new { id = createdGenre.GenreId }, mapper.Map<GenreDto>(createdGenre));
        }

        // GET all Genres
        // HTTP GET: /api/genres
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Henter alle genrer fra databasen
            var genresDomainModel = await genreRepository.GetAllAsync();

            // Mapper listen af Domain Models til en liste af DTO'er og returnerer dem
            return Ok(mapper.Map<List<GenreDto>>(genresDomainModel));
        }

        // GET Genre by ID
        // HTTP GET: /api/genres/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Henter en genre baseret på ID fra databasen
            var genreDomainModel = await genreRepository.GetByIdAsync(id);

            if (genreDomainModel == null)
            {
                // Returnerer 404 Not Found, hvis genren ikke findes
                return NotFound();
            }

            // Mapper den fundne genre (Domain Model) til DTO og returnerer den
            return Ok(mapper.Map<GenreDto>(genreDomainModel));
        }

        // UPDATE Genre by ID
        // HTTP PUT: /api/genres/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateGenreRequestDto updateGenreRequestDto)
        {
            if (updateGenreRequestDto == null)
            {
                // Returnerer en fejl, hvis ingen opdaterede data blev sendt i body
                return BadRequest("Updated genre data is required.");
            }

            // Mapper DTO (UpdateGenreRequestDto) til Domain Model (Genre)
            var genreDomainModel = mapper.Map<Genre>(updateGenreRequestDto);

            // Opdaterer genren i databasen
            var updatedGenre = await genreRepository.UpdateAsync(id, genreDomainModel);

            if (updatedGenre == null)
            {
                // Returnerer 404 Not Found, hvis genren ikke findes
                return NotFound();
            }

            // Mapper den opdaterede genre (Domain Model) til DTO og returnerer den
            return Ok(mapper.Map<GenreDto>(updatedGenre));
        }

        // DELETE Genre by ID
        // HTTP DELETE: /api/genres/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Sletter genren baseret på ID fra databasen
            var deletedGenreDomainModel = await genreRepository.DeleteAsync(id);

            if (deletedGenreDomainModel == null)
            {
                // Returnerer 404 Not Found, hvis genren ikke findes
                return NotFound();
            }

            // Mapper den slettede genre (Domain Model) til DTO og returnerer den
            return Ok(mapper.Map<GenreDto>(deletedGenreDomainModel));
        }
    }
}
