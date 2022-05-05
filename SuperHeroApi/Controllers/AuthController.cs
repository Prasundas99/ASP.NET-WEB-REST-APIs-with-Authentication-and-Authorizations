using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;


namespace SuperHeroApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class authController : ControllerBase
    {
        public static User user = new User();
        public readonly IConfiguration _configuration; 
        public authController(IConfiguration configuration){
            _configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserAuth request)
        {
            if (request.Username == null || request.Password == null)
                return BadRequest("Username or Password is empty");
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserAuth request)
        {
            if (request.Username == null || request.Password == null)
                return BadRequest("Username or Password is empty");
            
            if (user.Username != request.Username)
                return BadRequest("Username not Found");
               
             if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                    return BadRequest("Password is incorrect");

            string token = GenerateJwtToken(user);
            return new OkObjectResult(new { Token = token, Username = user.Username});
        }

        private string GenerateJwtToken(User user)
        {
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.Name, user.Username)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private Boolean VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }
    }
}