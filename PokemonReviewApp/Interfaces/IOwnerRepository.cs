using PokemonReviewApp.Model;

namespace PokemonReviewApp.Interfaces;

public interface IOwnerRepository
{
    ICollection<Owner> GetOwners();
    Owner GetOwner(int id);
    ICollection<Owner> GetOwnerOfPokemon(int pokemonId);
    ICollection<Pokemon> GetPokemonByOwner(int ownerId);
    bool OwnerExists(int ownerId);
    bool CreateOwner(Owner owner);
    bool UpdateOwner(Owner owner);
    bool DeleteOwner(Owner owner);
    bool Save();

}