using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;

namespace Recipes.Models
{
    public class UpdateRecipeCommand : EditRecipeBase
    {
        public int Id { get; set; }

        public void UpdateRecipe(Recipe recipe)
        {
            recipe.Name = Name;
            recipe.TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0);
            recipe.Method = Method;
            recipe.IsVegetarian = IsVegetarian;
            recipe.IsVegan = IsVegan;
            recipe.Ingridients = Ingridients?.Select(x => x.ToIngridient()).ToList();
        }
    }
}
