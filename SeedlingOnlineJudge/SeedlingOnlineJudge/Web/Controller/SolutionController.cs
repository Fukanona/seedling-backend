using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeedlingOnlineJudge.Web.Filters;
using SeedlingOnlineJudge.Infrastructure.File;
using SeedlingOnlineJudge.Model;
using SeedlingOnlineJudge.Util;
using Vtex.Commerce.Centauro.Web;
using SeedlingOnlineJudge.BO;

namespace SeedlingOnlineJudge.Web.Controller
{
    [ApiController]
    [Route("api/seedling")]
    public class SolutionController : ControllerBase
    {
        private readonly FileManager _fileManager;
        private readonly SolutionBO _solutionBo;
        public SolutionController(FileManager fileManager, SolutionBO solutionBo)
        {
            _fileManager = fileManager;
            _solutionBo = solutionBo;
        }

        [HttpPost]
        [Route("solution")]
        [ServiceFilter(typeof(UserFilter), IsReusable = true),]
        public async Task<IActionResult> UploadSolutionAsync(IFormFile solution, string problemId)
        {
            User user = this.GetUserFromContext();
            await _fileManager.SaveAsync(solution, @$"{FoldersPath.SolutionLocation}\{user.Username}", $"{problemId}.cpp").ConfigureAwait(false);

            return Ok(_solutionBo.RunUserSolution(user, problemId));
        }
    }
}