using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Dto.CreateDto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OwnerController(IOwnerRepository ownerRepository, IMapper mapper, ICountryRepository countryRepository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<OwnerDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetOwners()
    {
        var owners = ownerRepository.GetOwners();
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
        if (!ownerRepository.OwnerExists(id))
            return NotFound();
        var owner = ownerRepository.GetOwner(id);
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
        var owners = ownerRepository.GetOwnerOfPokemon(pokemonId);
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
        if (!ownerRepository.OwnerExists(ownerId))
            return NotFound();
        var pokemon = ownerRepository.GetPokemonByOwner(ownerId);
        var mappedPokemon = mapper.Map<List<PokemonDto>>(pokemon);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(mappedPokemon);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto ownerDto)
    {
        // Start of Validations
        if (ownerDto == null)
            return BadRequest(ModelState);

        var existingOwner = ownerRepository
            .GetOwners().FirstOrDefault(o=> o.LastName.Trim().ToUpper() == ownerDto.LastName.TrimEnd().ToUpper());

        if (existingOwner != null)
        {
            ModelState.AddModelError("", "User with such names already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        // End of validations

        var ownerMap = mapper.Map<Owner>(ownerDto);
        // Adding the country relation
        ownerMap.Country = countryRepository.GetCountry(countryId);
        
        var newOwner = ownerRepository.CreateOwner(ownerMap);
        
        if (!newOwner)
        {
            ModelState.AddModelError("","Error occured while creating the owner");
            return StatusCode(500, ModelState);
        }
        
        return Ok("Owner created");

    }
    
    [HttpPut("{ownerId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult UpdateOwner(int ownerId, [FromBody] UpdateOwnerDto updatedOwnerDto)
    {
        if(updatedOwnerDto == null)
            return BadRequest(ModelState);
        
        if(!ownerRepository.OwnerExists(ownerId))
            return NotFound();
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var ownerMap = mapper.Map<Owner>(updatedOwnerDto);
        ownerMap.Id = ownerId;
        
        if (ownerRepository.UpdateOwner(ownerMap))
            return Ok("Owner updated");
        
        ModelState.AddModelError("error","Error while updating owner");
        return StatusCode(500, ModelState);
    }
    
    [HttpDelete("{ownerId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteOwner(int ownerId)
    {
        if(!ownerRepository.OwnerExists(ownerId))
            return NotFound();
        
        var owner = ownerRepository.GetOwner(ownerId);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if(ownerRepository.DeleteOwner(owner))
            return Ok("owner deleted");
        
        ModelState.AddModelError("error","Error while updating owner");
        return StatusCode(500, ModelState);
    }
}