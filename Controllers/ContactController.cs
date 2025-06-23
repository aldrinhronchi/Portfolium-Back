using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolium_Back.Models.ViewModels;
using Portfolium_Back.Services.Interfaces;

namespace Portfolium_Back.Controllers
{
    /// <summary>
    /// Controller responsável pela gestão de contatos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        /// <summary>
        /// Construtor do ContactController
        /// </summary>
        /// <param name="contactService">Serviço de contato injetado via DI</param>
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        /// <summary>
        /// Lista todas as mensagens de contato com paginação e filtros
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<RequestViewModel<ContactMessageViewModel>>> GetAll(
            [FromQuery] Int32 Pagina = 1,
            [FromQuery] Int32 RegistrosPorPagina = 10,
            [FromQuery] String? CamposQuery = "",
            [FromQuery] String? ValoresQuery = "",
            [FromQuery] String? Ordenacao = "",
            [FromQuery] Boolean Ordem = false)
        {

            return Ok(await _contactService.GetAllAsync(Pagina, RegistrosPorPagina, CamposQuery, ValoresQuery, Ordenacao, Ordem));

        }

        /// <summary>
        /// Busca uma mensagem de contato por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestViewModel<ContactMessageViewModel>>> GetById(String id)
        {

            return Ok(await _contactService.GetByIdAsync(id));

        }

        /// <summary>
        /// Cria uma nova mensagem de contato
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<RequestViewModel<ContactMessageViewModel>>> Create([FromBody] ContactMessageViewModel contactMessageViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


           return Ok(await _contactService.CreateAsync(contactMessageViewModel));
        }

        /// <summary>
        /// Atualiza uma mensagem de contato existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RequestViewModel<ContactMessageViewModel>>> Update(String id, [FromBody] ContactMessageViewModel contactMessageViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _contactService.UpdateAsync(contactMessageViewModel));
        }

        /// <summary>
        /// Exclui uma mensagem de contato (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<RequestViewModel<ContactMessageViewModel>>> Delete(String id)
        {
            return Ok(await _contactService.DeleteAsync(id));
        }

        /// <summary>
        /// Envia uma mensagem de contato
        /// </summary>
        /// <param name="contactMessage">Dados da mensagem de contato</param>
        /// <returns>Resultado da operação</returns>
        /// <response code="200">Mensagem enviada com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        [HttpPost("send")]
        [AllowAnonymous]
        public async Task<IActionResult> SendMessage([FromBody] ContactMessageViewModel contactMessage)
        {
            if (!ModelState.IsValid)
            {
                throw new ValidationException("Dados inválidos para envio da mensagem");
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            return Ok(await _contactService.SendMessageAsync(contactMessage, ipAddress, userAgent));
        }

        /// <summary>
        /// Obtém informações de contato públicas
        /// </summary>
        /// <returns>Informações de contato</returns>
        [HttpGet("info")]
        [AllowAnonymous]
        public async Task<IActionResult> GetContactInfo()
        {
            return Ok(await _contactService.GetContactInfoAsync());
        }

        /// <summary>
        /// Marca uma mensagem como lida
        /// </summary>
        /// <param name="id">ID da mensagem</param>
        /// <returns>Resultado da operação</returns>
        [HttpPatch("messages/{id}/read")]
        [Authorize]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            return Ok(await _contactService.MarkAsReadAsync(id));
        }

        /// <summary>
        /// Obtém a contagem de mensagens não lidas
        /// </summary>
        /// <returns>Contagem de mensagens não lidas</returns>
        [HttpGet("messages/unread-count")]
        [Authorize]
        public async Task<IActionResult> GetUnreadCount()
        {
            return Ok(await _contactService.GetUnreadCountAsync());
        }
    }
}