using Portal.Dto;

namespace Portal.Applications.Services
{
    /// <summary>
    /// 表示登录管理服务
    /// </summary>
    public interface ILoginMangerService
    {
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="loginInfo">登录信息</param>
        /// <returns>用户标识</returns>
        UserIdentity Login(LoginInfo loginInfo, SysLoggerDto logger);

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="token">用户token</param>
        void LoginOut(string token);

        /// <summary>
        /// 验证token是否有效
        /// </summary>
        /// <param name="token">验证token</param>
        /// <returns>验证结果</returns>
        VerifyTokenResult Verify(string token);
    }
}
