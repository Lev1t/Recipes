using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Services;
using Microsoft.AspNetCore.Mvc;
using Recipes.Controllers;

namespace Recipes.Filters
{
    public class EnsureRecipeExistsAttribute : TypeFilterAttribute
    {
        public EnsureRecipeExistsAttribute() : base(typeof(EnsureRecipeFilter)) { }

        public class EnsureRecipeFilter : IAsyncActionFilter
        {
            RecipeService _service;
            public EnsureRecipeFilter(RecipeService service) => _service = service;

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ModelState.IsValid)
                {
                    var recipeId = (int)context.ActionArguments["id"];
                    if (await _service.DoesRecipeExistAsync(recipeId))
                    {
                        await next();
                    }
                    else
                    {
                        context.Result = new NotFoundResult();
                    }
                }
                else
                {
                    context.Result = new NotFoundResult();
                }
            }
        }
    }
}
