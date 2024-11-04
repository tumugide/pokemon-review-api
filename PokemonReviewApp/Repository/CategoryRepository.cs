using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Repository;

public class CategoryRepository :ICategoryInterface
{
    private readonly DataContext _context;

    public CategoryRepository(DataContext context)
    {
        _context = context;
    }
    public ICollection<Category> GetCategories()
    {
        return _context.Categories.OrderBy(c => c.Name).ToList();
    }

    public Category GetCategory(int id)
    {
       // return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
       return _context.Categories.Find(id);
    }

    public Category GetCategory(string name)
    {
       return _context.Categories.Where(c => c.Name == name).FirstOrDefault();
    }

    public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
    {
        return _context.PokemonCategories.Where(pc => pc.CategoryId == categoryId).Select(p => p.Pokemon).ToList();
    }

    public bool CategoryExists(int categoryId)
    {
       return _context.Categories.Any(c => c.Id == categoryId);
    }

    public bool CreateCategory(Category category)
    {
        _context.Add(category);

       return Save();
    }

    public bool Save()
    {
        var save = _context.SaveChanges();
        return save > 0 ? true : false;
    }
}