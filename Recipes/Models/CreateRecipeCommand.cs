using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;

namespace Recipes.Models
{
    public class CreateRecipeCommand : EditRecipeBase
    {
        public Recipe ToRecipe()
        {
            return new Recipe
            {
                Name = Name,
                TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0),
                IsDeleted = false,
                Method = Method,
                IsVegetarian = IsVegetarian,
                IsVegan = IsVegan,
                Ingridients = Ingridients?.Select(i => i.ToIngridient()).ToList()
            };
        }
    }
}
