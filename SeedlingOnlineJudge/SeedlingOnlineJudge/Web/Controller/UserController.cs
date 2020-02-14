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

        [HttpGet]
        [Route("user/{username}")]
        public IActionResult GetUser(string username)
        {
            var user = _userManager.GetUser(username);
            if (user == null) return NotFound();
            
            return Ok(user);
        }

        [HttpPut]
        [Route("user/{username}")]
        public IActionResult UpdateUser(string username, User updatedUser)
        {
            var res = _userManager.UpdateUser(username, updatedUser);

            return Ok(res);
        }
    }
}
