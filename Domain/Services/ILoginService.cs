using Portal.Domain.Model;

namespace Portal.Domain.Services
{
    public interface ILoginService
    {
        UserIdentity Login(string loginName, string password, LoginType type);

    }

    public enum LoginType
    {
        //password 密码需进行hash后再比较
        Password,

        //直接比较hash密码
        HashPassword
    }
}
