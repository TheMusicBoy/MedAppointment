using Domain.IRepository;

namespace Domain.User
{
    public class UserUseCases {
        private IUserRepository Repository;

        public UserUseCases(IUserRepository repository) {
            Repository = repository;
        }

        public Result SignUpUser(User user) {
            if (string.IsNullOrEmpty(user.Login)       ||
                string.IsNullOrEmpty(user.Password)    ||
                string.IsNullOrEmpty(user.PhoneNumber) ||
                string.IsNullOrEmpty(user.FullName))
                return Result.Fail("Some fields are empty");
            
            if (Repository.ExistsByLogin(user.Login))
                return Result.Fail("User with this login already exists");

            if (Repository.Exists(user.UserId))
                return Result.Fail("User with this ID already exists");

            Repository.Create(user);
            return Result.Ok();
        }

        public Result<User> SignInUser(string login, string password) {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return Result.Fail<User>("Login or password are empty");

            if (!Repository.check(login, password))
                return Result.Fail<User>("Incorrect login or password");

            return Result.Ok(Repository.GetByLogin(login));
        }
    }
}