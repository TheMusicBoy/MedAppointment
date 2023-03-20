using Domain.IRepository;
using Model = Domain.User.Models;

namespace Domain.User.IRepository
{
    public interface IUserRepository : IRepository<Models.User> {
        bool ExistsByLogin(string login);

        bool Exists(Model.User user);

        bool check(string login, string password);

        Model.User GetByLogin(string login);
    }
}