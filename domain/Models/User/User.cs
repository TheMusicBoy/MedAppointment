using R = Domain.Role.Models;
using Domain.Logic;


namespace Domain.User.Models;

public class User
{
    public int UserId { get; }

    public string Login { get; set; }
    public string Password { get; set; }

    public R.Role RoleId { get; set; }

    public string FullName { get; set; }
    public string PhoneNumber { get; set; }

    public User(string login, string password, R.Role roleId, string fullName, string phoneNumber)
    {
        UserId = IdCreator.getInstance().NextId();
        Login = login;
        Password = password;
        RoleId = roleId;
        FullName = fullName;
        PhoneNumber = phoneNumber;
    }

}