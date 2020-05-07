using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Recipes.Data;
using Recipes.Models;
using Microsoft.EntityFrameworkCore;

namespace Recipes.Services
{
    public class RecipeService
    {
        readonly AppDbContext _context;
        readonly ILogger _logger;

        public RecipeService(AppDbContext context, ILoggerFactory factory)
        {
            _context = context;
            _logger = factory.CreateLogger<RecipeService>();
        }
        
        public ICollection<RecipeSummaryViewModel> GetRecipes()
        {
            return _context.Recipes
                .Where(r => !r.IsDeleted)
                .Select(x => new RecipeSummaryViewModel
                {
                    Id = x.RecipeId,
                    Name = x.Name,
                    TimeToCook = x.TimeToCook.Hours > 0
                                                ? $"{x.TimeToCook.Hours} hrs {x.TimeToCook.Minutes} mins"
                                                : $"{x.TimeToCook.Minutes} mins",
                    NumberOfIngridients = x.Ingridients.Count
                })
                .ToList();
        }

        public bool DoesRecipeExist(int id)
        {
            return _context.Recipes
                .Where(r => r.RecipeId == id)
                .Where(r => r.IsDeleted == false)
                .Any();
        }

        public RecipeDetailViewModel GetRecipeDetails(int id)
        {
            var recipe = _context.Recipes.Include(r => r.Ingridients).FirstOrDefault(r => r.RecipeId == id);
            return RecipeDetailViewModel.FromRecipe(recipe);
        }

        public int CreateRecipe(CreateRecipeCommand cmd)
        {
            var recipe = cmd.ToRecipe();
            _context.Recipes.Add(recipe);
            _context.SaveChanges();
            return recipe.RecipeId;
        }

        public UpdateRecipeCommand GetRecipeForUpdate(int id)
        {
            var recipe = _context.Recipes.Include(r => r.Ingridients).FirstOrDefault(r => r.RecipeId == id);
            return UpdateRecipeCommand.FromRecipe(recipe);
        }

        public void UpdateRecipe(UpdateRecipeCommand cmd)
        {
            var recipe = _context.Recipes.Include(r => r.Ingridients).FirstOrDefault(r => r.RecipeId == cmd.Id);
            if (recipe == null) throw new Exception($"Unable to find recipe with ID {cmd.Id}");
            if (recipe.IsDeleted) throw new Exception("Unable to update deleted recipe");
            cmd.UpdateRecipe(recipe);
            _context.SaveChanges();
        }

        public void DeleteRecipe(int id)
        {
            var recipe = _context.Find<Recipe>(id); //EDIT
            if (recipe.IsDeleted) throw new Exception("Unable to delete a deleted recipe");
            recipe.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
