using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeedlingOnlineJudge.Database;
using SeedlingOnlineJudge.Filters;
using SeedlingOnlineJudge.Model;
using SeedlingOnlineJudge.Util;
using Vtex.Commerce.Centauro.Web;

namespace SeedlingOnlineJudge.Controller
{
    [ApiController]
    [Route("api/seedling")]
    public class SeedlingController : ControllerBase
    {
        private readonly ProblemsManager _problemsManager;
        private readonly UserManager _userManager;

        public SeedlingController(ProblemsManager problemsManager, UserManager userManager)
        {
            _problemsManager = problemsManager;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("problem/{problemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProblemById(string problemId)
        {
            var problem = _problemsManager.GetProblemById(problemId);
            if (problem == null)
                return NotFound();
            return Ok(problem);
        }

        [HttpPost]
        [Route("problem")]
        [ServiceFilter(typeof(UserFilter), IsReusable = true)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AddProblem(ProblemDescription newProblem)
        {
            if (newProblem == null)
                return StatusCode(StatusCodes.Status400BadRequest, "You need to upload a problem!");

            var user = this.GetUserFromContext();

            newProblem.Author = user;

            var response = _problemsManager.AddProblem(newProblem);

            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost]
        [Route("problemio")]
        public async Task<IActionResult> AddProblemInAndOutAsync(IFormFile inFile, IFormFile outFile)
        {
            var lastProblem = _problemsManager.GetAllProblems().Last();

            var inLocation = await Helper.SaveAFileToDiskAsync(inFile, $"{lastProblem.Id}.in", Folders.InFileLocation).ConfigureAwait(false);
            if (inLocation == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error trying to save IN file");
            var outLocation = await Helper.SaveAFileToDiskAsync(outFile, $"{lastProblem.Id}.out", Folders.OutFileLocation).ConfigureAwait(false);
            if (outLocation == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error trying to save OUT file");

            return Ok();
        }

        [HttpPost]
        [Route("solution")]
        [ServiceFilter(typeof(UserFilter), IsReusable = true)]
        public async Task<IActionResult> UploadSolutionAsync(IFormFile solution, string problemId)
        {
            User user = this.GetUserFromContext();
            await Helper.SaveAFileToDiskAsync(solution, $"{problemId}.cpp", $"{Folders.SolutionFileLocation}/{user.Username}").ConfigureAwait(false);
            return Ok();
        }

        [HttpGet]
        [Route("problem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProblemsIds()
        {
            var allProblemsId = _problemsManager.GetAllProblemsIds();
            allProblemsId.Sort(new Helper.StrToIntAscComparator());
            return StatusCode(StatusCodes.Status200OK, allProblemsId);
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