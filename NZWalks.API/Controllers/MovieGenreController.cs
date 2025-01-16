using AutoMapper;
using DAL.Models.Domain;
using DAL.Models.DTO;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieGenreController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IMovieGenreRepository movieGenreRepository;

        public MovieGenreController(IMapper mapper, IMovieGenreRepository movieGenreRepository)
        {
            this.mapper = mapper;
            this.movieGenreRepository = movieGenreRepository;
        }

        // CREATE Movie-Genre 
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddMovieGenreRequestDto addMovieGenreRequestDto)
        {
            // Validate request data
            if (addMovieGenreRequestDto?.GenreIds == null || !addMovieGenreRequestDto.GenreIds.Any())
                return BadRequest("At least one Genre ID must be provided.");

            await movieGenreRepository.CreateAsync(addMovieGenreRequestDto.MovieId, addMovieGenreRequestDto.GenreIds);
            return Ok(new { Message = "Movie-Genre associations created successfully." });
        }

        // GET All Movie-Genre 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movieGenresDomainModel = await movieGenreRepository.GetAllAsync();
            return Ok(mapper.Map<List<MovieGenreDto>>(movieGenresDomainModel));
        }

        // GET Movie-Genre associations by MovieId
        [HttpGet("{movieId:int}")]
        public async Task<IActionResult> GetByMovieId(int movieId)
        {
            var genreIds = await movieGenreRepository.GetByIdAsync(movieId);

            if (genreIds == null || !genreIds.Any())
                return NotFound(new { Message = "No genres found for the specified movie." });

            return Ok(genreIds);
        }

        // DELETE Movie-Genre 
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteMovieGenreRequestDto deleteMovieGenreRequestDto)
        {
            if (deleteMovieGenreRequestDto?.GenreIds == null || !deleteMovieGenreRequestDto.GenreIds.Any())
                return BadRequest("At least one Genre ID must be provided for deletion.");

            await movieGenreRepository.DeleteAsync(deleteMovieGenreRequestDto.MovieId, deleteMovieGenreRequestDto.GenreIds);
            return Ok(new { Message = "Movie-Genre associations deleted successfully." });
        }
    }

}
