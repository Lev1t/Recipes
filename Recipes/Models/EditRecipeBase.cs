using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Recipes.Models
{
    public class EditRecipeBase
    {
        [Required, StringLength(100)]
        public string Name { get; set; }
        [Range(0, 24), DisplayName("Hours")]
        public int TimeToCookHrs { get; set; }
        [Range(0, 59), DisplayName("Minutes")]
        public int TimeToCookMins { get; set; }
        public string Method { get; set; }
        [DisplayName("Vegetarian")]
        public bool IsVegetarian { get; set; }
        [DisplayName("Vegan")]
        public bool IsVegan { get; set; }
        public IList<CreateIngridientCommand> Ingridients { get; set; } = new List<CreateIngridientCommand>();
    }
}
