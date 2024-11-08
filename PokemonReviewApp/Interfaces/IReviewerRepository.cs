using PokemonReviewApp.Model;

namespace PokemonReviewApp.Interfaces;

public interface IReviewerRepository
{
    ICollection<Reviewer> GetReviewers();
    Reviewer GetReviewer(int id);
    bool ReviewerExists(int id);
    ICollection<Review> GetReviewerReviews(int reviewerId);
    bool CreateReviewer (Reviewer reviewer);
    bool UpdateReviewer (Reviewer reviewer);
    bool DeleteReviewer(Reviewer reviewer);
    bool Save();
    
}