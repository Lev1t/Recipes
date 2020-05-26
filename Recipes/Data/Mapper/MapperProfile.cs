using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Recipes.Models;

namespace Recipes.Data.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateIngridientCommand, Ingridient>();
            CreateMap<CreateIngridientCommand, Ingridient>().ReverseMap();

            CreateMap<EditRecipeBase, Recipe>()
                .ForMember(dest => dest.TimeToCook, 
                opt => opt.MapFrom<HoursAndMinsToTimeSpanResolver>())
                    .ReverseMap()
                    .ForMember(dest => dest.TimeToCookHrs, opt => opt.MapFrom(s => s.TimeToCook.Hours))
                    .ForMember(dest => dest.TimeToCookMins, opt => opt.MapFrom(s => s.TimeToCook.Minutes));

            CreateMap<Recipe, RecipeSummaryViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.RecipeId))
                .ForMember(dest => dest.TimeToCook, opt => opt.MapFrom<TimeSpanToStringResolver>())
                .ForMember(dest => dest.NumberOfIngridients, opt => opt.MapFrom(source => source.Ingridients.Count));

            CreateMap<Recipe, RecipeDetailViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.RecipeId))
                .ForMember(dest => dest.TimeToCook, opt => opt.MapFrom<TimeSpanToStringResolver>()); ;

            CreateMap<Recipe, UpdateRecipeCommand>()
                .ForMember(dest => dest.TimeToCookHrs, opt => opt.MapFrom(s => s.TimeToCook.Hours))
                .ForMember(dest => dest.TimeToCookMins, opt => opt.MapFrom(s => s.TimeToCook.Minutes));
        }
    }
}
