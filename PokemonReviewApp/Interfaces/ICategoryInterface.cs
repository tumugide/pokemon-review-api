using PokemonReviewApp.Model;

namespace PokemonReviewApp.Interfaces;

public interface ICategoryInterface
{
    ICollection<Category> GetCategories();
    Category GetCategory(int id);
    Category GetCategory(string name);
    ICollection<Pokemon> GetPokemonByCategory(int categoryId);
    bool CategoryExists(int categoryId);
    bool CreateCategory(Category category);
    bool Save();
}