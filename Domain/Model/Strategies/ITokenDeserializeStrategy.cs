

namespace Portal.Domain.Model.Strategies
{

    /// <summary>
    /// Token 反序列策略
    /// </summary>
    public interface ITokenDeserializeStrategy
    {
        Token Deserialize(string tokenString);
    }
}
