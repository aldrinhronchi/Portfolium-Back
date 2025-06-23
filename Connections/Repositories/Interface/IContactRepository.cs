using Portfolium_Back.Models;

namespace Portfolium_Back.Connections.Repositories.Interface
{
    public interface IContactRepository
    {
        // ContactMessage
        Task<ContactMessage> CreateContactMessageAsync(ContactMessage contactMessage);
        Task<List<ContactMessage>> GetContactMessagesAsync(int page = 1, int size = 10, bool onlyUnread = false);
        Task<ContactMessage?> GetContactMessageByIdAsync(Guid id);
        Task<bool> UpdateContactMessageAsync(ContactMessage contactMessage);
        Task<bool> MarkAsReadAsync(Guid id);
        Task<int> GetUnreadCountAsync();
    }
} 