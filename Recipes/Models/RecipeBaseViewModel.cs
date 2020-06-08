using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Models
{
    public class RecipeBaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TimeToCook { get; set; }
    }
}
