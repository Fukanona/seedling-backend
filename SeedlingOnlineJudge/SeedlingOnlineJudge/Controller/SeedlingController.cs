using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeedlingOnlineJudge.Database;

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
        public IActionResult GetProblemById(string problemId)
        {
            var problem = _problemsDatabase.GetProblemById(problemId);
            if (problem == null)
                return NotFound();
            return Ok(problem);
        }
    }
}