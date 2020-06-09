using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Recipes.Services;
using Recipes.Filters;
using Recipes.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace Recipes.Controllers
{
    [Route("api/recipes")]
    public class RecipeApiController : Controller
    {
        readonly RecipeService _recipeService;
        readonly ILogger _logger;
        public RecipeApiController(RecipeService service, ILogger logger)
        {
            _recipeService = service;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            return Ok( await _recipeService.GetRecipesAsync());
        }

        [HttpGet("{id:int}"), EnsureRecipeExists]
        public async Task<IActionResult> GetDetails(int id)
        {
            return Ok(await _recipeService.GetRecipeDetailsAsync(id));
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(CreateRecipeCommand cmd)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            User user = await _userService.GetUserAsync(User);
        //            int recipeId = await _recipeService.CreateRecipeAsync(cmd, user);

        //            _logger.LogInformation("Recipe with ID {Id} has been created", recipeId);

        //            return RedirectToAction(nameof(Details), new { Id = recipeId });
        //        }
        //        else
        //        {
        //            _logger.LogWarning("Creating recipe is failed due to invalid ModelState", cmd.Name);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, "An error occured saving the recipe");
        //        _logger.LogError("Failed to save the recipe. Recipe name = {RecipeName}\nException: {Exception}", cmd.Name, ex);
        //    }
        //    return Ok();
        //}
    }
}
