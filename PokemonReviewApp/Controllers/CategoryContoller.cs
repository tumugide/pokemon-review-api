using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CategoryContoller :Controller
{
    private readonly ICategoryInterface _categoryInterface;
    private readonly IMapper _mapper;

    public CategoryContoller(ICategoryInterface categoryInterface, IMapper mapper)
    {
        _categoryInterface = categoryInterface;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
    public IActionResult getCategories()
    {
        var categories = _categoryInterface.GetCategories();
        var mappedCategories = _mapper.Map<List<CategoryDto>>(categories);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedCategories);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(Category))]
    [ProducesResponseType(404)]
    public IActionResult getCategory(int id)
    {
        if (!_categoryInterface.CategoryExists(id))
            return NotFound();
        
        var category = _categoryInterface.GetCategory(id);
        var mappedCategory = _mapper.Map<CategoryDto>(category);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedCategory);
    }

    [HttpGet("{categoryId}/pokemon")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    [ProducesResponseType(400)]
    public IActionResult getPokemonByCategory(int categoryId)
    {
        if(!_categoryInterface.CategoryExists(categoryId))
            return NotFound();
        
        var pokemon = _categoryInterface.GetPokemonByCategory(categoryId);
        var mappedPokemon = _mapper.Map<List<PokemonDto>>(pokemon);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedPokemon);
    }
}