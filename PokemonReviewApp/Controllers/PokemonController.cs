using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Dto.CreateDto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonController : ControllerBase
{
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IMapper _mapper;
    public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
    {
        _pokemonRepository = pokemonRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDto>))]
    public IActionResult GetPokemons()
    {
        var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(pokemons);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(PokemonDto))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemon(int id)
    {
        if (!_pokemonRepository.PokemonExists(id))
            return NotFound();
        var pokemon = _pokemonRepository.GetPokemon(id);
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
        if (!_pokemonRepository.PokemonExists(pokemonId))
            return NotFound();

        var rating = _pokemonRepository.GetPokemonRating(pokemonId);
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(rating);
    }
    
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult CreatePokemon([FromBody] AddPokemonDto pokemonDto)
    {
        // Start of Validations
        if (pokemonDto == null)
            return BadRequest(ModelState);

        var existingPokemon = _pokemonRepository
            .GetPokemons().FirstOrDefault(o=> o.Name.Trim().ToUpper() == pokemonDto.Name.TrimEnd().ToUpper());

        if (existingPokemon != null)
        {
            ModelState.AddModelError("", "Pokemon with such name already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        // End of validations

        var pokemonMap = _mapper.Map<Pokemon>(pokemonDto);
        // Adding the country relation
        
        var newPokemon = _pokemonRepository.CreatePokemon(pokemonDto.OwnerId, pokemonDto.CategoryId, pokemonMap);
        
        if (newPokemon)
            return Ok("Pokemon created");
        
        ModelState.AddModelError("","Error occured while creating the pokemon");
        return StatusCode(500, ModelState);

    }
    
    [HttpPut("{pokemonId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult UpdatePokemon(int pokemonId, [FromBody] AddPokemonDto updatedPokemonDto)
    {
        if(updatedPokemonDto == null)
            return BadRequest(ModelState);
        
        if(!_pokemonRepository.PokemonExists(pokemonId))
            return NotFound();
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var pokemonMap = _mapper.Map<Pokemon>(updatedPokemonDto);
        pokemonMap.Id = pokemonId;
        
        if (_pokemonRepository.UpdatePokemon(updatedPokemonDto.OwnerId,updatedPokemonDto.CategoryId,pokemonMap))
            return Ok("Pokemon updated");
        
        ModelState.AddModelError("error","Error while updating pokemon");
        return StatusCode(500, ModelState);

    }
    
    [HttpDelete("{pokemonId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeletePokemon(int pokemonId)
    {
        if(!_pokemonRepository.PokemonExists(pokemonId))
            return NotFound();
        
        var pokemon = _pokemonRepository.GetPokemon(pokemonId);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if(_pokemonRepository.DeletePokemon(pokemon))
            return Ok("Pokemon and it's related are deleted");
        
        ModelState.AddModelError("error","Error while updating pokemon");
        return StatusCode(500, ModelState);
    }
    
}