using Domain.Models.Role;
using Domain.Logic;


namespace Domain.Models.User;

public class User
{
    private int UserId { get; }

    public string Login { get; set; }
    public string Password { get; set; }

    public Role.Role RoleId { get; set; }

    public string FullName { get; set; }
    public string PhoneNumber { get; set; }

    public User(string login, string password, Role.Role roleId, string fullName, string phoneNumber)
    {
        UserId = IdCreator.getInstance().NextId();
        Login = login;
        Password = password;
        RoleId = roleId;
        FullName = fullName;
        PhoneNumber = phoneNumber;
    }

}