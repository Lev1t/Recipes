using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recipes.Data;
using Recipes.Models;
using Recipes.Services;
using Recipes.Filters;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Extensions.Logging;

namespace Recipes.Controllers
{
    [Authorize]
    public class RecipeController : Controller
    {
        private readonly RecipeService _recipeService;
        private readonly UserManager<User> _userService;
        private readonly IAuthorizationService _authService;
        private readonly ILogger<RecipeController> _logger;

        public RecipeController(RecipeService recipeService, 
            UserManager<User> userManager, 
            IAuthorizationService authorizationService,
            ILogger<RecipeController> logger)
        {
            _recipeService = recipeService;
            _userService = userManager;
            _authService = authorizationService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var model = await _recipeService.GetRecipesAsync();
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [EnsureRecipeExists]
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Loading recipe with ID {Id}", id);
            var model = await _recipeService.GetRecipeDetailsAsync(id);

            if(model == null)
            {
                _logger.LogWarning("Couldn't find recipe with ID {Id}", id);
                return NotFound();
            }

            var authResult = await AuthorizeToManageRecipeAsync(id);
            if(authResult.Succeeded)
            {
                model.CanEdit = true;
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateRecipeCommand());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRecipeCommand cmd)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = await _userService.GetUserAsync(User);
                    int recipeId = await _recipeService.CreateRecipeAsync(cmd, user);

                    _logger.LogInformation("Recipe with ID {Id} has been created", recipeId);

                    return RedirectToAction(nameof(Details), new { Id = recipeId });
                }
                else
                {
                    _logger.LogWarning("Creating recipe is failed due to invalid ModelState", cmd.Name);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occured saving the recipe");
                _logger.LogError("Failed to save the recipe. Recipe name = {RecipeName}\nException: {Exception}", cmd.Name, ex);
            }
            return View(cmd);
        }

        [HttpGet]
        [EnsureRecipeExists]
        public async Task<IActionResult> Edit(int id)
        {
            var authResult = await AuthorizeToManageRecipeAsync(id);
            if (!authResult.Succeeded)
            {
                _logger.LogWarning("User {UserName} attempted to edit the recipe ID {Id} without permission", User.Identity.Name, id);
                return new ForbidResult();
            }

            var model = await _recipeService.GetRecipeForUpdateAsync(id);
            if (model == null)
            {
                _logger.LogError("Failed to create UpdateRecipeCommand for ID {Id}", id);
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateRecipeCommand cmd)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var authResult = await AuthorizeToManageRecipeAsync(cmd.Id);
                    if (!authResult.Succeeded)
                    {
                        _logger.LogWarning("User {UserName} attempted to edit the recipe ID {Id} without permission", 
                            User.Identity.Name, cmd.Id);

                        return new ForbidResult();
                    }
                    await _recipeService.UpdateRecipeAsync(cmd);
                    _logger.LogInformation("User {UserName} has updated the recipe ID {Id}", User.Identity.Name, cmd.Id);
                    return RedirectToAction(nameof(Details), new { Id = cmd.Id });
                }
                else
                {
                    _logger.LogWarning("Updating the recipe ID {Id}  by User {UserName} was fail due to invalid ModelState", 
                        cmd.Id, User.Identity.Name);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred saving the recipe");
                _logger.LogError("Failed to update the recipe ID {Id} by User {UserName}\nException: {Exception}",
                    cmd.Id, User.Identity.Name, ex);
            }
            return View(cmd);
        }


        [EnsureRecipeExists]
        public async Task<IActionResult> Delete(int id)
        {
            var authResult = await AuthorizeToManageRecipeAsync(id);
            if (authResult.Succeeded)
            {
                await _recipeService.DeleteRecipeAsync(id);
                _logger.LogInformation("User {UserName} deleted the recipe ID {Id}", User.Identity.Name, id);
                return RedirectToAction(nameof(Index));
            }
            _logger.LogWarning("User {UserName} attempted to delete the recipe ID {Id} without permission",
                User.Identity.Name, id);
            return new ForbidResult();
        }

        private async Task<AuthorizationResult> AuthorizeToManageRecipeAsync(int recipeId)
        {
            return await _authService.AuthorizeAsync(User, recipeId, "CanManageRecipe");
        }
    }
}
