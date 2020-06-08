using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Recipes.Models;

namespace Recipes.Data.Mapper
{
    public class HoursAndMinsToTimeSpanResolver : IValueResolver<EditRecipeBase, Recipe, TimeSpan>
    {
        public TimeSpan Resolve(EditRecipeBase source, Recipe destination, TimeSpan destMember, ResolutionContext context)
        {
            return new TimeSpan(source.TimeToCookHrs, source.TimeToCookMins, 0);
        }
    }
}
