using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Repositories;
using DAL.Models.DTO;
using DAL.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinema.API.Controllers
{
    // /api/theaters
    [Route("api/[controller]")]
    [ApiController]
    public class TheatersController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITheaterRepository theaterRepository;

        public TheatersController(IMapper mapper, ITheaterRepository theaterRepository)
        {
            this.mapper = mapper;
            this.theaterRepository = theaterRepository;
        }

        // CREATE Theater
        // POST: /api/theaters
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddTheaterRequestDto addTheaterRequestDto)
        {
            if (addTheaterRequestDto == null)
            {
                return BadRequest("Invalid theater data.");
            }

            var theaterDomainModel = mapper.Map<Theater>(addTheaterRequestDto);
            var createdTheater = await theaterRepository.CreateAsync(theaterDomainModel);

            // Map Domain model to DTO
            return CreatedAtAction(nameof(GetById), new { id = createdTheater.TheaterId }, mapper.Map<TheaterDto>(createdTheater));
        }

        // GET All Theaters
        // GET: /api/theaters
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var theatersDomainModel = await theaterRepository.GetAllAsync();

            // Map Domain Model to DTO
            return Ok(mapper.Map<List<TheaterDto>>(theatersDomainModel));
        }

        // Get Theater By Id
        // GET: /api/theaters/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var theaterDomainModel = await theaterRepository.GetByIdAsync(id);

            if (theaterDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<TheaterDto>(theaterDomainModel));
        }

        // Update Theater By Id
        // PUT: /api/theaters/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTheaterRequestDto updateTheaterRequestDto)
        {
            if (updateTheaterRequestDto == null)
            {
                return BadRequest("Invalid update data.");
            }

            // Map DTO to Domain Model
            var theaterDomainModel = mapper.Map<Theater>(updateTheaterRequestDto);

            var updatedTheater = await theaterRepository.UpdateAsync(id, theaterDomainModel);

            if (updatedTheater == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<TheaterDto>(updatedTheater));
        }

        // DELETE Theater By Id
        // DELETE: /api/theaters/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedTheaterDomainModel = await theaterRepository.DeleteAsync(id);

            if (deletedTheaterDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<TheaterDto>(deletedTheaterDomainModel));
        }
    }
}
