using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeedlingOnlineJudge.Database;
using SeedlingOnlineJudge.Web.Filters;
using SeedlingOnlineJudge.Infrastructure.File;
using SeedlingOnlineJudge.Model;
using SeedlingOnlineJudge.Util;
using Vtex.Commerce.Centauro.Web;

namespace SeedlingOnlineJudge.Web.Controller
{
    [ApiController]
    [Route("api/seedling")]
    public class ProblemController : ControllerBase
    {
        private readonly ProblemsManager _problemsManager;
        private readonly FileManager _fileManager;

        public ProblemController(ProblemsManager problemsManager, FileManager fileManager)
        {
            _problemsManager = problemsManager;
            _fileManager = fileManager;
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
        [ServiceFilter(typeof(AuthorFilter), IsReusable = true)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AddProblem(ProblemDescription newProblem)
        {
            if (newProblem == null)
                return StatusCode(StatusCodes.Status400BadRequest, "You need to upload a problem!");

            var author = this.GetAuthorFromContext();

            newProblem.Author = author;
            newProblem.LastUpdate = Helper.GetDateTimeNowBrazil();

            var response = _problemsManager.AddProblem(newProblem);

            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost]
        [Route("problemio")]
        [ServiceFilter(typeof(UserFilter), IsReusable = true)]
        public async Task<IActionResult> AddProblemInAndOutAsync(IFormFile inFile, IFormFile outFile)
        {
            User user = this.GetUserFromContext();

            var lastProblem = _problemsManager.GetAllProblems().Last();

            var inLocation = await _fileManager.SaveAsync(inFile, $"{lastProblem.Id}.in", $"{FoldersPath.IOLocation}/{user.Username}").ConfigureAwait(false);
            if (inLocation == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error trying to save IN file");
            var outLocation = await _fileManager.SaveAsync(outFile, $"{lastProblem.Id}.out", $"{FoldersPath.IOLocation}/{user.Username}").ConfigureAwait(false);
            if (outLocation == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error trying to save OUT file");

            return Ok();
        }

        [HttpGet]
        [Route("problem/all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProblemsIds()
        {
            var allProblemsId = _problemsManager.GetAllProblemsIds();
            if (allProblemsId == null) return StatusCode(StatusCodes.Status404NotFound);

            allProblemsId.Sort(new Helper.StrToIntAscComparator());
            return StatusCode(StatusCodes.Status200OK, allProblemsId);
        }

        [HttpPut]
        [Route("problem/{problemId}")]
        [ServiceFilter(typeof(UserFilter), IsReusable = true)]
        public IActionResult UpdateProblemById(string problemId, ProblemDescription updatedProblem)
        {
            var user = this.GetUserFromContext();

            var problem = _problemsManager.GetProblemById(problemId);
            if (problem == null) return NotFound();

            if (!problem.Author.Username.Equals(user.Username))
                return BadRequest("You cannot modify this problem");

            problem.Copy(updatedProblem);
            problem.LastUpdate = Helper.GetDateTimeNowBrazil();

            return Ok(problem);
        }

        [HttpGet]
        [Route("problem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProblems()
        {
            var allProblems = _problemsManager.GetAllProblems();
            if (allProblems == null) return StatusCode(StatusCodes.Status404NotFound);

            return StatusCode(StatusCodes.Status200OK, allProblems);
        }
    }
}