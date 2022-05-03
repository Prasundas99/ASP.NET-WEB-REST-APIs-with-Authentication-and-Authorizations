using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SuperHeroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var heros = new List<SuperHero>
            {
                new SuperHero { Id = 1, Name = "Superman", FirstName = "Clark", LastName = "Kent" },
                new SuperHero { Id = 2, Name = "Batman", FirstName = "Bruce", LastName = "Wayne" },
                new SuperHero { Id = 3, Name = "Spiderman", FirstName = "Peter", LastName = "Parker" }
            };
            return Ok(new SuperHero
            {
                Id = 1,
                Name = "Superman",
                FirstName = "Clark",
                LastName = "Kent"
            });
        }
    }
}
