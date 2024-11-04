using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Repository;

public class PokemonRepository : IPokemonInterface
{
    private readonly DataContext _context;
    public PokemonRepository(DataContext context)
    {
        _context = context;
    }

    // getting all the pokemons and ordering them by id
    public ICollection<Pokemon> GetPokemons()
    {
        return _context.Pokemons.ToList();
    }

    // Getting a single pokemon based on the id
    public Pokemon GetPokemon(int id)
    {
        // return _context.Pokemons.Where(p => p.Id == id).FirstOrDefault();
        return _context.Pokemons.Find(id);
    }

    // Getting a single pokemon based on the name
    public Pokemon GetPokemon(string name)
    {
        return _context.Pokemons.Where(p => p.Name == name).FirstOrDefault();
    }

    public decimal GetPokemonRating(int pokemonId)
    {
        var review = _context.Reviews.Where(p => p.Id == pokemonId);

        if (review.Count() <= 0)
            return 0;
        return ((decimal)review.Sum(r=>r.Rating) / review.Count()); // Calculating the average
    }

    public bool PokemonExists(int pokemonId)
    {
        return _context.Pokemons.Any(p => p.Id == pokemonId);
    }
}