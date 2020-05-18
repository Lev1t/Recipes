using Recipes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Models
{
    public class RecipeDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TimeToCook { get; set; }
        public string Method { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public bool CanEdit { get; set; }
        public string CreatedById { get; set; }
        public ICollection<CreateIngridientCommand> Ingridients { get; set; }

        public static RecipeDetailViewModel FromRecipe(Recipe recipe)
        {
            if (recipe == null) return new RecipeDetailViewModel();
            return new RecipeDetailViewModel
            {
                Id = recipe.RecipeId,
                Name = recipe.Name,
                TimeToCook = recipe.TimeToCook.Hours > 0
                                            ? $"{recipe.TimeToCook.Hours} hrs {recipe.TimeToCook.Minutes} mins"
                                            : $"{recipe.TimeToCook.Minutes} mins",
                Method = recipe.Method,
                IsVegetarian = recipe.IsVegetarian,
                IsVegan = recipe.IsVegan,
                CreatedById = recipe.CreatedById,
                Ingridients = recipe.Ingridients?.Select(i => new CreateIngridientCommand
                {
                    Name = i.Name,
                    Quantity = i.Quantity,
                    Unit = i.Unit
                })?.ToList()
            };
        }
    }
}
