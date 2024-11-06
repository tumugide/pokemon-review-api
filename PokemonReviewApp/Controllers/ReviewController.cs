using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Dto.CreateDto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController :ControllerBase
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IPokemonRepository _pokemonRepository;

    public ReviewController(IReviewRepository reviewRepository, IMapper mapper, IReviewerRepository reviewerRepository, IPokemonRepository pokemonRepository)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
        _reviewerRepository = reviewerRepository;
        _pokemonRepository = pokemonRepository;
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
    
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult CreateReview([FromBody] AddReviewDto reviewDto)
    {
        // Start of Validations
        if (reviewDto == null)
            return BadRequest(ModelState);

        var existingReview = _reviewRepository
            .GetReviews().FirstOrDefault(o=> o.Title.Trim().ToUpper() == reviewDto.Title.TrimEnd().ToUpper());

        if (existingReview != null)
        {
            ModelState.AddModelError("", "Review with such title already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        // End of validations

        var reviewMap = _mapper.Map<Review>(reviewDto);
        // Adding the country relation
        reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewDto.ReviewerId);
        reviewMap.Pokemon = _pokemonRepository.GetPokemon(reviewDto.PokemonId);
        
        var newReview = _reviewRepository.CreateReview(reviewMap);
        
        if (!newReview)
        {
            ModelState.AddModelError("","Error occured while creating the reviewer");
            return StatusCode(500, ModelState);
        }
        
        return Ok("Review created");

    }
    
    [HttpPut("{reviewId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult UpdateReview(int reviewId, [FromBody] AddReviewDto updatedReviewDto)
    {
        if(updatedReviewDto == null)
            return BadRequest(ModelState);
        
        if(!_reviewRepository.ReviewExists(reviewId))
            return NotFound();
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var reviewMap = _mapper.Map<Review>(updatedReviewDto);
        reviewMap.Reviewer = _reviewerRepository.GetReviewer(updatedReviewDto.ReviewerId);
        reviewMap.Pokemon = _pokemonRepository.GetPokemon(updatedReviewDto.PokemonId);
        
        reviewMap.Id = reviewId;
        
        if (_reviewRepository.UpdateReview(reviewMap))
            return Ok("Review updated");
        
        ModelState.AddModelError("error","Error while updating review");
        return StatusCode(500, ModelState);
    }
    
    [HttpDelete("{reviewId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteReview(int reviewId)
    {
        if(!_reviewRepository.ReviewExists(reviewId))
            return NotFound();
        
        var review = _reviewRepository.GetReview(reviewId);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if(_reviewRepository.DeleteReview(review))
            return Ok("review deleted");
        
        ModelState.AddModelError("error","Error while updating review");
        return StatusCode(500, ModelState);
    }
}