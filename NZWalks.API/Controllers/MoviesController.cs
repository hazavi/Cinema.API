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
        public async Task<IActionResult> Create([FromForm] AddMovieRequestDto request)
        {
            try
            {
                // Validate if the poster file is uploaded
                if (request.PosterFile == null)
                {
                    return BadRequest(new { Message = "No poster file uploaded." });
                }

                // Validate file type and size (adjust to your needs)
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                const long maxFileSize = 5 * 1024 * 1024; // 5MB

                var fileExtension = Path.GetExtension(request.PosterFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { Message = "Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed." });
                }

                if (request.PosterFile.Length > maxFileSize)
                {
                    return BadRequest(new { Message = "The poster file exceeds the maximum size of 5MB." });
                }

                // Convert poster file to byte array
                byte[] posterBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await request.PosterFile.CopyToAsync(memoryStream);
                    posterBytes = memoryStream.ToArray();
                }

                // Create movie object
                var movie = new Movie
                {
                    Title = request.Title,
                    Description = request.Description,
                    DurationMinutes = request.DurationMinutes,
                    Rating = request.Rating,
                    ReleaseDate = request.ReleaseDate,
                    isShowing = request.isShowing,
                    PosterUrl = posterBytes, // Save the byte array as the poster
                    MovieGenres = request.GenreIds.Select(genreId => new MovieGenre
                    {
                        GenreId = genreId
                    }).ToList()
                };

                // Save the movie to the database
                await movieRepository.CreateAsync(movie);

                // Create MovieDto to return as response
                var movieDto = new MovieDto
                {
                    MovieId = movie.MovieId,
                    PosterUrlBase64 = Convert.ToBase64String(movie.PosterUrl),
                    Title = movie.Title,
                    Description = movie.Description,
                    DurationMinutes = movie.DurationMinutes,
                    Rating = movie.Rating,
                    ReleaseDate = movie.ReleaseDate,
                    isShowing = movie.isShowing,
                    GenreIds = movie.MovieGenres.Select(mg => mg.GenreId).ToList()
                };

                return CreatedAtAction(nameof(GetById), new { id = movie.MovieId }, movieDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while creating the movie.", Details = ex.Message });
            }
        }


        // GET Poster by Movie ID
        [HttpGet("{id:int}/poster")]
        public async Task<IActionResult> GetPoster(int id)
        {
            var movieDomainModel = await movieRepository.GetByIdAsync(id);
            if (movieDomainModel == null || movieDomainModel.PosterUrl == null)
            {
                return NotFound();
            }
            return File(movieDomainModel.PosterUrl, "image/jpeg"); // Adjust MIME type as needed
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
            var movie = await movieRepository.GetByIdAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = new MovieDto
            {
                MovieId = movie.MovieId,
                PosterUrlBase64 = movie.PosterUrl != null ? Convert.ToBase64String(movie.PosterUrl) : null,
                Title = movie.Title,
                Description = movie.Description,
                DurationMinutes = movie.DurationMinutes,
                Rating = movie.Rating,
                ReleaseDate = movie.ReleaseDate,
                isShowing = movie.isShowing,
                GenreIds = movie.MovieGenres.Select(mg => mg.GenreId).ToList()
            };

            return Ok(movieDto);
        }

        // Update Movie By Id
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateMovieRequestDto updateMovieRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingMovie = await movieRepository.GetByIdAsync(id);
            if (existingMovie == null)
            {
                return NotFound();
            }

            // Update movie properties
            existingMovie.Title = updateMovieRequestDto.Title;
            existingMovie.Description = updateMovieRequestDto.Description;
            existingMovie.DurationMinutes = updateMovieRequestDto.DurationMinutes;
            existingMovie.Rating = updateMovieRequestDto.Rating;
            existingMovie.ReleaseDate = updateMovieRequestDto.ReleaseDate;
            existingMovie.isShowing = updateMovieRequestDto.isShowing;

            // Handle Genre Updates
            existingMovie.MovieGenres = updateMovieRequestDto.GenreIds.Select(genreId => new MovieGenre
            {
                GenreId = genreId
            }).ToList();

            // Handle Poster Update
            if (updateMovieRequestDto.PosterFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await updateMovieRequestDto.PosterFile.CopyToAsync(memoryStream);
                    existingMovie.PosterUrl = memoryStream.ToArray();
                }
            }

            var updatedMovie = await movieRepository.UpdateAsync(id, existingMovie);

            if (updatedMovie == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating movie");
            }

            var movieDto = new MovieDto
            {
                MovieId = updatedMovie.MovieId,
                PosterUrlBase64 = updatedMovie.PosterUrl != null ? Convert.ToBase64String(updatedMovie.PosterUrl) : null,
                Title = updatedMovie.Title,
                Description = updatedMovie.Description,
                DurationMinutes = updatedMovie.DurationMinutes,
                Rating = updatedMovie.Rating,
                ReleaseDate = updatedMovie.ReleaseDate,
                isShowing = updatedMovie.isShowing,
                GenreIds = updatedMovie.MovieGenres.Select(mg => mg.GenreId).ToList()
            };

            return Ok(movieDto);
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

        private bool IsImage(string fileName)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
            return allowedExtensions.Contains(fileExtension);
        }
    }

}
