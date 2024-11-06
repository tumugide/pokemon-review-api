namespace PokemonReviewApp.Dto.CreateDto;

public class AddReviewDto
{
    public string Title { get; set; }
    public string Text { get; set; }

    public int Rating { get; set; }
    
    public int ReviewerId { get; set; }
    public int PokemonId { get; set; }
}