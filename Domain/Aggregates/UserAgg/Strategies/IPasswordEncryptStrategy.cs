
namespace Portal.Domain.Aggregates.UserAgg.Strategies
{
    public interface IPasswordEncryptStrategy
    {
        string Encrypt(string password);
    }
}
