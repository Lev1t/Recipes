using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;

namespace Recipes.Models
{
    public class RecipeSummaryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TimeToCook { get; set; }
        public int NumberOfIngridients { get; set; }

        public static RecipeSummaryViewModel FromRecipe(Recipe recipe)
        {
            return new RecipeSummaryViewModel
                {
                    Id = recipe.RecipeId,
                    Name = recipe.Name,
                    TimeToCook = recipe.TimeToCook.Hours > 0
                                                ? $"{recipe.TimeToCook.Hours} hrs {recipe.TimeToCook.Minutes} mins"
                                                : $"{recipe.TimeToCook.Minutes} mins",
                    NumberOfIngridients = recipe.Ingridients.Count
            };
        }
    }
}
