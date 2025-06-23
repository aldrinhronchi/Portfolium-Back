using Portfolium_Back.Models.ViewModels;

namespace Portfolium_Back.Services.Interfaces
{
    /// <summary>
    /// Interface para serviços de contato
    /// </summary>
    public interface IContactService
    {
        Task<RequestViewModel<ContactMessageViewModel>> GetAllAsync(Int32 Pagina, Int32 RegistrosPorPagina,
            String CamposQuery = "", String ValoresQuery = "", String Ordenacao = "", Boolean Ordem = false);

        Task<RequestViewModel<ContactMessageViewModel>> GetByIdAsync(String id);

        Task<RequestViewModel<ContactMessageViewModel>> CreateAsync(ContactMessageViewModel contactMessageViewModel);

        Task<RequestViewModel<ContactMessageViewModel>> UpdateAsync(ContactMessageViewModel contactMessageViewModel);

        Task<RequestViewModel<ContactMessageViewModel>> DeleteAsync(String id);

        /// <summary>
        /// Envia uma mensagem de contato
        /// </summary>
        /// <param name="contactMessage">Dados da mensagem de contato</param>
        /// <param name="ipAddress">Endereço IP do remetente</param>
        /// <param name="userAgent">User Agent do navegador</param>
        /// <returns>Resultado da operação</returns>
        Task<RequestViewModel<ContactMessageViewModel>> SendMessageAsync(ContactMessageViewModel contactMessage, string? ipAddress, string? userAgent);

        /// <summary>
        /// Obtém informações de contato públicas
        /// </summary>
        /// <returns>Informações de contato</returns>
        Task<RequestViewModel<object>> GetContactInfoAsync();


        /// <summary>
        /// Marca uma mensagem como lida
        /// </summary>
        /// <param name="id">ID da mensagem</param>
        /// <returns>Resultado da operação</returns>
        Task<RequestViewModel<ContactMessageViewModel>> MarkAsReadAsync(Guid id);

        /// <summary>
        /// Obtém a contagem de mensagens não lidas
        /// </summary>
        /// <returns>Contagem de mensagens não lidas</returns>
        Task<RequestViewModel<int>> GetUnreadCountAsync();
    }
} 