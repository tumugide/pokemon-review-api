using PokemonReviewApp.Model;

namespace PokemonReviewApp.Interfaces;

public interface IReviewerInterface
{
    ICollection<Reviewer> GetReviewers();
    Reviewer GetReviewer(int id);
    bool ReviewerExists(int id);
    ICollection<Review> GetReviewerReviews(int reviewerId);
}