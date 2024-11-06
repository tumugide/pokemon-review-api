using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Repository;

public class CountryRepository :ICountryRepository
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
        return _context.Countries.Find(id);
       // return _context.Countries.Where(c => c.Id == id).FirstOrDefault();
    }

    public bool CountryExists(int id)
    {
       return _context.Countries.Any(c => c.Id == id);
    }

    public Country GetCountryByOwner(int OwnerId)
    {
        return _context.Owners.Where(o =>o.Id == OwnerId).Select(c => c.Country).FirstOrDefault();
    }

    public ICollection<Owner> GetOwnersByCountry(int CountryId)
    {
        return _context.Owners.Where(c => c.Country.Id == CountryId).ToList();
    }

    public bool CreateCountry(Country country)
    {
        _context.Add(country);
        return Save();
    }

    public bool UpdateCountry(Country country)
    {
        _context.Update(country);
        return Save();
    }

    public bool DeleteCountry(Country country)
    {
        _context.Remove(country);
        return Save();
    }

    public bool Save()
    {
        var save = _context.SaveChanges();
        
        return save > 0 ? true : false;
    }
}