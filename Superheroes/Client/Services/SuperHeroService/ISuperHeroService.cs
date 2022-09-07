using Superheroes.Shared;

namespace Superheroes.Client.Services.SuperHeroService
{
    public interface ISuperHeroService
    {
        List<Superhero> Heroes { get; set; }
        List<Comic> Comics { get; set; }
        Task <Superhero> GetSingleHero(int id);
        Task GetComics();
        Task GetSuperheroes();
        Task CreateSuperHero(Superhero hero);
        Task UpdateSuperHero(Superhero hero);
        Task DeleteSuperHero(int id);
    }
}
