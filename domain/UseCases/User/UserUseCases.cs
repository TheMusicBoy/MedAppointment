using Model = Domain.User.Models;
using URep = Domain.User.IRepository;
using Rep = Domain.IRepository;

namespace Domain.User.UseCases
{
    class UserUseCases {
        private URep.IUserRepository Repository;

        public UserUseCases(URep.IUserRepository repository) {
            Repository = repository;
        }

        public Rep.Result SignUpUser(Model.User user) {
            if (string.IsNullOrEmpty(user.Login)       ||
                string.IsNullOrEmpty(user.Password)    ||
                string.IsNullOrEmpty(user.PhoneNumber) ||
                string.IsNullOrEmpty(user.FullName))
                return Rep.Result.Fail("Some fields are empty");
            
            if (Repository.ExistsByLogin(user.Login))
                return Rep.Result.Fail("User with this login already exists");

            if (Repository.Exists(user.UserId))
                return Rep.Result.Fail("User with this ID already exists");

            Repository.Create(user);
            return Rep.Result.Ok();
        }

        public Rep.Result<Model.User> SignInUser(string login, string password) {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return Rep.Result.Fail<Model.User>("Login or password are empty");

            if (!Repository.check(login, password))
                return Rep.Result.Fail<Model.User>("Incorrect login or password");

            return Rep.Result.Ok(Repository.GetByLogin(login));
        }
    }
}