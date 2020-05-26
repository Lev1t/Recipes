using Recipes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Models
{
    public class RecipeDetailViewModel : RecipeBaseViewModel
    {
        public string Method { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public bool CanEdit { get; set; }
        public string CreatedById { get; set; }
        public ICollection<CreateIngridientCommand> Ingridients { get; set; }
    }
}
