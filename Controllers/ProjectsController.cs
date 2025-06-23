using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolium_Back.Extensions.Helpers;
using Portfolium_Back.Models.ViewModels;
using Portfolium_Back.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Portfolium_Back.Controllers
{
    /// <summary>
    /// Controller para gerenciar projetos do portfólio
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Lista todos os projetos com paginação e filtros
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<RequestViewModel<ProjectViewModel>>> GetAll(
            [FromQuery] Int32 Pagina = 1,
            [FromQuery] Int32 RegistrosPorPagina = 10,
            [FromQuery] String? CamposQuery = "",
            [FromQuery] String? ValoresQuery = "",
            [FromQuery] String? Ordenacao = "",
            [FromQuery] Boolean Ordem = false)
        {
            return Ok(await _projectService.GetAllAsync(Pagina, RegistrosPorPagina, CamposQuery, ValoresQuery, Ordenacao, Ordem));
        }

        /// <summary>
        /// Busca um projeto por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestViewModel<ProjectViewModel>>> GetById(String id)
        {
            return Ok(await _projectService.GetByIdAsync(id));
        }

        /// <summary>
        /// Obter projetos em destaque
        /// </summary>
        [HttpGet("featured")]
        public async Task<ActionResult<RequestViewModel<ProjectViewModel>>> GetFeaturedProjects([FromQuery] int? limit = null)
        {
            return Ok(await _projectService.GetFeaturedAsync(limit));
        }

        /// <summary>
        /// Obter projetos por categoria
        /// </summary>
        [HttpGet("category/{category}")]
        public async Task<ActionResult<RequestViewModel<ProjectViewModel>>> GetProjectsByCategory(string category)
        {
            return Ok(await _projectService.GetByCategoryAsync(category));
        }

        /// <summary>
        /// Obter categorias disponíveis
        /// </summary>
        [HttpGet("categories")]
        public async Task<ActionResult<RequestViewModel<string>>> GetCategories()
        {
            return Ok(await _projectService.GetCategoriesAsync());
        }

        /// <summary>
        /// Obter tecnologias disponíveis
        /// </summary>
        [HttpGet("technologies")]
        public async Task<ActionResult<RequestViewModel<string>>> GetTechnologies()
        {
            return Ok(await _projectService.GetTechnologiesAsync());
        }

        /// <summary>
        /// Obter estatísticas dos projetos
        /// </summary>
        [HttpGet("stats")]
        public async Task<ActionResult<RequestViewModel<ProjectStatsViewModel>>> GetProjectStats()
        {
            return Ok(await _projectService.GetStatsAsync());
        }

        /// <summary>
        /// Cria um novo projeto
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RequestViewModel<ProjectViewModel>>> Create([FromBody] ProjectViewModel projectViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = TokenHelper.GetSingleUserId();

            return Ok(await _projectService.CreateAsync(projectViewModel, userId));
        }

        /// <summary>
        /// Atualiza um projeto existente
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<RequestViewModel<ProjectViewModel>>> Update(String id, [FromBody] ProjectViewModel projectViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Garantir que o ID está correto
            if (Guid.TryParse(id, out Guid projectId))
            {
                projectViewModel.GuidID = projectId;
            }

            return Ok(await _projectService.UpdateAsync(projectViewModel));
        }

        /// <summary>
        /// Exclui um projeto (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<RequestViewModel<ProjectViewModel>>> Delete(String id)
        {
            return Ok(await _projectService.DeleteAsync(id));
        }
    }
}