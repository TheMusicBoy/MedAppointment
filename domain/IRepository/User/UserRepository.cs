using Domain.IRepository;
using Model = Domain.User;

namespace Domain.User
{
    public interface IUserRepository : IRepository<User> {
        bool ExistsByLogin(string login);

        bool ExistsUser(User user);

        bool check(string login, string password);

        User GetByLogin(string login);
    }
}