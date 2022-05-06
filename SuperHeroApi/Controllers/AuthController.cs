using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using SuperHeroApi.Services.UserService;
namespace SuperHeroApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class authController : ControllerBase
    {
        public static User user = new User();
        public readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public authController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }


/**
* @api {get} api/auth/service GetUser
* @apiName GetUser
* @apiGroup auth
* @apiVersion  1.0.0
* @apiDescription Get user information from token from services (Good Practice)
* @apiPermission Admin
* @apiHeader {String} Authorization Bearer {token}
* @apiSuccessExample {json} Success-Response:
* HTTP/1.1 200 OK
* {
*  "name": "John",
*  "name": "Doe",
*  "role": "Admin"
* }
*/
        [HttpGet("service"), Authorize]
        public ActionResult<string> GetService()
        {
            var userName = _userService.GetMyName();
            return Ok(userName);
        }

/**
* @api {get} api/auth/ GetUser
* @apiName GetUser
* @apiGroup auth
* @apiVersion  1.0.0
* @apiDescription Get user information from token in Controller version (BAD PRACTICE)
* @apiPermission Admin
* @apiHeader {String} Authorization Bearer {token}
* @apiSuccessExample {json} Success-Response:
* HTTP/1.1 200 OK
* {
*  "name": "John",
*  "name": "Doe",
*  "role": "Admin"
* }
*/
        [HttpGet, Authorize]
        public ActionResult<object> Get()
        {
            var userName = User?.Identity?.Name;
            var userName2 = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue(ClaimTypes.Role);
            return Ok(new { userName, userName2, role });
        }


/**
* @api {get} /api/auth/register Register
* @apiName Register
* @apiGroup Auth
* @apiVersion  1.0.0
* @apiDescription Register a new user
* @apiParamExample {json} Request-Example:
* {
*   "username": "username",
*   "password": "password",
* }
*/
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

/**
    * @api {get} /api/auth/login Login
    * @apiName Login
    * @apiGroup auth
    * @apiVersion  1.0.0
    * @apiDescription Login
    * @apiPermission none
    * @apiHeader {string} Accept application/json
    * @apiHeader {string} Content-Type application/json
    * @apiParam {string} username Username
    * @apiParam {string} password Password
    * @apiParamExample {json} Request-Example:
    * {
    *   "username": "admin",
    *   "password": "admin"
    * }
    * @apiSuccess {string} token Token
*/
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
            return new OkObjectResult(new { Token = token, Username = user.Username });
        }

        private string GenerateJwtToken(User user)
        {
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
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
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private Boolean VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }
    }
}