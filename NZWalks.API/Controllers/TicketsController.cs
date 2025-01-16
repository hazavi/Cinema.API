using AutoMapper;
using DAL.Models.Domain;  // Assuming Ticket domain model is here
using DAL.Models.DTO;     // Assuming Ticket DTOs are here
using DAL.Repositories;    // Assuming ITicketRepository is here
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITicketRepository ticketRepository;

        // Constructor to inject dependencies
        public TicketsController(IMapper mapper, ITicketRepository ticketRepository)
        {
            this.mapper = mapper;
            this.ticketRepository = ticketRepository;
        }

        // CREATE Ticket
        // POST: /api/tickets
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddTicketRequestDto addTicketRequestDto)
        {
            // Map DTO to Domain Model
            var ticketDomainModel = mapper.Map<Ticket>(addTicketRequestDto);

            await ticketRepository.CreateAsync(ticketDomainModel);

            // Map Domain model to DTO
            return Ok(mapper.Map<TicketDto>(ticketDomainModel));
        }

        // GET all Tickets
        // GET: /api/tickets
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ticketsDomainModel = await ticketRepository.GetAllAsync();

            // Map Domain Model to DTO
            return Ok(mapper.Map<List<TicketDto>>(ticketsDomainModel));
        }

        // GET Ticket by Id
        // GET: /api/tickets/{id}
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ticketDomainModel = await ticketRepository.GetByIdAsync(id);

            if (ticketDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<TicketDto>(ticketDomainModel));
        }

        // UPDATE Ticket by Id
        // PUT: /api/tickets/{id}
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTicketRequestDto updateTicketRequestDto)
        {
            // Map DTO to Domain Model
            var ticketDomainModel = mapper.Map<Ticket>(updateTicketRequestDto);

            ticketDomainModel = await ticketRepository.UpdateAsync(id, ticketDomainModel);

            if (ticketDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<TicketDto>(ticketDomainModel));
        }

        // DELETE Ticket by Id
        // DELETE: /api/tickets/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedTicketDomainModel = await ticketRepository.DeleteAsync(id);
            if (deletedTicketDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<TicketDto>(deletedTicketDomainModel));
        }
    }
}
