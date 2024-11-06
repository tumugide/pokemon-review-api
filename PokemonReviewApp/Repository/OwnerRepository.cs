using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Repository;

public class OwnerRepository : IOwnerRepository
{
    private readonly DataContext _context;

    public OwnerRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Owner> GetOwners()
    {
        return _context.Owners.ToList();
    }

    public Owner GetOwner(int id)
    {
        return _context.Owners.Find(id);
    }

    public ICollection<Owner> GetOwnerOfPokemon(int pokemonId)
    {
        return _context.PokemonOwners.Where(p => p.PokemonId == pokemonId).Select(p => p.Owner).ToList();
    }

    public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
    {
        return _context.PokemonOwners.Where(p => p.OwnerId == ownerId).Select(p => p.Pokemon).ToList();
    }

    public bool OwnerExists(int ownerId)
    {
        return _context.Owners.Any(o => o.Id == ownerId);
    }

    public bool CreateOwner(Owner owner)
    {
        _context.Owners.Add(owner);
        return Save();
    }

    public bool UpdateOwner(Owner owner)
    {
        _context.Update(owner);
        return Save();
    }

    public bool Save()
    {
        return (_context.SaveChanges()) > 0 ? true : false;
    }
}