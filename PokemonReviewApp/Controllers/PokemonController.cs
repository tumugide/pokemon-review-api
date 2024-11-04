using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonController : ControllerBase
{
    private readonly IPokemonInterface _pokemonInterface;
    private readonly IMapper _mapper;
    public PokemonController(IPokemonInterface pokemonInterface, IMapper mapper)
    {
        _pokemonInterface = pokemonInterface;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDto>))]
    public IActionResult GetPokemons()
    {
        var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonInterface.GetPokemons());
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(pokemons);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(PokemonDto))]
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