using Portfolium_Back.Connections.Repositories.Interface;
using Portfolium_Back.Models;

namespace Portfolium_Back.Connections.Repositories.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetAll();
    }
}