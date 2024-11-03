using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewerController :ControllerBase
{
    private readonly IReviewerInterface _reviewerInterface;
    private readonly IMapper _mapper;

    public ReviewerController(IReviewerInterface reviewerInterface, IMapper mapper)
    {
        _reviewerInterface = reviewerInterface;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<ReviewerDto>))]
    public IActionResult GetReviewers()
    {
        var reviewers = _reviewerInterface.GetReviewers();
        var mappedReviewers = _mapper.Map<List<ReviewerDto>>(reviewers);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedReviewers);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(ReviewerDto))]
    [ProducesResponseType(404)]
    public IActionResult GetReviewer(int id)
    {
        if (!_reviewerInterface.ReviewerExists(id))
            return NotFound();
        var reviewer = _reviewerInterface.GetReviewer(id);
        var mappedReviewer = _mapper.Map<ReviewerDto>(reviewer);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedReviewer);
    }

    [HttpGet("/reviews/{reviewerId}")]
    [ProducesResponseType(200, Type = typeof(List<ReviewDto>))]
    [ProducesResponseType(404)]
    public IActionResult GetReviewsByReviewer(int reviewerId)
    {
        if(!_reviewerInterface.ReviewerExists(reviewerId))
            return NotFound();
        
        var reviews = _reviewerInterface.GetReviewerReviews(reviewerId);
        var mappedReviews = _mapper.Map<List<ReviewDto>>(reviews);
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(mappedReviews);
    }
}