using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Dto.CreateDto;
using PokemonReviewApp.Model;

namespace PokemonReviewApp.Helper;

public class MappingProfiles :Profile
{
    public MappingProfiles()
    {
        CreateMap<Pokemon, PokemonDto>();
        CreateMap<AddPokemonDto, Pokemon>();
        
        CreateMap<Category, CategoryDto>();
        CreateMap<AddCategoryDto, Category>();
        
        CreateMap<Country, CountryDto>();
        CreateMap<CountryDto, Country>();
        
        CreateMap<Owner, OwnerDto>();
        CreateMap<OwnerDto, Owner>();
        
        CreateMap<Review, ReviewDto>();
        CreateMap<AddReviewDto, Review>();
        
        CreateMap<Reviewer, ReviewerDto>();
        CreateMap<AddReviewerDto, Reviewer>();
    }
}