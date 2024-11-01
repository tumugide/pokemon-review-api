using PokemonReviewApp.Model;

namespace PokemonReviewApp.Interfaces;

public interface IPokemonInterface
{
    ICollection<Pokemon> GetPokemons();
}