using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Superheroes.Shared;

namespace Superheroes.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperheroController : ControllerBase
    {
        private readonly DataContext _context;

        public SuperheroController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Superhero>>> GetSuperHeroes()
        {
            var heroes = await _context.Superheroes.Include(sh => sh.Comic).ToListAsync();
            return Ok(heroes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Superhero>> GetSingleHero(int id)
        {
            var hero = await _context.Superheroes
                .Include(h => h.Comic)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hero == null)
            {
                return NotFound("Sorry, Hero not found");
            }
            return Ok(hero);
        }
        [HttpGet("comics")]
        public async Task<ActionResult<List<Comic>>> GetComics()
        {
            var comics = await _context.Comics.ToListAsync();
            return Ok(comics);
        }
        [HttpPost]
        public async Task<ActionResult<List<Superhero>>> CreateSuperHero(Superhero hero)
        {
            hero.Comic = null;
            _context.Superheroes.Add(hero);
            await _context.SaveChangesAsync();

            return Ok(await GetDbHeroes());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Superhero>>> UpdateSuperHero(Superhero hero, int id)
        {
            var dbHero = await _context.Superheroes
                .Include(sh => sh.Comic)
                .FirstOrDefaultAsync(sh => sh.Id == id);
            if (dbHero == null)
            {
                return NotFound("Sorry, no hero returned");
            }
            else
            {
                dbHero.FirstName = hero.LastName;
                dbHero.LastName = hero.FirstName;
                dbHero.HeroName = hero.HeroName;
                dbHero.ComicId = hero.ComicId;

                await _context.SaveChangesAsync();
                return Ok(await GetDbHeroes());
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Superhero>>> DeleteSuperHero(int id)
        {
            var dbHero = await _context.Superheroes
                .Include(sh => sh.Comic)
                .FirstOrDefaultAsync(sh => sh.Id == id);
            if (dbHero == null)
            {
                return NotFound("Sorry, no hero returned");
            }
            else
            {
                _context.Superheroes.Remove(dbHero);
                await _context.SaveChangesAsync();
                return Ok(await GetDbHeroes());
            }

        }
        private async Task<List<Superhero>> GetDbHeroes()
        {
            return await _context.Superheroes.Include(sh => sh.Comic).ToListAsync();  
        }
    }
}
