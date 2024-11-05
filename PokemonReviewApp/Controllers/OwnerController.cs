using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OwnerController(IOwnerInterface ownerInterface, IMapper mapper, ICountryInterface countryInterface) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<OwnerDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetOwners()
    {
        var owners = ownerInterface.GetOwners();
        var mappedOwners = mapper.Map<List<OwnerDto>>(owners);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(mappedOwners);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(OwnerDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public IActionResult GetOwner(int id)
    {
        if (!ownerInterface.OwnerExists(id))
            return NotFound();
        var owner = ownerInterface.GetOwner(id);
        var mappedOwner = mapper.Map<OwnerDto>(owner);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(mappedOwner);
    }

    [HttpGet("owner/{pokemonId}")]
    [ProducesResponseType(200, Type = typeof(List<OwnerDto>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetOwnersByPokemon(int pokemonId)
    {
        var owners = ownerInterface.GetOwnerOfPokemon(pokemonId);
        var mappedOwners = mapper.Map<List<OwnerDto>>(owners);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(mappedOwners);
    }

    [HttpGet("pokemon/{ownerId}")]
    [ProducesResponseType(200, Type = typeof(List<PokemonDto>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetPokemonsByOwner(int ownerId)
    {
        if (!ownerInterface.OwnerExists(ownerId))
            return NotFound();
        var pokemon = ownerInterface.GetPokemonByOwner(ownerId);
        var mappedPokemon = mapper.Map<List<PokemonDto>>(pokemon);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(mappedPokemon);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult CreateOwner([FromBody] OwnerDto ownerDto)
    {
        // Start of Validations
        if (ownerDto == null)
            return BadRequest(ModelState);

        var existingOwner = ownerInterface
            .GetOwners().FirstOrDefault(o=> o.LastName.Trim().ToUpper() == ownerDto.LastName.TrimEnd().ToUpper());

        if (existingOwner != null)
        {
            ModelState.AddModelError("", "User with such names already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        // End of validations

        var ownerEntity = mapper.Map<Owner>(ownerDto);
        // Adding the country relation
        ownerEntity.Country = countryInterface.GetCountry(ownerDto.CountryId);
        
        var newOwner = ownerInterface.CreateOwner(ownerEntity);
        
        if (!newOwner)
        {
            ModelState.AddModelError("","Error occured while creating the owner");
            return StatusCode(500, ModelState);
        }
        
        return Ok("Owner created");

    }
}