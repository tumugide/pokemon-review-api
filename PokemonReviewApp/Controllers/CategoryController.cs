using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CategoryController(ICategoryRepository categoryRepository, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
    public IActionResult getCategories()
    {
        var categories = categoryRepository.GetCategories();
        var mappedCategories = mapper.Map<List<CategoryDto>>(categories);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedCategories);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(Category))]
    [ProducesResponseType(404)]
    public IActionResult GetCategory(int id)
    {
        if (!categoryRepository.CategoryExists(id))
            return NotFound();
        
        var category = categoryRepository.GetCategory(id);
        var mappedCategory = mapper.Map<CategoryDto>(category);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedCategory);
    }

    [HttpGet("{categoryId}/pokemon")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonByCategory(int categoryId)
    {
        if(!categoryRepository.CategoryExists(categoryId))
            return NotFound();
        
        var pokemon = categoryRepository.GetPokemonByCategory(categoryId);
        var mappedPokemon = mapper.Map<List<PokemonDto>>(pokemon);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedPokemon);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public IActionResult CreateCategory([FromBody] CategoryDto categoryDto)
    {
        if(categoryDto == null)
            return BadRequest(ModelState);
        
        var category = categoryRepository.GetCategories().Where(c => c.Name.Trim().ToUpper() == categoryDto.Name.Trim().ToUpper()).FirstOrDefault();

        if (category != null)
        {
            ModelState.AddModelError("error", "Category already exists");
            return StatusCode(422, ModelState);
        }
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var categoryEntity = mapper.Map<Category>(categoryDto); // The actual saving line
        
        if (!categoryRepository.CreateCategory(categoryEntity))
        {
            ModelState.AddModelError("error","Error while creating category");
            return StatusCode(500, ModelState);
        }

        return Ok("Category created");

    }
}