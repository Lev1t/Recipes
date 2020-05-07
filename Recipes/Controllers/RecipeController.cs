using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Recipes.Models;
using Recipes.Services;

namespace Recipes.Controllers
{
    public class RecipeController : Controller
    {
        RecipeService _recipeService;
        public RecipeController(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }
        public IActionResult Index()
        {
            return View(_recipeService.GetRecipes());
        }

        public IActionResult Details(int id)
        {
            return View(_recipeService.GetRecipeDetails(id));
        }
        
        public IActionResult Create()
        {
            return View(new CreateRecipeCommand());
        }

        [HttpPost]
        public IActionResult Create(CreateRecipeCommand cmd)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    int recipeId = _recipeService.CreateRecipe(cmd);
                    return RedirectToAction(nameof(Details), new { Id = recipeId});
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occured saving the recipe");
            }
            return View(cmd);
        }

        public IActionResult Edit(int id)
        {
            var model = _recipeService.GetRecipeForUpdate(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(UpdateRecipeCommand cmd)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _recipeService.UpdateRecipe(cmd);
                    return RedirectToAction(nameof(Details), new { Id = cmd.Id });
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred saving the recipe");
            }
            return View(cmd);
        }

        public IActionResult Delete(int id)
        {
            _recipeService.DeleteRecipe(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
