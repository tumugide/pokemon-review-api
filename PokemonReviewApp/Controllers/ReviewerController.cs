using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Dto.CreateDto;
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
    
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult CreateOwner([FromBody] AddReviewerDto reviewerDto)
    {
        // Start of Validations
        if (reviewerDto == null)
            return BadRequest(ModelState);

        var existingReviewer = reviewerRepository
            .GetReviewers().FirstOrDefault(o=> o.LastName.Trim().ToUpper() == reviewerDto.LastName.TrimEnd().ToUpper());

        if (existingReviewer != null)
        {
            ModelState.AddModelError("", "Reviewer with such names already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        // End of validations

        var reviewerMap = mapper.Map<Reviewer>(reviewerDto);
        
        var newReviewer = reviewerRepository.CreateReviewer(reviewerMap);
        
        if (!newReviewer)
        {
            ModelState.AddModelError("","Error occured while creating the reviewer");
            return StatusCode(500, ModelState);
        }
        
        return Ok("Reviewer created");

    }
    
    [HttpPut("{reviewerId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult UpdateReviewer(int reviewerId, [FromBody] AddReviewerDto updatedReviewerDto)
    {
        if(updatedReviewerDto == null)
            return BadRequest(ModelState);
        
        if(!reviewerRepository.ReviewerExists(reviewerId))
            return NotFound();
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var reviewerMap = mapper.Map<Reviewer>(updatedReviewerDto);
        reviewerMap.Id = reviewerId;
        
        if (reviewerRepository.UpdateReviewer(reviewerMap))
            return Ok("Reviewer updated");
        
        ModelState.AddModelError("error","Error while updating reviewer");
        return StatusCode(500, ModelState);
    }
}