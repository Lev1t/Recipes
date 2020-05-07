using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Services;
using Microsoft.AspNetCore.Mvc;

namespace Recipes.Filters
{
    public class EnsureRecipeExistsAttribute : TypeFilterAttribute
    {
        public EnsureRecipeExistsAttribute() : base(typeof(EnsureRecipeFilter)) { }

        public class EnsureRecipeFilter : IActionFilter
        {
            RecipeService _service;
            public EnsureRecipeFilter(RecipeService service) => _service = service;
            public void OnActionExecuted(ActionExecutedContext context)
            {
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                var recipeId = (int)context.ActionArguments["id"];
                if(!_service.DoesRecipeExist(recipeId))
                {
                    context.Result = new NotFoundResult();
                }
            }
        }
    }
}
