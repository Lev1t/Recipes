using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Recipes.Services;
using Recipes.Filters;

namespace Recipes.Controllers
{
    [Route("api/recipes")]
    public class RecipeApiController : Controller
    {
        RecipeService _service;
        public RecipeApiController(RecipeService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult GetList()
        {
            int x = 0;
            int y = 9 / x;
            return Ok(_service.GetRecipesAsync());
        }

        [HttpGet("{id:int}"), EnsureRecipeExists]
        public IActionResult GetDetails(int id)
        {
            return Ok(_service.GetRecipeDetailsAsync(id));
        }
    }
}
