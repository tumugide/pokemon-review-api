using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController :ControllerBase
{
    private readonly IReviewInterface _reviewInterface;
    private readonly IMapper _mapper;

    public ReviewController(IReviewInterface reviewInterface, IMapper mapper)
    {
        _reviewInterface = reviewInterface;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<Review>))]
    [ProducesResponseType(400)]
    public IActionResult GetReviews()
    {
        var reviews = _reviewInterface.GetReviews();
        var mappedReviews = _mapper.Map<List<ReviewDto>>(reviews);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedReviews);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(Review))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetReview(int id)
    {
        if(!_reviewInterface.ReviewExists(id))
            return NotFound();
        var review = _reviewInterface.GetReview(id);
        var mappedReview = _mapper.Map<ReviewDto>(review);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedReview);
    }

    [HttpGet("pokemon/{pokemonId}")]
    [ProducesResponseType(200, Type = typeof(List<Review>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetReviewsByPokemon(int pokemonId)
    {
        var reviews = _reviewInterface.GetReviewsOfAPokemon(pokemonId);
        var mappedReviews = _mapper.Map<List<ReviewDto>>(reviews);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(mappedReviews);
    }
}