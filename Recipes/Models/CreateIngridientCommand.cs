using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;

namespace Recipes.Models
{
    public class CreateIngridientCommand
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }

        public Ingridient ToIngridient()
        {
            return new Ingridient
            {
                Name = Name,
                Quantity = Quantity,
                Unit = Unit
            };
        }
    }
}
