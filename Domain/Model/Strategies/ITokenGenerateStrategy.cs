

namespace Portal.Domain.Model.Strategies
{

    /// <summary>
    /// Token 生成策略
    /// </summary>
    public interface ITokenGenerateStrategy
    {
        Token Generate(string loginName);
    }
}
