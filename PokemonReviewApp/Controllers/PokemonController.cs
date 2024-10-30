using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonController : ControllerBase
{
    private readonly DataContext _context;

    public PokemonController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("test")]
    public IActionResult TestConnection()
    {
        try
        {
            var canConnect = _context.Database.CanConnect();
            return Ok($"Database connection: {canConnect}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }
}