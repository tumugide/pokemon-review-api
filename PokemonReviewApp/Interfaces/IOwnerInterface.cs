using PokemonReviewApp.Model;

namespace PokemonReviewApp.Interfaces;

public interface IOwnerInterface
{
    ICollection<Owner> GetOwners();
    Owner GetOwner(int id);
    ICollection<Owner> GetOwnerOfPokemon(int pokemonId);
    ICollection<Pokemon> GetPokemonByOwner(int ownerId);
    bool OwnerExists(int ownerId);
}