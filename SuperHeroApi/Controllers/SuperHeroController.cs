using Microsoft.AspNetCore.Mvc;

namespace SuperHeroApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuperHerosController : ControllerBase
    {
        private readonly DataContext _context;
        public SuperHerosController(DataContext context)
        {
            _context = context;
        }


        /**
        * GET api/superheroes
        * @Description: Get all superheroes
        */
        [HttpGet]
        public async Task<ActionResult<List<SuperHeros>>> Get()
        {
            return Ok(await _context.SuperHeros.ToListAsync());
        }

        /**
        * GET api/superheroes/{id}
        * @Description: Get a superhero by id
        */
        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHeros>> Get(int id)
        {
            var hero = await _context.SuperHeros.FindAsync(id);
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
            _context.SuperHeros.Add(hero);
            await _context.SaveChangesAsync();
            return Ok(hero);
        }

        /**
        * PUT api/superheroes/{id}
        * @Description: Update a superhero
        */
        [HttpPut("{id}")]
        public async Task<ActionResult<List<SuperHeros>>> UpdateHero(int id, SuperHeros hero)
        {
            var heros = await _context.SuperHeros.FindAsync(id);
            if (heros == null)
                return NotFound("Superhero not found");
            heros.Name = hero.Name;
            heros.FirstName = hero.FirstName;
            heros.LastName = hero.LastName;
            heros.Place = hero.Place;
            return Ok(heros);
        }

        /**
        * DELETE api/superheroes/{id}
        * @Description: Delete a superhero
        */
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHeros>>> DeleteHero(int id)
        {
            var heros = await _context.SuperHeros.FindAsync(id);
            if (heros == null)
                return NotFound("Superhero not found");
            _context.SuperHeros.Remove(heros);
            return Ok(heros);
        }

    }
}