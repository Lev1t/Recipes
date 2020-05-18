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

namespace Recipes.Controllers
{
    [Authorize]
    public class RecipeController : Controller
    {
        private readonly RecipeService _recipeService;
        private readonly UserManager<User> _userService;
        private readonly IAuthorizationService _authService;

        public RecipeController(RecipeService recipeService, UserManager<User> userManager, IAuthorizationService authorizationService)
        {
            _recipeService = recipeService;
            _userService = userManager;
            _authService = authorizationService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var model = await _recipeService.GetRecipesAsync();
            return View(model);
        }

        [AllowAnonymous]
        [EnsureRecipeExists]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _recipeService.GetRecipeDetailsAsync(id);

            var authResult = await AuthorizeToManageRecipeAsync(id);
            if(authResult.Succeeded)
            {
                model.CanEdit = true;
            }
            return View(model);
        }

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
                    return RedirectToAction(nameof(Details), new { Id = recipeId });
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occured saving the recipe");
            }
            return View(cmd);
        }

        [EnsureRecipeExists]
        public async Task<IActionResult> Edit(int id)
        {
            var authResult = await AuthorizeToManageRecipeAsync(id);
            if (!authResult.Succeeded)
            {
                return new ForbidResult();
            }

            var model = await _recipeService.GetRecipeForUpdateAsync(id);
            if (model == null)
            {
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
                        return new ForbidResult();
                    }
                    await _recipeService.UpdateRecipeAsync(cmd);
                    return RedirectToAction(nameof(Details), new { Id = cmd.Id });
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred saving the recipe");
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
                return RedirectToAction(nameof(Index));
            }
            return new ForbidResult();
        }

        private async Task<AuthorizationResult> AuthorizeToManageRecipeAsync(int recipeId)
        {
            return await _authService.AuthorizeAsync(User, recipeId, "CanManageRecipe");
        }
    }
}
