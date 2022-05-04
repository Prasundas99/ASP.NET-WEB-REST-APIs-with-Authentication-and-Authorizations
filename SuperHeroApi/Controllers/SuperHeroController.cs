using Microsoft.AspNetCore.Mvc;

namespace SuperHeroApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuperHerosController : ControllerBase
    {
        private static List<SuperHeros> heros = new List<SuperHeros>{
                new SuperHeros{
                    Id = 1,
                    Name = "Superman",
                    FirstName = "Clark",
                    LastName = "Kent",
                    Place = "Metropolis"

                },
                new SuperHeros{
                    Id = 2,
                    Name = "Batman",
                    FirstName = "Bruce",
                    LastName = "Wayne",
                    Place = "Gotham"
                },
                new SuperHeros{
                    Id = 3,
                    Name = "Spiderman",
                    FirstName = "Peter",
                    LastName = "Parker",
                    Place = "New York"
                },
                new SuperHeros{
                    Id = 4,
                    Name = "Tony",
                    FirstName = "Stark",
                    LastName = "Ironman",
                    Place = "New York"
                },
            };


        /**
        * GET api/superheroes
        * @Description: Get all superheroes
        */
        [HttpGet]
        public async Task<ActionResult<List<SuperHeros>>> Get()
        {
            return Ok(heros);
        }

        /**
        * GET api/superheroes/{id}
        * @Description: Get a superhero by id
        */
        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHeros>> Get(int id)
        {
            var hero = heros.Find(h => h.Id == id);
            if (hero == null)
                return NotFound("Superhero not found");
            return Ok(hero);
        }


        /**
        * POST api/superheroes
        * @Description: Create a new superhero
        */
        [HttpPost]
        public async Task<ActionResult<List<SuperHeros>>> AddHero(SuperHeros hero)
        {
            heros.Add(hero);
            return Ok(heros);
        }

        /**
        * PUT api/superheroes/{id}
        * @Description: Update a superhero
        */
        [HttpPut("{id}")]
        public async Task<ActionResult<List<SuperHeros>>> UpdateHero(int id, SuperHeros hero)
        {
            var heroToUpdate = heros.Find(h => h.Id == id);
            if (heroToUpdate == null)
                return NotFound("Superhero not found");
            heroToUpdate.Name = hero.Name;
            heroToUpdate.FirstName = hero.FirstName;
            heroToUpdate.LastName = hero.LastName;
            heroToUpdate.Place = hero.Place;
            return Ok(heros);
        }

        /**
        * DELETE api/superheroes/{id}
        * @Description: Delete a superhero
        */
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHeros>>> DeleteHero(int id)
        {
            var heroToDelete = heros.Find(h => h.Id == id);
            if (heroToDelete == null)
                return NotFound("Superhero not found");
            heros.Remove(heroToDelete);
            return Ok(heros);
        }

    }
}