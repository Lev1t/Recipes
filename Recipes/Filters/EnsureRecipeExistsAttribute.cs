using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Services;
using Microsoft.AspNetCore.Mvc;
using Recipes.Controllers;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Recipes.Filters
{
    public class EnsureRecipeExistsAttribute : TypeFilterAttribute
    {
        public EnsureRecipeExistsAttribute() : base(typeof(EnsureRecipeFilter)) { }

        public class EnsureRecipeFilter : IAsyncActionFilter
        {
            RecipeService _service;
            ILogger<EnsureRecipeFilter> _logger;
            public EnsureRecipeFilter(RecipeService service, ILogger<EnsureRecipeFilter> logger)
            {
                _service = service;
                _logger = logger;
            }

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
                        _logger.LogWarning("Recipe with id {RecipeId} doesn't exist", recipeId);
                        context.Result = new NotFoundResult();
                    }
                }
                else
                {
                    _logger.LogWarning("ModelState invalid");
                    context.Result = new NotFoundResult();
                }
            }
        }
    }
}
