using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Helper;

public class MappingProfiles :Profile
{
    public MappingProfiles()
    {
        CreateMap<Pokemon, PokemonDto>();
        CreateMap<Category, CategoryDto>();
    }
}