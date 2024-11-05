using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<ReviewerDto>))]
    public IActionResult GetReviewers()
    {
        var reviewers = reviewerRepository.GetReviewers();
        var mappedReviewers = mapper.Map<List<ReviewerDto>>(reviewers);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedReviewers);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(ReviewerDto))]
    [ProducesResponseType(404)]
    public IActionResult GetReviewer(int id)
    {
        if (!reviewerRepository.ReviewerExists(id))
            return NotFound();
        var reviewer = reviewerRepository.GetReviewer(id);
        var mappedReviewer = mapper.Map<ReviewerDto>(reviewer);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedReviewer);
    }

    [HttpGet("/reviews/{reviewerId}")]
    [ProducesResponseType(200, Type = typeof(List<ReviewDto>))]
    [ProducesResponseType(404)]
    public IActionResult GetReviewsByReviewer(int reviewerId)
    {
        if(!reviewerRepository.ReviewerExists(reviewerId))
            return NotFound();
        
        var reviews = reviewerRepository.GetReviewerReviews(reviewerId);
        var mappedReviews = mapper.Map<List<ReviewDto>>(reviews);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedReviews);
    }
}