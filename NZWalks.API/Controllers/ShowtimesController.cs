using AutoMapper;
using DAL.Models.Domain;
using DAL.Models.DTO;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowtimesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IShowtimeRepository showtimeRepository;

        public ShowtimesController(IMapper mapper, IShowtimeRepository showtimeRepository)
        {
            this.mapper = mapper;
            this.showtimeRepository = showtimeRepository;
        }

        // CREATE Showtime
        // POST: /api/showtimes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddShowtimeRequestDto addShowtimeRequestDto)
        {
            // Map DTO to Domain Model
            var showtimeDomainModel = mapper.Map<Showtime>(addShowtimeRequestDto);

            await showtimeRepository.CreateAsync(showtimeDomainModel);

            // Map Domain model to DTO
            return Ok(mapper.Map<ShowtimeDto>(showtimeDomainModel));
        }

        // GET all Showtimes
        // GET: /api/showtimes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var showtimesDomainModel = await showtimeRepository.GetAllAsync();

            // Map Domain Model to DTO
            return Ok(mapper.Map<List<ShowtimeDto>>(showtimesDomainModel));
        }

        // GET Showtime by Id
        // GET: /api/showtimes/{id}
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var showtimeDomainModel = await showtimeRepository.GetByIdAsync(id);

            if (showtimeDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<ShowtimeDto>(showtimeDomainModel));
        }

        // UPDATE Showtime by Id
        // PUT: /api/showtimes/{id}
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateShowtimeRequestDto updateShowtimeRequestDto)
        {
            // Map DTO to Domain Model
            var showtimeDomainModel = mapper.Map<Showtime>(updateShowtimeRequestDto);

            showtimeDomainModel = await showtimeRepository.UpdateAsync(id, showtimeDomainModel);

            if (showtimeDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<ShowtimeDto>(showtimeDomainModel));
        }

        // DELETE Showtime by Id
        // DELETE: /api/showtimes/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedShowtimeDomainModel = await showtimeRepository.DeleteAsync(id);
            if (deletedShowtimeDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ShowtimeDto>(deletedShowtimeDomainModel));
        }
    }
}
