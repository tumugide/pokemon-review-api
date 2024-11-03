using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OwnerController :ControllerBase
{
    private readonly IOwnerInterface _ownerInterface;
    private readonly IMapper _mapper;

    public OwnerController(IOwnerInterface ownerInterface, IMapper mapper)
    {
        _ownerInterface = ownerInterface;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<Owner>))]
    [ProducesResponseType(400)]
    public IActionResult GetOwners()
    {
        var owners = _ownerInterface.GetOwners();
        var mappedOwners = _mapper.Map<List<OwnerDto>>(owners);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedOwners);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(Owner))]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public IActionResult GetOwner(int id)
    {
        if(!_ownerInterface.OwnerExists(id))
            return NotFound();
        var owner = _ownerInterface.GetOwner(id);
        var mappedOwner = _mapper.Map<OwnerDto>(owner);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(mappedOwner);
    }

    [HttpGet("owner/{pokemonId}")]
    [ProducesResponseType(200, Type = typeof(List<Owner>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetOwnersByPokemon(int pokemonId)
    {
        var owners = _ownerInterface.GetOwnerOfPokemon(pokemonId);
        var mappedOwners = _mapper.Map<List<OwnerDto>>(owners);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(mappedOwners);
    }

    [HttpGet("pokemon/{ownerId}")]
    [ProducesResponseType(200, Type = typeof(List<Pokemon>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetPokemonsByOwner(int ownerId)
    {
        if (!_ownerInterface.OwnerExists(ownerId))
            return NotFound();
        var pokemon = _ownerInterface.GetPokemonByOwner(ownerId);
        var mappedPokemon = _mapper.Map<List<PokemonDto>>(pokemon);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(mappedPokemon);
    }
}