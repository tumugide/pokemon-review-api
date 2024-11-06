using PokemonReviewApp.Model;

namespace PokemonReviewApp.Interfaces;

public interface ICountryRepository
{
    ICollection<Country> GetCountries();
    Country GetCountry(int id);
    bool CountryExists(int id);
    Country GetCountryByOwner(int OwnerId);
    ICollection<Owner> GetOwnersByCountry(int CountryId);
    bool CreateCountry(Country country);
    bool UpdateCountry(Country country);
    bool DeleteCountry(Country country);
    bool Save();
}