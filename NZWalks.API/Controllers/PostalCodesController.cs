using AutoMapper;
using DAL.Models.Domain;
using DAL.Models.DTO;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostalCodesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPostalCodeRepository postalCodeRepository;

        public PostalCodesController(IMapper mapper, IPostalCodeRepository postalCodeRepository)
        {
            this.mapper = mapper;
            this.postalCodeRepository = postalCodeRepository;
        }

        // CREATE PostalCode
        // POST: /api/postalcodes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddPostalCodeRequestDto addPostalCodeRequestDto)
        {
            // Check for duplicate PostalCodeId
            var existingPostalCode = await postalCodeRepository.GetByIdAsync(addPostalCodeRequestDto.PostalCodeId);
            if (existingPostalCode != null)
            {
                return BadRequest("PostalCodeId already exists. Please provide a unique ID.");
            }

            var postalCodeDomainModel = mapper.Map<PostalCode>(addPostalCodeRequestDto);

            await postalCodeRepository.CreateAsync(postalCodeDomainModel);

            return Ok(mapper.Map<PostalCodeDto>(postalCodeDomainModel));
        }

        // GET All PostalCodes
        // GET: /api/postalcodes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var postalCodesDomainModel = await postalCodeRepository.GetAllAsync();

            // Map Domain Model to DTO
            return Ok(mapper.Map<List<PostalCodeDto>>(postalCodesDomainModel));
        }

        // GET PostalCode By Id
        // GET: /api/postalcodes/{id}
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var postalCodeDomainModel = await postalCodeRepository.GetByIdAsync(id);

            if (postalCodeDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<PostalCodeDto>(postalCodeDomainModel));
        }

        // UPDATE PostalCode By Id
        // PUT: /api/postalcodes/{id}
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePostalCodeRequestDto updatePostalCodeRequestDto)
        {
            // Map DTO to Domain Model
            var postalCodeDomainModel = mapper.Map<PostalCode>(updatePostalCodeRequestDto);

            postalCodeDomainModel = await postalCodeRepository.UpdateAsync(id, postalCodeDomainModel);

            if (postalCodeDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<PostalCodeDto>(postalCodeDomainModel));
        }

        // DELETE PostalCode By Id
        // DELETE: /api/postalcodes/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedPostalCodeDomainModel = await postalCodeRepository.DeleteAsync(id);
            if (deletedPostalCodeDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<PostalCodeDto>(deletedPostalCodeDomainModel));
        }
    }
}
