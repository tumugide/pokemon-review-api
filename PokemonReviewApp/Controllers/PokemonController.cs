using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonController : Controller
{
    private readonly IPokemonInterface _pokemonInterface;
    public PokemonController(IPokemonInterface pokemonInterface)
    {
        this._pokemonInterface = pokemonInterface;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    public IActionResult GetPokemons()
    {
        var pokemons = this._pokemonInterface.GetPokemons();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(pokemons);
    }
    
}