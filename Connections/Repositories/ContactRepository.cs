using Microsoft.EntityFrameworkCore;
using Portfolium_Back.Connections.Repositories.Interface;
using Portfolium_Back.Context;
using Portfolium_Back.Models;

namespace Portfolium_Back.Connections.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly PortfoliumContext _context;

        public ContactRepository(PortfoliumContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Cria uma nova mensagem de contato
        /// </summary>
        /// <param name="contactMessage">Dados da mensagem</param>
        /// <returns>Mensagem criada</returns>
        public async Task<ContactMessage> CreateContactMessageAsync(ContactMessage contactMessage)
        {
            contactMessage.GuidID = Guid.NewGuid();
            contactMessage.DateCreated = DateTime.UtcNow;
            contactMessage.DateUpdated = DateTime.UtcNow;
            contactMessage.IsActive = true;
            contactMessage.IsRead = false;

            _context.ContactMessages.Add(contactMessage);
            await _context.SaveChangesAsync();

            return contactMessage;
        }

        /// <summary>
        /// Obtém mensagens de contato com paginação
        /// </summary>
        /// <param name="page">Página</param>
        /// <param name="size">Tamanho da página</param>
        /// <param name="onlyUnread">Apenas não lidas</param>
        /// <returns>Lista de mensagens</returns>
        public async Task<List<ContactMessage>> GetContactMessagesAsync(int page = 1, int size = 10, bool onlyUnread = false)
        {
            var query = _context.ContactMessages
                .Where(c => c.IsActive);

            if (onlyUnread)
            {
                query = query.Where(c => !c.IsRead);
            }

            return await query
                .OrderByDescending(c => c.DateCreated)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        /// <summary>
        /// Obtém uma mensagem específica por ID
        /// </summary>
        /// <param name="id">ID da mensagem</param>
        /// <returns>Mensagem encontrada ou null</returns>
        public async Task<ContactMessage?> GetContactMessageByIdAsync(Guid id)
        {
            return await _context.ContactMessages
                .Where(c => c.GuidID == id && c.IsActive)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Atualiza uma mensagem de contato
        /// </summary>
        /// <param name="contactMessage">Dados da mensagem</param>
        /// <returns>True se atualizado com sucesso</returns>
        public async Task<bool> UpdateContactMessageAsync(ContactMessage contactMessage)
        {
            contactMessage.DateUpdated = DateTime.UtcNow;

            _context.ContactMessages.Update(contactMessage);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        /// <summary>
        /// Marca uma mensagem como lida
        /// </summary>
        /// <param name="id">ID da mensagem</param>
        /// <returns>True se marcada como lida com sucesso</returns>
        public async Task<bool> MarkAsReadAsync(Guid id)
        {
            var contactMessage = await _context.ContactMessages.FindAsync(id);
            if (contactMessage == null) return false;

            contactMessage.IsRead = true;
            contactMessage.ReadAt = DateTime.UtcNow;
            contactMessage.DateUpdated = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        /// <summary>
        /// Obtém a contagem de mensagens não lidas
        /// </summary>
        /// <returns>Número de mensagens não lidas</returns>
        public async Task<int> GetUnreadCountAsync()
        {
            return await _context.ContactMessages
                .Where(c => c.IsActive && !c.IsRead)
                .CountAsync();
        }
    }
} 