using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonController : Controller
{
    private readonly IPokemonInterface _pokemonInterface;
    private readonly IMapper _mapper;
    public PokemonController(IPokemonInterface pokemonInterface, IMapper mapper)
    {
        _pokemonInterface = pokemonInterface;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    public IActionResult GetPokemons()
    {
        var pokemons = _pokemonInterface.GetPokemons();
        var mappedPokemons = _mapper.Map<List<PokemonDto>>(pokemons);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(mappedPokemons);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(Pokemon))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemon(int id)
    {
        if (!_pokemonInterface.PokemonExists(id))
            return NotFound();
        var pokemon = _pokemonInterface.GetPokemon(id);
        var mappedPoke = _mapper.Map<PokemonDto>(pokemon);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(mappedPoke);
    }

    [HttpGet("{pokemonId}/rating")]
    [ProducesResponseType(200, Type = typeof(decimal))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonRating(int pokemonId)
    {
        if (!_pokemonInterface.PokemonExists(pokemonId))
            return NotFound();

        var rating = _pokemonInterface.GetPokemonRating(pokemonId);
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(rating);
    }
    
}