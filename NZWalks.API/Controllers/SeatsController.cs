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
    public class SeatsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ISeatRepository seatRepository;

        public SeatsController(IMapper mapper, ISeatRepository seatRepository)
        {
            this.mapper = mapper;
            this.seatRepository = seatRepository;
        }

        // CREATE Seat
        // POST: /api/seats
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddSeatRequestDto addSeatRequestDto)
        {
            // Map DTO to Domain Model
            var seatDomainModel = mapper.Map<Seat>(addSeatRequestDto);

            await seatRepository.CreateAsync(seatDomainModel);

            // Map Domain model to DTO
            return Ok(mapper.Map<SeatDto>(seatDomainModel));
        }

        // GET all Seats
        // GET: /api/seats
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var seatsDomainModel = await seatRepository.GetAllAsync();

            // Map Domain Model to DTO
            return Ok(mapper.Map<List<SeatDto>>(seatsDomainModel));
        }

        // GET Seat by Id
        // GET: /api/seats/{id}
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var seatDomainModel = await seatRepository.GetByIdAsync(id);

            if (seatDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<SeatDto>(seatDomainModel));
        }

        // UPDATE Seat by Id
        // PUT: /api/seats/{id}
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSeatRequestDto updateSeatRequestDto)
        {
            // Map DTO to Domain Model
            var seatDomainModel = mapper.Map<Seat>(updateSeatRequestDto);

            seatDomainModel = await seatRepository.UpdateAsync(id, seatDomainModel);

            if (seatDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<SeatDto>(seatDomainModel));
        }

        // DELETE Seat by Id
        // DELETE: /api/seats/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedSeatDomainModel = await seatRepository.DeleteAsync(id);
            if (deletedSeatDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<SeatDto>(deletedSeatDomainModel));
        }
    }
}
