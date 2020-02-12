
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

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static StoreConfiguration GetStoreConfigurationFromContext(this ControllerBase controller)
        //    => (StoreConfiguration)controller.HttpContext.Items[nameof(StoreConfiguration)];
    }
}