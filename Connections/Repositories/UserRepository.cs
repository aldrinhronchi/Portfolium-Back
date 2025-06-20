using Portfolium_Back.Connections.Repositories.Interface;
using Portfolium_Back.Context;
using Portfolium_Back.Models;
using Portfolium_Back.Connections.Repositories;

namespace Portfolium_Back.Connections.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(PortfoliumContext context)
            : base(context) { }

        public IEnumerable<User> GetAll()
        {
            return Query(x => x.IsActive);
        }
    }
}