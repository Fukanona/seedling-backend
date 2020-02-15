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

            problem = _problemsManager.UpdateProblem(problem, updatedProblem);

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
        [Route("problem/io/{problemId}")]
        [ServiceFilter(typeof(UserFilter), IsReusable = true)]
        public async Task<IActionResult> AddProblemIOAsync(string problemId, IFormFile inFile, IFormFile outFile)
        {
            if (!_problemsManager.HasPermission(problemId, this.GetUserFromContext()))
                return StatusCode(StatusCodes.Status403Forbidden, "You cannot add IO for this problem");

            var inLocation = await _fileManager.SaveAsync(inFile, $"{FoldersPath.IOLocation}", $"{ problemId}.in").ConfigureAwait(false);
            if (inLocation == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error trying to save IN file");
            var outLocation = await _fileManager.SaveAsync(outFile, $"{FoldersPath.IOLocation}", $"{problemId}.out").ConfigureAwait(false);
            if (outLocation == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error trying to save OUT file");

            _problemsManager.SetProblemActive(problemId);

            return Ok();
        }

        [HttpPut]
        [Route("problem/io/{problemId}")]
        [ServiceFilter(typeof(UserFilter), IsReusable = true)]
        public async Task<IActionResult> UpdateProblemIOAsync(string problemId, IFormFile inFile, IFormFile outFile)
        {
            if (!_problemsManager.HasPermission(problemId, this.GetUserFromContext()))
                return StatusCode(StatusCodes.Status403Forbidden, "You cannot add IO for this problem");

            if (!_problemsManager.IsProblemActive(problemId))
                return StatusCode(StatusCodes.Status401Unauthorized, "O problema ainda nao tem IO");

            if (inFile != null)
            {
                var inLocation = await _fileManager.SaveAsync(inFile, $"{FoldersPath.IOLocation}", $"{problemId}.in").ConfigureAwait(false);
                if (inLocation == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error trying to save IN file");
            }
            
            if(outFile != null)
            {
                var outLocation = await _fileManager.SaveAsync(outFile, $"{FoldersPath.IOLocation}", $"{problemId}.out").ConfigureAwait(false);
                if (outLocation == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error trying to save OUT file");
            }
            
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