using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeedlingOnlineJudge.Database;
using SeedlingOnlineJudge.Model;

namespace SeedlingOnlineJudge.Controller
{
    [ApiController]
    [Route("api/seedling")]
    public class SeedlingController : ControllerBase
    {
        private readonly ProblemsDatabase _problemsDatabase;
        public SeedlingController(ProblemsDatabase problemsDatabase)
        {
            _problemsDatabase = problemsDatabase;
        }

        [HttpGet]
        [Route("problem/{problemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProblemById(string problemId)
        {
            var problem = _problemsDatabase.GetProblemById(problemId);
            if (problem == null)
                return NotFound();
            return Ok(problem);
        }

        [HttpPost]
        [Route("problem")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AddProblem(ProblemDto newProblem)
        {
            var response = _problemsDatabase.AddProblem(newProblem);

            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpGet]
        [Route("problem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProblemsIds()
        {
            return Ok(_problemsDatabase.GetAllProblemsIds());
        }

        [HttpGet]
        [Route("simula")]
        public IActionResult Simula()
        {
            var problem1 = _problemsDatabase.GetProblemById("1");
            var problem2 = _problemsDatabase.GetProblemById("2");

            _problemsDatabase.Save<ProblemDto>(problem1);

            return Ok(problem1);
        }
    }
}