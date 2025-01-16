using AutoMapper;
using DAL.Models.Domain;
using DAL.Models.DTO;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IMovieRepository movieRepository;

        public MoviesController(IMapper mapper, IMovieRepository movieRepository)
        {
            this.mapper = mapper;
            this.movieRepository = movieRepository;
        }

        // CREATE Movie
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddMovieRequestDto addMovieRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Map DTO to Domain Model
                var movieDomainModel = mapper.Map<Movie>(addMovieRequestDto);

                // Use the repository to add the movie and associated genres
                var createdMovie = await movieRepository.CreateAsync(movieDomainModel);

                // Map the response to the DTO, including related genres
                var movieWithGenres = await movieRepository.GetByIdAsync(createdMovie.MovieId);

                if (movieWithGenres == null)
                {
                    return NotFound();
                }

                // Return Created status with the location of the newly created resource
                var createdMovieDto = mapper.Map<MovieDto>(movieWithGenres);
                return CreatedAtAction(nameof(GetById), new { id = createdMovie.MovieId }, createdMovieDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message, errorCode = "ArgumentException" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the movie.", error = ex.Message });
            }

        }

        // GET All Movies
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var moviesDomainModel = await movieRepository.GetAllAsync();

            return Ok(mapper.Map<List<MovieDto>>(moviesDomainModel));
        }

        // Get Movie By Id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movieDomainModel = await movieRepository.GetByIdAsync(id);

            if (movieDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<MovieDto>(movieDomainModel));
        }

        // Update Movie By Id
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMovieRequestDto updateMovieRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movieDomainModel = mapper.Map<Movie>(updateMovieRequestDto);
            movieDomainModel = await movieRepository.UpdateAsync(id, movieDomainModel);

            if (movieDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<MovieDto>(movieDomainModel));
        }

        // Delete Movie By Id
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedMovieDomainModel = await movieRepository.DeleteAsync(id);
            if (deletedMovieDomainModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }

}
