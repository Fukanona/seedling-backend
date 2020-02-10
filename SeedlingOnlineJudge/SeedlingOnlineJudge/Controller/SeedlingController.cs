using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeedlingOnlineJudge.Database;
using SeedlingOnlineJudge.Model;
using SeedlingOnlineJudge.Util;

namespace SeedlingOnlineJudge.Controller
{
    [ApiController]
    [Route("api/seedling")]
    public class SeedlingController : ControllerBase
    {
        private readonly ProblemsManager _problemsManager;
        private readonly IDatabase _database;

        public SeedlingController(ProblemsManager problemsManager, IDatabase database)
        {
            _problemsManager = problemsManager;
            _database = database;
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AddProblem([FromBody] ProblemDto newProblem)
        {
            if (newProblem == null)
                return StatusCode(StatusCodes.Status400BadRequest, "You need to upload a problem!");

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

            var problemLocation = new IOLocationDto
            {
                Id = lastProblem.Id,
                In = inLocation,
                Out = outLocation
            };

            _database.Save<IOLocationDto>(problemLocation);

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
    }
}