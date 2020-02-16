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
using SeedlingOnlineJudge.BO;
using System.Collections.Generic;

namespace SeedlingOnlineJudge.Web.Controller
{
    [ApiController]
    [Route("api/seedling")]
    public class ProblemController : ControllerBase
    {
        private readonly ProblemsManager _problemsManager;
        private readonly FileManager _fileManager;

        private readonly ContractBO _contractBo;

        public ProblemController(ProblemsManager problemsManager, FileManager fileManager,
                                 ContractBO contractBo)
        {
            _problemsManager = problemsManager;
            _fileManager = fileManager;

            _contractBo = contractBo;
        }

        [HttpGet]
        [Route("problem/{problemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProblemById(string problemId)
        {
            var problem = _problemsManager.GetProblemById(problemId);
            if (problem == null)
                return NotFound(_contractBo.BuildBackendResponse<string>("Este problema não existe no sistema", SOJStatusCode.SOJStatusCodes.NotFound));
            return Ok(_contractBo.BuildBackendResponse<ProblemDescription>(null, SOJStatusCode.SOJStatusCodes.OK, problem));
        }

        [HttpPut]
        [Route("problem/{problemId}")]
        [ServiceFilter(typeof(UserFilter), IsReusable = true)]
        public IActionResult UpdateProblemById(string problemId, ProblemDescription updatedProblem)
        {
            var user = this.GetUserFromContext();

            var problem = _problemsManager.GetProblemById(problemId);
            if (problem == null) NotFound(_contractBo.BuildBackendResponse<string>("Este problema não existe no sistema.", SOJStatusCode.SOJStatusCodes.NotFound));

            if (!problem.Author.Username.Equals(user.Username))
                return BadRequest(_contractBo.BuildBackendResponse<string>("Você não pode modificar este problema.", SOJStatusCode.SOJStatusCodes.BadRequest));

            problem = _problemsManager.UpdateProblem(problem, updatedProblem);

            return Ok(_contractBo.BuildBackendResponse<ProblemDescription>(null, SOJStatusCode.SOJStatusCodes.OK, problem));
        }

        [HttpPost]
        [Route("problem")]
        [ServiceFilter(typeof(AuthorFilter), IsReusable = true)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AddProblem(ProblemDescription newProblem)
        {
            if (newProblem == null)
                return StatusCode(StatusCodes.Status400BadRequest,
                                    _contractBo.BuildBackendResponse<string>("Você precisar enviar um arquivo!",
                                                                            SOJStatusCode.SOJStatusCodes.BadRequest,
                                                                            null));

            var author = this.GetAuthorFromContext();

            newProblem.Author = author;
            newProblem.LastUpdate = Helper.GetDateTimeNowBrazil();

            var response = _problemsManager.AddProblem(newProblem);

            return StatusCode(StatusCodes.Status201Created,
                            _contractBo.BuildBackendResponse<ProblemDescription>("Problema adicionado com sucesso",
                                                                                SOJStatusCode.SOJStatusCodes.Created,
                                                                                response));
        }

        [HttpPost]
        [Route("problem/io/{problemId}")]
        [ServiceFilter(typeof(UserFilter), IsReusable = true)]
        public async Task<IActionResult> AddProblemIOAsync(string problemId, IFormFile inFile, IFormFile outFile)
        {
            if (!_problemsManager.HasPermission(problemId, this.GetUserFromContext()))
                if (!_problemsManager.HasPermission(problemId, this.GetUserFromContext()))
                    return StatusCode(StatusCodes.Status403Forbidden,
                                            _contractBo.BuildBackendResponse<string>("Você não tem autorização para atualizar este problema",
                                                                                    SOJStatusCode.SOJStatusCodes.Forbidden,
                                                                                    null));

            var inLocation = await _fileManager.SaveAsync(inFile, $"{FoldersPath.IOLocation}", $"{ problemId}.in").ConfigureAwait(false);
            if (inLocation == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                                        _contractBo.BuildBackendResponse<string>("Erro tentando salvar o arquivo de entrada",
                                                                                SOJStatusCode.SOJStatusCodes.InternalServerError,
                                                                                null));
            var outLocation = await _fileManager.SaveAsync(outFile, $"{FoldersPath.IOLocation}", $"{problemId}.out").ConfigureAwait(false);
            if (outLocation == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                                        _contractBo.BuildBackendResponse<string>("Erro tentando salvar o arquivo de saída",
                                                                                SOJStatusCode.SOJStatusCodes.InternalServerError,
                                                                                null));

            _problemsManager.SetProblemActive(problemId);

            return Ok(_contractBo.BuildBackendResponse<string>(null, SOJStatusCode.SOJStatusCodes.OK, null));
        }

        [HttpPut]
        [Route("problem/io/{problemId}")]
        [ServiceFilter(typeof(UserFilter), IsReusable = true)]
        public async Task<IActionResult> UpdateProblemIOAsync(string problemId, IFormFile inFile, IFormFile outFile)
        {
            if (!_problemsManager.HasPermission(problemId, this.GetUserFromContext()))
                return StatusCode(StatusCodes.Status403Forbidden,
                                        _contractBo.BuildBackendResponse<string>("Você não tem autorização para atualizar este problema",
                                                                                SOJStatusCode.SOJStatusCodes.Forbidden,
                                                                                null));

            if (!_problemsManager.IsProblemActive(problemId))
                return StatusCode(StatusCodes.Status401Unauthorized,
                                        _contractBo.BuildBackendResponse<string>("Você não pode atualizar um problema que ainda não está ativo",
                                                                                SOJStatusCode.SOJStatusCodes.Unauthorized,
                                                                                null));


            if (inFile != null)
            {
                var inLocation = await _fileManager.SaveAsync(inFile, $"{FoldersPath.IOLocation}", $"{problemId}.in").ConfigureAwait(false);
                if (inLocation == null)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                                        _contractBo.BuildBackendResponse<string>("Erro tentando salvar o arquivo de entrada",
                                                                                SOJStatusCode.SOJStatusCodes.InternalServerError,
                                                                                null));
            }
            
            if(outFile != null)
            {
                var outLocation = await _fileManager.SaveAsync(outFile, $"{FoldersPath.IOLocation}", $"{problemId}.out").ConfigureAwait(false);
                if (outLocation == null)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                                        _contractBo.BuildBackendResponse<string>("Erro tentando salvar o arquivo de saída",
                                                                                SOJStatusCode.SOJStatusCodes.InternalServerError,
                                                                                null));
            }
            
            return Ok(_contractBo.BuildBackendResponse<string>("IO atualizado com sucesso", SOJStatusCode.SOJStatusCodes.OK, null));
        }

        [HttpGet]
        [Route("problem/ids")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProblemsIds()
        {
            var allProblemsId = _problemsManager.GetAllProblemsIds();
            if (allProblemsId == null) return NotFound(_contractBo.BuildBackendResponse<string>("Nenhum problem encontrado no sistema", SOJStatusCode.SOJStatusCodes.NotFound));

            allProblemsId.Sort(new Helper.StrToIntAscComparator());
            return Ok(_contractBo.BuildBackendResponse<List<string>>(null, SOJStatusCode.SOJStatusCodes.OK, allProblemsId));
        }

        [HttpGet]
        [Route("problem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProblems()
        {
            var allProblems = _problemsManager.GetAllProblems();
            if (allProblems == null) return NotFound(_contractBo.BuildBackendResponse<string>("Nenhum problem encontrado no sistema", SOJStatusCode.SOJStatusCodes.NotFound));

            return Ok(_contractBo.BuildBackendResponse<List<ProblemDescription>>(null, SOJStatusCode.SOJStatusCodes.OK, allProblems));
        }
    }
}