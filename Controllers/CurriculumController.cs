using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolium_Back.Extensions.Helpers;
using Portfolium_Back.Models.ViewModels;
using Portfolium_Back.Services.Interfaces;
using System.Security.Claims;

namespace Portfolium_Back.Controllers
{
    /// <summary>
    /// Controller responsável pela gestão de currículo
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculumController : ControllerBase
    {
        private readonly ICurriculumService _curriculumService;

        /// <summary>
        /// Construtor do CurriculumController
        /// </summary>
        /// <param name="curriculumService">Serviço de currículo injetado via DI</param>
        public CurriculumController(ICurriculumService curriculumService)
        {
            _curriculumService = curriculumService;
        }

        /// <summary>
        /// Obtém o currículo completo do usuário 
        /// </summary>
        /// <param name="userId">ID do usuário (opcional - se não informado, usa o usuário logado)</param>
        /// <returns>Dados completos do currículo</returns>
        /// <response code="200">Currículo retornado com sucesso</response>
        /// <response code="404">Currículo não encontrado</response>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCurriculum()
        {
            return Ok(await _curriculumService.GetCurriculumAsync());
        }

        /// <summary>
        /// Obtém as informações pessoais
        /// </summary>
        /// <returns>Informações pessoais</returns>
        [HttpGet("personal-info")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPersonalInfo()
        {
            return Ok(await _curriculumService.GetPersonalInfoAsync());
        }

        /// <summary>
        /// Cria ou atualiza as informações pessoais
        /// </summary>
        /// <param name="personalInfo">Dados das informações pessoais</param>
        /// <returns>Resultado da operação</returns>
        [HttpPost("personal-info")]
        [Authorize]
        public async Task<IActionResult> CreatePersonalInfo([FromBody] PersonalInfoViewModel personalInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _curriculumService.CreatePersonalInfoAsync(personalInfo));
        }

        /// <summary>
        /// Atualiza as informações pessoais
        /// </summary>
        /// <param name="personalInfo">Dados das informações pessoais</param>
        /// <returns>Resultado da operação</returns>
        [HttpPut("personal-info")]
        [Authorize]
        public async Task<IActionResult> UpdatePersonalInfo([FromBody] PersonalInfoViewModel personalInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _curriculumService.UpdatePersonalInfoAsync(personalInfo));
        }

        /// <summary>
        /// Lista as habilidades 
        /// </summary>
        /// <param name="category">Categoria específica (opcional)</param>
        /// <returns>Lista de habilidades</returns>
        [HttpGet("skills")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSkills(string? category = null)
        {
            var skills = await _curriculumService.GetSkillsAsync(category);
            return Ok(skills);
        }

        /// <summary>
        /// Cria uma nova habilidade
        /// </summary>
        /// <param name="skill">Dados da habilidade</param>
        /// <returns>Resultado da operação</returns>
        [HttpPost("skills")]
        [Authorize]
        public async Task<IActionResult> CreateSkill([FromBody] SkillViewModel skill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _curriculumService.CreateSkillAsync(skill));
        }

        /// <summary>
        /// Atualiza uma habilidade específica
        /// </summary>
        /// <param name="skill">Dados da habilidade</param>
        /// <returns>Resultado da operação</returns>
        [HttpPut("skills")]
        [Authorize]
        public async Task<IActionResult> UpdateSkill([FromBody] SkillViewModel skill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _curriculumService.UpdateSkillAsync(skill));
        }

        /// <summary>
        /// Remove uma habilidade
        /// </summary>
        /// <param name="id">ID da habilidade</param>
        /// <returns>Resultado da operação</returns>
        [HttpDelete("skills/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSkill(Guid id)
        {
            return Ok(await _curriculumService.DeleteSkillAsync(id));
        }

        /// <summary>
        /// Lista as experiências profissionais 
        /// </summary>
        /// <returns>Lista de experiências</returns>
        [HttpGet("experiences")]
        [AllowAnonymous]
        public async Task<IActionResult> GetExperiences()
        {
            return Ok(await _curriculumService.GetExperiencesAsync());
        }

        /// <summary>
        /// Cria uma nova experiência
        /// </summary>
        /// <param name="experience">Dados da experiência</param>
        /// <returns>Resultado da operação</returns>
        [HttpPost("experiences")]
        [Authorize]
        public async Task<IActionResult> CreateExperience([FromBody] ExperienceViewModel experience)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _curriculumService.CreateExperienceAsync(experience));
        }

        /// <summary>
        /// Atualiza uma experiência específica
        /// </summary>
        /// <param name="experience">Dados da experiência</param>
        /// <returns>Resultado da operação</returns>
        [HttpPut("experiences")]
        [Authorize]
        public async Task<IActionResult> UpdateExperience([FromBody] ExperienceViewModel experience)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _curriculumService.UpdateExperienceAsync(experience));
        }

        /// <summary>
        /// Remove uma experiência
        /// </summary>
        /// <param name="id">ID da experiência</param>
        /// <returns>Resultado da operação</returns>
        [HttpDelete("experiences/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteExperience(Guid id)
        {
            return Ok(await _curriculumService.DeleteExperienceAsync(id));
        }

        /// <summary>
        /// Lista a educação 
        /// </summary>
        /// <returns>Lista de educação</returns>
        [HttpGet("education")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEducation()
        {
            return Ok(await _curriculumService.GetEducationAsync());
        }

        /// <summary>
        /// Cria uma nova educação
        /// </summary>
        /// <param name="education">Dados da educação</param>
        /// <returns>Resultado da operação</returns>
        [HttpPost("education")]
        [Authorize]
        public async Task<IActionResult> CreateEducation([FromBody] EducationViewModel education)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _curriculumService.CreateEducationAsync(education));
        }

        /// <summary>
        /// Atualiza uma educação específica
        /// </summary>
        /// <param name="education">Dados da educação</param>
        /// <returns>Resultado da operação</returns>
        [HttpPut("education")]
        [Authorize]
        public async Task<IActionResult> UpdateEducation([FromBody] EducationViewModel education)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _curriculumService.UpdateEducationAsync(education));
        }

        /// <summary>
        /// Remove uma educação
        /// </summary>
        /// <param name="id">ID da educação</param>
        /// <returns>Resultado da operação</returns>
        [HttpDelete("education/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEducation(Guid id)
        {
            return Ok(await _curriculumService.DeleteEducationAsync(id));
        }

        /// <summary>
        /// Lista as certificações 
        /// </summary>
        /// <returns>Lista de certificações</returns>
        [HttpGet("certifications")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCertifications()
        {
            return Ok(await _curriculumService.GetCertificationsAsync());
        }

        /// <summary>
        /// Cria uma nova certificação
        /// </summary>
        /// <param name="certification">Dados da certificação</param>
        /// <returns>Resultado da operação</returns>
        [HttpPost("certifications")]
        [Authorize]
        public async Task<IActionResult> CreateCertification([FromBody] CertificationViewModel certification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _curriculumService.CreateCertificationAsync(certification));
        }

        /// <summary>
        /// Atualiza uma certificação específica
        /// </summary>
        /// <param name="certification">Dados da certificação</param>
        /// <returns>Resultado da operação</returns>
        [HttpPut("certifications")]
        [Authorize]
        public async Task<IActionResult> UpdateCertification([FromBody] CertificationViewModel certification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _curriculumService.UpdateCertificationAsync(certification));
        }

        /// <summary>
        /// Remove uma certificação
        /// </summary>
        /// <param name="id">ID da certificação</param>
        /// <returns>Resultado da operação</returns>
        [HttpDelete("certifications/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCertification(Guid id)
        {
            return Ok(await _curriculumService.DeleteCertificationAsync(id));
        }

        /// <summary>
        /// Lista os serviços 
        /// </summary>
        /// <returns>Lista de serviços</returns>
        [HttpGet("services")]
        [AllowAnonymous]
        public async Task<IActionResult> GetServices()
        {
            return Ok(await _curriculumService.GetServicesAsync());
        }

        /// <summary>
        /// Cria um novo serviço
        /// </summary>
        /// <param name="service">Dados do serviço</param>
        /// <returns>Resultado da operação</returns>
        [HttpPost("services")]
        [Authorize]
        public async Task<IActionResult> CreateService([FromBody] ServiceViewModel service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _curriculumService.CreateServiceAsync(service));
        }

        /// <summary>
        /// Atualiza um serviço específico
        /// </summary>
        /// <param name="service">Dados do serviço</param>
        /// <returns>Resultado da operação</returns>
        [HttpPut("services")]
        [Authorize]
        public async Task<IActionResult> UpdateService([FromBody] ServiceViewModel service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _curriculumService.UpdateServiceAsync(service));
        }

        /// <summary>
        /// Remove um serviço
        /// </summary>
        /// <param name="id">ID do serviço</param>
        /// <returns>Resultado da operação</returns>
        [HttpDelete("services/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            return Ok(await _curriculumService.DeleteServiceAsync(id));
        }

        /// <summary>
        /// Obtém estatísticas do currículo 
        /// </summary>
        /// <returns>Estatísticas do currículo</returns>
        [HttpGet("stats")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStats()
        {
            return Ok(await _curriculumService.GetStatsAsync());
        }
    }
}