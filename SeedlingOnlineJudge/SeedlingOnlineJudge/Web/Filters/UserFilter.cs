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
    [AttributeUsage(AttributeTargets.Method)]
    public class UserFilter : Attribute, IAsyncResourceFilter
    {
        private readonly UserManager _userManager;
        public UserFilter(UserManager userManager)
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
            } else
            {
                httpContext.Items.Add(typeof(User), user);
                await next();
            }
        }
    }
}