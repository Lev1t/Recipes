using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Recipes.Data;
using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Authorization
{
    public class IsRecipeOwnerHandler : AuthorizationHandler<IsRecipeOwnerRequirement, int>
    {
        private readonly AppDbContext _context;
        public IsRecipeOwnerHandler(AppDbContext context)
        {
            _context = context;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            IsRecipeOwnerRequirement requirement, 
            int recipeId)
        {
            var recipeOwner = from u in _context.Users
                              where u.UserName == context.User.Identity.Name // finds the corresponding user 
                              from r in _context.Recipes
                              where r.RecipeId == recipeId                   // finds the corresponding recipe
                              where u.Id == r.CreatedById                    // checks that the user has created this recipe
                              select new { };
            if (await recipeOwner.CountAsync() > 0)
            {
                context.Succeed(requirement);
            }
        }
    }
}
