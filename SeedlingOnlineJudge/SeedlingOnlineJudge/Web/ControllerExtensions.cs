
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using SeedlingOnlineJudge.Model;

namespace Vtex.Commerce.Centauro.Web
{
    public static class ControllerExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static User GetUserFromContext(this ControllerBase controller)
            => (User)controller.HttpContext.Items[typeof(User)];

        public static Author GetAuthorFromContext(this ControllerBase controller)
            => (Author)controller.HttpContext.Items[typeof(Author)];

    }
}