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
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public ReviewController(IReviewRepository reviewRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<Review>))]
    [ProducesResponseType(400)]
    public IActionResult GetReviews()
    {
        var reviews = _reviewRepository.GetReviews();
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
        if(!_reviewRepository.ReviewExists(id))
            return NotFound();
        var review = _reviewRepository.GetReview(id);
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
        var reviews = _reviewRepository.GetReviewsOfAPokemon(pokemonId);
        var mappedReviews = _mapper.Map<List<ReviewDto>>(reviews);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(mappedReviews);
    }
}