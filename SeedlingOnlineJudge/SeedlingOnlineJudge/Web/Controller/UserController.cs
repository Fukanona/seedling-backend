using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeedlingOnlineJudge.BO;
using SeedlingOnlineJudge.Database;
using SeedlingOnlineJudge.Model;
using SeedlingOnlineJudge.Util;
using SeedlingOnlineJudge.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vtex.Commerce.Centauro.Web;

namespace SeedlingOnlineJudge.Web.Controller
{
    [ApiController]
    [Route("api/seedling")]
    public class UserController : ControllerBase
    {
        private readonly UserManager _userManager;

        private readonly ContractBO _contractBo;

        public UserController(UserManager userManager, ContractBO contractBo)
        {
            _userManager = userManager;

            _contractBo = contractBo;
        }

        [HttpPost]
        [Route("user")]
        public IActionResult RegisterNewUser(User newUser)
        {
            newUser.RegisterTime = Helper.GetDateTimeNowBrazil();

            _userManager.SaveUser(newUser);

            return StatusCode(StatusCodes.Status201Created, _contractBo.BuildBackendResponse<User>("Usuário cadastrado com sucesso", SOJStatusCode.SOJStatusCodes.Created, null));
        }

        [HttpGet]
        [Route("user/{username}")]
        public IActionResult GetUser(string username)
        {
            var user = _userManager.GetUser(username);
            if (user == null) return NotFound(_contractBo.BuildBackendResponse<string>("Usuário não cadastrado no sistema", SOJStatusCode.SOJStatusCodes.NotFound, null));
            
            return Ok(_contractBo.BuildBackendResponse<User>(null, SOJStatusCode.SOJStatusCodes.OK, user));
        }

        [HttpGet]
        [Route("user/login")]
        public IActionResult ValidateUserLogin()
        {
            var request = this.HttpContext.Request;
            request.Query.TryGetValue("username", out var username);
            request.Query.TryGetValue("password", out var password);

            var res = _userManager.ValidateUser(username.ToString(), password.ToString());
            if (res == false) return StatusCode(StatusCodes.Status401Unauthorized, _contractBo.BuildBackendResponse<string>("Username e/ou password incorreto(s)", SOJStatusCode.SOJStatusCodes.Unauthorized));
            return Ok(_contractBo.BuildBackendResponse<string>("Login efetuado com sucesso!", SOJStatusCode.SOJStatusCodes.OK, null));
        }

        [HttpGet]
        [Route("user/password/{username}")]
        public IActionResult RecoverPassword(string username)
        {
            var user = _userManager.GetUser(username);

            user.Password = _userManager.DecryptPassword(user);

            return Ok(_contractBo.BuildBackendResponse<User>(null, SOJStatusCode.SOJStatusCodes.OK, user));
        }

        [HttpPut]
        [Route("user/{username}")]
        [ServiceFilter(typeof(UserFilter), IsReusable = true)]
        public IActionResult UpdateUser(string username, User updatedUser)
        {
            var user = this.GetUserFromContext();
            if (!user.Username.Equals(username))
                return NotFound(_contractBo.BuildBackendResponse<string>("Você não tem permissão para atualizar este usuário", SOJStatusCode.SOJStatusCodes.Unauthorized, null));
            var res = _userManager.UpdateUser(username, updatedUser);

            return Ok(_contractBo.BuildBackendResponse<User>(null, SOJStatusCode.SOJStatusCodes.OK, res));
        }
    }
}
