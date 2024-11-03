using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountryController :ControllerBase
{
    private readonly ICountryInterface _countryInterface;
    private readonly IMapper _mapper;

    public CountryController(ICountryInterface countryInterface, IMapper mapper)
    {
        _countryInterface = countryInterface;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<Country>))]
    [ProducesResponseType(400)]
    public IActionResult GetCountries()
    {
        var countries = _countryInterface.GetCountries();
        var mappedCounties = _mapper.Map<List<CountryDto>>(countries);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedCounties);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(Country))]
    [ProducesResponseType(404)]
    public IActionResult GetCountry(int id)
    {
        if (!_countryInterface.CountryExists(id))
            return NotFound();
        
        var country = _countryInterface.GetCountry(id);
        var mappedCountry = _mapper.Map<CountryDto>(country);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedCountry);
    }
}