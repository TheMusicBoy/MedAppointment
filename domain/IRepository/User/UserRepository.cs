using Domain.IRepository;
using Model = Domain.User.Models;

namespace Domain.User.IUserRepository
{
    public interface IUserRepository : IRepository.IRepository<Models.User> {
        bool ExistsByLogin(string login);

        bool ExistsById(int id);

        bool Exists(Model.User user);

        bool check(string login, string password);

        Model.User GetByLogin(string login);
    }
}