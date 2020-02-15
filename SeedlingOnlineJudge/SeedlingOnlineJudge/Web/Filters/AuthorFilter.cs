using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SeedlingOnlineJudge.Database;
using SeedlingOnlineJudge.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Web.Filters
{
    public class AuthorFilter : Attribute, IAsyncResourceFilter
    {
        private readonly UserManager _userManager;
        
        public AuthorFilter(UserManager userManager)
        {
            _userManager = userManager;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var request = httpContext.Request;
            string username = null;
            if (request.Query.TryGetValue("username", out var stringValues) && stringValues.Count > default(int))
                username = stringValues[default];
            var user = _userManager.GetUserByUsername(username);

            if (user == null)
            {
                context.Result = new BadRequestObjectResult($"invalid username");
            }
            else
            {
                Author author = new Author();
                author.Copy(user);

                httpContext.Items.Add(typeof(Author), author);
                await next();
            }
        }
    }
}
