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
        private readonly IUserService _userService;

        /// <summary>
        /// Construtor do UsersController
        /// </summary>
        /// <param name="userService">Serviço de usuários injetado via DI</param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Lista todos os usuários com paginação e filtros
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
        public async Task<ActionResult<RequestViewModel<UserViewModel>>> GetAll(
            [FromQuery] Int32 Pagina = 1,
            [FromQuery] Int32 RegistrosPorPagina = 10,
            [FromQuery] String? CamposQuery = "",
            [FromQuery] String? ValoresQuery = "",
            [FromQuery] String? Ordenacao = "",
            [FromQuery] Boolean Ordem = false)
        {

            var result = await _userService.GetAllAsync(Pagina, RegistrosPorPagina, CamposQuery, ValoresQuery, Ordenacao, Ordem);
            return Ok(result);

        }

        /// <summary>
        /// Busca um usuário por ID
        /// </summary>
        /// <param name="id">GUID do usuário a ser buscado</param>
        /// <returns>Dados do usuário encontrado</returns>
        /// <response code="200">Usuário encontrado com sucesso</response>
        /// <response code="400">ID inválido</response>
        /// <response code="401">Token de autenticação inválido</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<RequestViewModel<UserViewModel>>> GetById(String id)
        {

            var result = await _userService.GetByIdAsync(id);
            return Ok(result);

        }

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        /// <param name="userViewModel">Dados do usuário a ser criado</param>
        /// <returns>Resultado da operação</returns>
        /// <response code="200">Usuário criado com sucesso</response>
        /// <response code="400">Dados inválidos ou email já em uso</response>
        /// <response code="401">Token de autenticação inválido</response>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RequestViewModel<UserViewModel>>> Create([FromBody] UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _userService.CreateAsync(userViewModel));
        }

        /// <summary>
        /// Atualiza um usuário existente
        /// </summary>
        /// <param name="id">GUID do usuário a ser atualizado</param>
        /// <param name="userViewModel">Dados do usuário a ser atualizado</param>
        /// <returns>Resultado da operação</returns>
        /// <response code="200">Usuário atualizado com sucesso</response>
        /// <response code="400">Dados inválidos ou email já em uso</response>
        /// <response code="401">Token de autenticação inválido</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<RequestViewModel<UserViewModel>>> Update(String id, [FromBody] UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            return Ok(await _userService.UpdateAsync(userViewModel));
        }

        /// <summary>
        /// Exclui um usuário (soft delete)
        /// </summary>
        /// <param name="id">GUID do usuário a ser removido</param>
        /// <returns>Resultado da operação</returns>
        /// <response code="200">Usuário removido com sucesso</response>
        /// <response code="400">ID inválido</response>
        /// <response code="401">Token de autenticação inválido</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<RequestViewModel<UserViewModel>>> Delete(String id)
        {

            return Ok(await _userService.DeleteAsync(id));

        }

        /// <summary>
        /// Autentica um usuário
        /// </summary>
        /// <param name="userViewModel">Credenciais de login (email e senha)</param>
        /// <returns>Token JWT e dados do usuário autenticado</returns>
        /// <response code="200">Autenticação realizada com sucesso</response>
        /// <response code="400">Credenciais inválidas</response>
        /// <response code="401">Email ou senha incorretos</response>
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public ActionResult<RequestViewModel<UserViewModel>> Authenticate([FromBody] UserAuthenticateRequestViewModel userViewModel)
        {


            return Ok(_userService.Authenticate(userViewModel));

        }
    }
}