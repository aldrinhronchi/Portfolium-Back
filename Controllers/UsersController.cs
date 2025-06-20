using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolium_Back.Models.ViewModels;
using Portfolium_Back.Services.Interfaces;

namespace Portfolium_Back.Controllers
{
    /// <summary>
    /// Controller responsável pela gestão de usuários do sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        /// <summary>
        /// Construtor do UsersController
        /// </summary>
        /// <param name="userService">Serviço de usuários injetado via DI</param>
        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Lista usuários com paginação, filtros e ordenação
        /// </summary>
        /// <param name="Pagina">Número da página (padrão: 1)</param>
        /// <param name="RegistrosPorPagina">Quantidade de registros por página (padrão: 10)</param>
        /// <param name="CamposQuery">Campos para filtro separados por ';|;' (ex: Name;|;Email)</param>
        /// <param name="ValoresQuery">Valores para filtro separados por ';|;' (ex: João;|;@gmail.com)</param>
        /// <param name="Ordenacao">Campo para ordenação (ex: Name, Email)</param>
        /// <param name="Ordem">Direção da ordenação: true = crescente, false = decrescente</param>
        /// <returns>Lista paginada de usuários</returns>
        /// <response code="200">Lista de usuários retornada com sucesso</response>
        /// <response code="400">Erro de validação nos parâmetros</response>
        /// <response code="401">Token de autenticação inválido</response>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListarAsync(Int32 Pagina = 1, Int32 RegistrosPorPagina = 10,
            String CamposQuery = "", String ValoresQuery = "", String Ordenacao = "", Boolean Ordem = false)
        {
            var resultado = await this.userService.ListarAsync(Pagina, RegistrosPorPagina, CamposQuery, ValoresQuery, Ordenacao, Ordem);
            return Ok(resultado);
        }

        /// <summary>
        /// Busca um usuário específico pelo ID
        /// </summary>
        /// <param name="id">GUID do usuário a ser buscado</param>
        /// <returns>Dados do usuário encontrado</returns>
        /// <response code="200">Usuário encontrado com sucesso</response>
        /// <response code="400">ID inválido</response>
        /// <response code="401">Token de autenticação inválido</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(String id)
        {
            var user = this.userService.GetById(id);
            return Ok(user);
        }

        /// <summary>
        /// Cria um novo usuário ou atualiza um existente
        /// </summary>
        /// <param name="userViewModel">Dados do usuário a ser criado/atualizado</param>
        /// <returns>Resultado da operação</returns>
        /// <response code="200">Usuário criado/atualizado com sucesso</response>
        /// <response code="400">Dados inválidos ou email já em uso</response>
        /// <response code="401">Token de autenticação inválido</response>
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> SalvarAsync(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var resultado = await this.userService.SalvarAsync(userViewModel);
            return Ok(resultado);
        }

        /// <summary>
        /// Remove um usuário do sistema
        /// </summary>
        /// <param name="id">GUID do usuário a ser removido</param>
        /// <returns>Resultado da operação</returns>
        /// <response code="200">Usuário removido com sucesso</response>
        /// <response code="400">ID inválido</response>
        /// <response code="401">Token de autenticação inválido</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> ExcluirAsync(String id)
        {
            var resultado = await this.userService.ExcluirAsync(id);
            return Ok(resultado);
        }

        /// <summary>
        /// Autentica um usuário no sistema
        /// </summary>
        /// <param name="userViewModel">Credenciais de login (email e senha)</param>
        /// <returns>Token JWT e dados do usuário autenticado</returns>
        /// <response code="200">Autenticação realizada com sucesso</response>
        /// <response code="400">Credenciais inválidas</response>
        /// <response code="401">Email ou senha incorretos</response>
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public IActionResult Authenticate(UserAuthenticateRequestViewModel userViewModel)
        {
            var resultado = this.userService.Authenticate(userViewModel);
            return Ok(resultado);
        }
    }
}