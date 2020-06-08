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
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Recipes.Services
{
    public class RecipeService
    {
        readonly AppDbContext _context;
        readonly UserManager<User> _userService;
        readonly ILogger _logger;
        readonly IMapper _mapper;

        public RecipeService(AppDbContext context, UserManager<User> userService, ILoggerFactory factory, IMapper mapper)
        {
            _context = context;
            _userService = userService;
            _logger = factory.CreateLogger<RecipeService>();
            _mapper = mapper;
        }

        public async Task<ICollection<RecipeSummaryViewModel>> GetRecipesAsync()
        {
            return _mapper.Map<IList<RecipeSummaryViewModel>>(await _context.Recipes
                .Where(r => !r.IsDeleted).Include(r => r.Ingridients).ToListAsync());
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
            return _mapper.Map<RecipeDetailViewModel>(recipe);
        }

        public async Task<int> CreateRecipeAsync(CreateRecipeCommand cmd, User user)
        {
            var recipe = _mapper.Map<Recipe>(cmd);
            recipe.CreatedById = user.Id;
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
            return recipe.RecipeId;
        }

        public async Task<UpdateRecipeCommand> GetRecipeForUpdateAsync(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Ingridients)
                .FirstOrDefaultAsync(r => r.RecipeId == id);

            return _mapper.Map<UpdateRecipeCommand>(recipe);
        }

        public async Task UpdateRecipeAsync(UpdateRecipeCommand cmd)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Ingridients)
                .FirstOrDefaultAsync(r => r.RecipeId == cmd.Id);

            if (recipe == null)
                throw new Exception($"Unable to find recipe with ID {cmd.Id}");

            cmd.UpdateRecipe(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRecipeAsync(int id)
        {
            var recipe = await _context.FindAsync<Recipe>(id);
            recipe.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}
