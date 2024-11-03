using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Repository;

public class ReviewRepository : IReviewInterface
{
    private readonly DataContext _context;

    public ReviewRepository(DataContext context)
    {
        _context = context;
    }
    
    public ICollection<Review> GetReviews()
    {
        return _context.Reviews.ToList();
    }

    public Review GetReview(int id)
    {
        return _context.Reviews.Find(id);
    }

    public bool ReviewExists(int id)
    {
        return _context.Reviews.Any(r => r.Id == id);
    }

    public ICollection<Review> GetReviewsOfAPokemon(int pokemonId)
    {
        return _context.Reviews.Where(r => r.Pokemon.Id == pokemonId).ToList();
    }
}