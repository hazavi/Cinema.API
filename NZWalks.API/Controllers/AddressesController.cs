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
    public class AddressesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IAddressRepository addressRepository;

        public AddressesController(IMapper mapper, IAddressRepository addressRepository)
        {
            this.mapper = mapper;
            this.addressRepository = addressRepository;
        }

        // CREATE Address
        // POST: /api/address
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddAddressRequestDto addAddressRequestDto)
        {
            // Map DTO to Domain Model
            var addressDomainModel = mapper.Map<Address>(addAddressRequestDto);

            await addressRepository.CreateAsync(addressDomainModel);

            // Map Domain model to DTO
            return Ok(mapper.Map<AddressDto>(addressDomainModel));
        }

        // GET All Addresses
        // GET: /api/address
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var addressesDomainModel = await addressRepository.GetAllAsync();

            // Map Domain Model to DTO
            return Ok(mapper.Map<List<AddressDto>>(addressesDomainModel));
        }

        // Get Address By Id
        // GET: /api/address/{id}
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var addressDomainModel = await addressRepository.GetByIdAsync(id);

            if (addressDomainModel == null)
            {
                return NotFound();
            }
            // Map Domain Model to DTO
            return Ok(mapper.Map<AddressDto>(addressDomainModel));
        }

        // Update Address By Id
        // PUT: /api/address/{id}
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAddressRequestDto updateAddressRequestDto)
        {
            // Map DTO to Domain Model
            var addressDomainModel = mapper.Map<Address>(updateAddressRequestDto);

            addressDomainModel = await addressRepository.UpdateAsync(id, addressDomainModel);

            if (addressDomainModel == null)
            {
                return NotFound();
            }
            // Map Domain Model to DTO
            return Ok(mapper.Map<AddressDto>(addressDomainModel));
        }

        // Delete Address By Id
        // DELETE: /api/address/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedAddressDomainModel = await addressRepository.DeleteAsync(id);
            if (deletedAddressDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<AddressDto>(deletedAddressDomainModel));
        }
    }
}
