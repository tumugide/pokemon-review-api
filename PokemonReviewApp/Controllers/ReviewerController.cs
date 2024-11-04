using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewerController(IReviewerInterface reviewerInterface, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<ReviewerDto>))]
    public IActionResult GetReviewers()
    {
        var reviewers = reviewerInterface.GetReviewers();
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
        if (!reviewerInterface.ReviewerExists(id))
            return NotFound();
        var reviewer = reviewerInterface.GetReviewer(id);
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
        if(!reviewerInterface.ReviewerExists(reviewerId))
            return NotFound();
        
        var reviews = reviewerInterface.GetReviewerReviews(reviewerId);
        var mappedReviews = mapper.Map<List<ReviewDto>>(reviews);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedReviews);
    }
}