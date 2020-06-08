using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Recipes.Models;

namespace Recipes.Data.Mapper
{
    public class TimeSpanToStringResolver : IValueResolver<Recipe, RecipeBaseViewModel, string>
    {
        public string Resolve(Recipe source, RecipeBaseViewModel destination, string destMember, ResolutionContext context)
        {
            return source.TimeToCook.Hours > 0
                    ? $"{source.TimeToCook.Hours} hrs {source.TimeToCook.Minutes} mins"
                    : $"{source.TimeToCook.Minutes} mins";
        }
    }
}
