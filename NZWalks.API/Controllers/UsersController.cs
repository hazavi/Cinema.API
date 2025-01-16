using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DAL.Repositories;
using DAL.Models.DTO;
using DAL.Models.Domain;
using Cinema.API;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class UsersController : ControllerBase
    {
        // Felter til dependencies
        private readonly IMapper _mapper; // Mapper DTO'er til og fra domain models
        private readonly IUserRepository _userRepository; // Interface til user operation i db
        private readonly Jwtsettings _jwtSettings; // JWT Configuration
        private readonly ITokenService _tokenService; // Service til at generere JWT-tokens

        // Constructor for klassen til at initialisere dependencies
        // Kaldt automatisk når en intans af klassen bliver kaldt
        // (parameter) - type og navn
        public UsersController(IMapper mapper, IUserRepository userRepository, Jwtsettings jwtSettings, ITokenService tokenService) 
        {
            // Asigner parameters værdiere til klasens field
            _mapper = mapper;
            _userRepository = userRepository;
            _jwtSettings = jwtSettings;
            _tokenService = tokenService;
        }

        // Controller action-metode
        // async - udføre opgaver, der tager tid (som db kald) uden at blokere programmet.
        // T<> - return-type, der kører asynkront,
        // IActionResult - interface der bruges til at definere, hvad en HTTP respons skal være fx: Ok(), BadRequest() osv..
        //  FromBody - at dataen kommer fra HTTP-anmodningens body.
        [HttpPost] // POST-endpoint- Create
        public async Task<IActionResult> Create([FromBody] AddUserRequestDto addUserRequestDto) // metode
        {
            // tjekker, om der er sendt ugyldige data
            if (addUserRequestDto == null)
            {
                return BadRequest("Invalid user data."); // Returnere http 400 bad request
            }

            // var - keyword :  automatisk finder ud af hvilken datatype en variabel skal have
            var password = addUserRequestDto.Password; // Henter user password
            var userDomainModel = _mapper.Map<User>(addUserRequestDto); // Mapper DTO'en til en Domain-Model
            userDomainModel.IsAdmin = addUserRequestDto.IsAdmin; // Sætter om user er admin
            await _userRepository.CreateAsync(userDomainModel, password); // Opretter user i db

            // Returnerer HTTP 201 Created og info om den nye user
            return CreatedAtAction(nameof(GetById), new { id = userDomainModel.UserId }, _mapper.Map<UserDto>(userDomainModel));
        }

        [HttpGet] // GET-endpoint - Read
        public async Task<IActionResult> GetAll() 
        {
            // Henter Alt Users og asigner det i 'users'
            var users = await _userRepository.GetAllAsync();

            return Ok(_mapper.Map<List<UserDto>>(users)); // Returnerer en HTTP 200 OK-respons med en liste over users,
        }

        [HttpGet("{id:int}")] // GET-endpoint med en int parameter
        public async Task<IActionResult> GetById(int id)
        {
            // Henter Users efter id og asigner det i 'users'
            var user = await _userRepository.GetByIdAsync(id);

            // Tjeker om user er null
            if (user == null)
            {
                return NotFound(); // retunere http 404 not found respons
            }

            // Hvis "user" ikke er null, returneres en http 200 OK-respons med dataen. 
            return Ok(_mapper.Map<UserDto>(user));
        }

        // Update
        [HttpPut("{id:int}")] // Put-endpoint med en int parameter
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequestDto updateUserRequestDto)
        {
            // tjeker om det modtagne data er
            if (updateUserRequestDto == null)
            {
                return BadRequest("Invalid update data."); // Hvis data er null, returneres en bad request
            }

            // Mapper updater... til User-objektet som er domail for user
            var user = _mapper.Map<User>(updateUserRequestDto);

            // Kalder UpdateAsync-metoden fra repo for at opdatere user i db
            var updatedUser = await _userRepository.UpdateAsync(id, user, updateUserRequestDto.NewPassword);

            // hvis update ikke lykkedes og updatedUser er null
            if (updatedUser == null)
            {
                return NotFound(); // retunere not found request
            }
            // hvis opdateringen lykkedes, returnerer (OK) 
            return Ok(_mapper.Map<UserDto>(updatedUser));
        }

        // Delete
        [HttpDelete("{id:int}")] // Delete-endpoint , med int parameter
        public async Task<IActionResult> Delete(int id)
        {
            // Kalder DeleteAsync-metoden fra repo for at slette brugeren med det angivne ID.
            var deletedUser = await _userRepository.DeleteAsync(id);

            // Hvis deletedUser er null
            if (deletedUser == null)
            {
                return NotFound(); // Returnerer  (NotFound)
            }

            // Hvis brugeren blev slettet korrekt, retunere Ok 
            return Ok(_mapper.Map<UserDto>(deletedUser));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AddUserRequestDto addUserRequestDto)
        {
            if (addUserRequestDto == null)
            {
                return BadRequest("Invalid registration data.");
            }

            try
            {
                var password = addUserRequestDto.Password;
                var userDomainModel = _mapper.Map<User>(addUserRequestDto);
                userDomainModel.IsAdmin = false;

                await _userRepository.CreateAsync(userDomainModel, password);
                return CreatedAtAction(nameof(GetById), new { id = userDomainModel.UserId }, _mapper.Map<UserDto>(userDomainModel));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error occurred: {ex.Message}");
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto) 
        {
            var user = await _userRepository.GetByEmailAsync(loginRequestDto.Email);

            // Hvis brugeren ikke findes, eller adgangskoden ikke stemmer , returneres en Unauthorized fejl (401).
            if (user == null || !VerifyPasswordHash(loginRequestDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Unauthorized("Invalid credentials.");
            }
            // Genererer et JWT-token for den autentificerede bruger.
            var token = _tokenService.GenerateJwtToken(user);

            return Ok(new LoginResponseDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                Token = token
            });
        }


        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            if (passwordHash == null || passwordSalt == null)
            {
                return false;
            }

            // Brug HMACSHA512-algoritmen til at generere en hash af den indtastede password med saltet
            using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash); // Returnerer true
        }

        public interface ITokenService
        {
            // Interface-metode til at generere JWT-token.
            string GenerateJwtToken(User user);
        }

        public class TokenService : ITokenService
        {
            private readonly Jwtsettings _jwtSettings;

            public TokenService(Jwtsettings jwtSettings)
            {
                _jwtSettings = jwtSettings;
            }

            public string GenerateJwtToken(User user)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }

    }
}
