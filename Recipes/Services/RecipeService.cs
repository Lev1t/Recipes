using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Recipes.Data;
using Recipes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Net.Cache;

namespace Recipes.Services
{
    public class RecipeService
    {
        readonly AppDbContext _context;
        readonly ILogger _logger;
        readonly UserManager<User> _userService;

        public RecipeService(AppDbContext context, UserManager<User> userService, ILoggerFactory factory)
        {
            _context = context;
            _userService = userService;
            _logger = factory.CreateLogger<RecipeService>();
        }
        
        public async Task<ICollection<RecipeSummaryViewModel>> GetRecipesAsync()
        {
            return await _context.Recipes
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
                .ToListAsync();
        }

        public async Task<Recipe> GetRecipeAsync(int id)
        {
            return await _context.Recipes.FindAsync(id);
        }

        public async Task<bool> DoesRecipeExistAsync(int id)
        {
            return await _context.Recipes
                .Where(r => r.RecipeId == id)
                .Where(r => r.IsDeleted == false)
                .AnyAsync();
        }

        public async Task<RecipeDetailViewModel> GetRecipeDetailsAsync(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Ingridients)
                .FirstOrDefaultAsync(r => r.RecipeId == id);
            return RecipeDetailViewModel.FromRecipe(recipe);
        }

        public async Task<int> CreateRecipeAsync(CreateRecipeCommand cmd, User user)
        {
            var recipe = cmd.ToRecipe(user);
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
            return recipe.RecipeId;
        }

        public async Task<UpdateRecipeCommand> GetRecipeForUpdateAsync(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Ingridients)
                .FirstOrDefaultAsync(r => r.RecipeId == id);

            if (recipe == null) 
                throw new Exception($"Unable to find recipe with ID {id}");

            return UpdateRecipeCommand.FromRecipe(recipe);
        }

        public async Task UpdateRecipeAsync(UpdateRecipeCommand cmd)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Ingridients)
                .FirstOrDefaultAsync(r => r.RecipeId == cmd.Id);

            if (recipe == null) 
                throw new Exception($"Unable to find recipe with ID {cmd.Id}");

            if (recipe.IsDeleted) 
                throw new Exception("Unable to update deleted recipe");

            cmd.UpdateRecipe(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRecipeAsync(int id)
        {
            var recipe = await _context.FindAsync<Recipe>(id);

            if (recipe.IsDeleted) 
                throw new Exception("Unable to delete a deleted recipe");

            recipe.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}
