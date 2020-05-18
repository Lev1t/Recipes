using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;
using Recipes.Models;

namespace Recipes.Models
{
    public class CreateRecipeCommand : EditRecipeBase
    {
        public Recipe ToRecipe(User user)
        {
            return new Recipe
            {
                Name = Name,
                TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0),
                IsDeleted = false,
                Method = Method,
                IsVegetarian = IsVegetarian,
                IsVegan = IsVegan,
                CreatedById = user.Id,
                Ingridients = Ingridients?.Select(i => i.ToIngridient()).ToList()
            };
        }
    }
}
