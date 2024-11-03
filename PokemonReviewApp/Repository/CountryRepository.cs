using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Repository;

public class CountryRepository :ICountryInterface
{
    private readonly DataContext _context;

    public CountryRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Country> GetCountries()
    {
        return _context.Countries.ToList();
    }

    public Country GetCountry(int id)
    {
        // return _context.Countries.Find(id);
       return _context.Countries.Where(c => c.Id == id).FirstOrDefault();
    }

    public bool CountryExists(int id)
    {
       return _context.Countries.Any(c => c.Id == id);
    }
}