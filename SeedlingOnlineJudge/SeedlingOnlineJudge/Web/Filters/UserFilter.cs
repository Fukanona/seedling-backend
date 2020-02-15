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

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System;
//using System.Threading.Tasks;
//using Vtex.Commerce.Centauro.Contracts.LicenseManager;
//using Vtex.Commerce.Centauro.Services;
//namespace Vtex.Commerce.Centauro.Web.Filters
//{
//    [AttributeUsage(AttributeTargets.Method)]
//    public sealed class AccountFilterAttribute : Attribute, IAsyncResourceFilter
//    {

//        private readonly LicenseManagerClient _licenseManagerClient;

//        public AccountFilterAttribute(LicenseManagerClient licenseManagerClient)
//        {
//            _licenseManagerClient = licenseManagerClient;
//        }

//        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
//        {
//            var httpContext = context.HttpContext;
//            var request = httpContext.Request;
//            string accountName = null;
//            if (request.Query.TryGetValue("an", out var stringValues) && stringValues.Count > default(int))
//                accountName = stringValues[default];
//            AccountDto account;
//            if (string.IsNullOrWhiteSpace(accountName))
//                account = await _licenseManagerClient.GetAccountByHostAsync(request.Host.Host);
//            else
//                account = await _licenseManagerClient.GetAccountAsync(accountName);
//            if (account == null)
//                context.Result = new NotFoundObjectResult("account not found");
//            else
//            {
//                if (!(account.IsActive ?? false))
//                    context.Result = new BadRequestObjectResult($"account {account.AccountName} is inactive");
//                else
//                {
//                    httpContext.Items.Add(nameof(AccountDto), account);
//                    await next();
//                }
//            }
//        }
//    }
//}