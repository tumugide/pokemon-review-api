using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Repository;

public class ReviewerRepository :IReviewerRepository
{
    private readonly DataContext _context;

    public ReviewerRepository(DataContext context)
    {
        _context = context;
    }
    
    public ICollection<Reviewer> GetReviewers()
    {
       return _context.Reviewers.ToList();
    }

    public Reviewer GetReviewer(int id)
    {
        return _context.Reviewers.Where(r => r.Id == id).Include(r => r.Reviews).FirstOrDefault(); // Including the reviews with the result
    }

    public bool ReviewerExists(int id)
    {
        return _context.Reviewers.Any(e => e.Id == id);
    }

    public ICollection<Review> GetReviewerReviews(int reviewerId)
    {
        return _context.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
    }

    public bool CreateReviewer(Reviewer reviewer)
    {
        _context.Add(reviewer);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }
}