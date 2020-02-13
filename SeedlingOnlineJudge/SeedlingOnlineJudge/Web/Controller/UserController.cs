using Microsoft.AspNetCore.Mvc;
using SeedlingOnlineJudge.Database;
using SeedlingOnlineJudge.Model;
using SeedlingOnlineJudge.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Web.Controller
{
    [ApiController]
    [Route("api/seedling")]
    public class UserController : ControllerBase
    {
        private readonly UserManager _userManager;

        public UserController(UserManager userManager)
        {
            _userManager = userManager;
        }
        [HttpPost]
        [Route("user")]
        public IActionResult RegisterNewUser(User newUser)
        {
            newUser.RegisterTime = Helper.GetDateTimeNowBrazil();

            _userManager.SaveUser(newUser);

            return Ok(newUser);
        }
    }
}
