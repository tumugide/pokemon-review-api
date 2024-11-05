using PokemonReviewApp.Model;

namespace PokemonReviewApp.Interfaces;

public interface IPokemonRepository
{
    ICollection<Pokemon> GetPokemons(); // Getting all pokemons
    
    Pokemon GetPokemon(int id); // Getting a single pokemon by id
    Pokemon GetPokemon(string name); // Getting as single pokemon by name
    decimal GetPokemonRating(int pokemonId);
    bool PokemonExists(int pokemonId);

    bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
    bool Save();
}