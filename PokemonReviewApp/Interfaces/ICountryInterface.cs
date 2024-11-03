using PokemonReviewApp.Model;

namespace PokemonReviewApp.Interfaces;

public interface ICountryInterface
{
    ICollection<Country> GetCountries();
    Country GetCountry(int id);
    bool CountryExists(int id);
}