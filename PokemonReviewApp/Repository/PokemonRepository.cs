using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Repository;

public class PokemonRepository : IPokemonRepository
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

    public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
    {
        var pokemonOwnerEntity = _context.Owners.FirstOrDefault(o => o.Id == ownerId);
        var category = _context.Categories.Find(categoryId);

        var pokemonOwner = new PokemonOwner()
        {
            Owner = pokemonOwnerEntity,
            Pokemon = pokemon
        };

        _context.Add(pokemonOwner);
        
        var pokemonCategory = new PokemonCategory()
        {
            Category = category,
            Pokemon = pokemon
        };

        _context.Add(pokemonCategory);
        _context.Add(pokemon);

        return Save();
    }

    public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
    {
        var pokemonOwnerEntity = _context.Owners.FirstOrDefault(o => o.Id == ownerId);
        var category = _context.Categories.Find(categoryId);

        var pokemonOwner = new PokemonOwner()
        {
            Owner = pokemonOwnerEntity,
            Pokemon = pokemon
        };

        _context.Update(pokemonOwner);
        
        var pokemonCategory = new PokemonCategory()
        {
            Category = category,
            Pokemon = pokemon
        };

        _context.Update(pokemonCategory);
        
        _context.Update(pokemon);
        return Save();
    }

    public bool DeletePokemon(Pokemon pokemon)
    {
        var pokemonCategory = _context.PokemonCategories.Where(c => c.Pokemon == pokemon).First();
        _context.Remove(pokemonCategory);
        
        var pokemonOwner = _context.PokemonOwners.Where(p => p.Pokemon == pokemon).First();
        _context.Remove(pokemonOwner);
        
        var pokemonReviews = _context.Reviews.Where(p => p.Pokemon == pokemon).ToList();
        _context.RemoveRange(pokemonReviews);
        
        _context.Remove(pokemon);
        
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}