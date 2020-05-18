using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Authorization
{
    public class IsRecipeOwnerRequirement : IAuthorizationRequirement { }
}
