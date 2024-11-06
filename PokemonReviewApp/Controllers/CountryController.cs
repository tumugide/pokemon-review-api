using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Dto.CreateDto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountryController(ICountryRepository countryRepository, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<CountryDto>))]
    [ProducesResponseType(400)]
    public IActionResult GetCountries()
    {
        var countries = mapper.Map<List<CountryDto>>(countryRepository.GetCountries());
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(countries);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(CountryDto))]
    [ProducesResponseType(404)]
    public IActionResult GetCountry(int id)
    {
        if (!countryRepository.CountryExists(id))
            return NotFound();
        
        var country = countryRepository.GetCountry(id);
        var mappedCountry = mapper.Map<CountryDto>(country);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedCountry);
    }

    [HttpGet("country/{ownerId}")]
    [ProducesResponseType(200, Type = typeof(CountryDto))]
    [ProducesResponseType(404)]
    public IActionResult GetCountryByOwner(int ownerId)
    {
        var country = countryRepository.GetCountryByOwner(ownerId);
        var mappedCountry = mapper.Map<CountryDto>(country);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(mappedCountry);
    }

    [HttpGet("owners/{countryId}")]
    [ProducesResponseType(200, Type = typeof(List<OwnerDto>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetOwnersByCountry(int countryId)
    {
        if (!countryRepository.CountryExists(countryId))
            return NotFound();
        
        var owners = countryRepository.GetOwnersByCountry(countryId);
        var mappedOwners = mapper.Map<List<OwnerDto>>(owners);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(mappedOwners);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult CreateCountry([FromBody] CountryDto countryDto)
    {
        if (countryDto == null)
            return BadRequest(ModelState);

        var country = countryRepository.GetCountries().Where(c => c.Name == countryDto.Name).FirstOrDefault();
        if (country !=null)
        {
            ModelState.AddModelError("","Country already exist");
            return StatusCode(422, ModelState);
        }
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var countryEntity = mapper.Map<Country>(countryDto);
        if (!countryRepository.CreateCountry(countryEntity))
        {
           ModelState.AddModelError("","Error occured while creating the country");
           return StatusCode(500, ModelState);
        }
        
        return Ok("Country created");
    }
    
    [HttpPut("{countryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult UpdateCountry(int countryId, [FromBody] AddCountryDto updatedCountryDto)
    {
        if(updatedCountryDto == null)
            return BadRequest(ModelState);
        
        if(!countryRepository.CountryExists(countryId))
            return NotFound();
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var countryMap = mapper.Map<Country>(updatedCountryDto); // The actual saving line

        countryMap.Id = countryId;
        if (!countryRepository.UpdateCountry(countryMap))
        {
            ModelState.AddModelError("error","Error while updating country");
            return StatusCode(500, ModelState);
        }

        return Ok("Country updated");

    }
}