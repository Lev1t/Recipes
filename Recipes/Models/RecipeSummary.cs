using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;

namespace Recipes.Models
{
    public class RecipeSummaryViewModel :RecipeBaseViewModel
    {
        public int NumberOfIngridients { get; set; }
    }
}
