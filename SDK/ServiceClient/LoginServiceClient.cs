using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Dto.Common;
using Portal.Dto.Response;
using Portal.SDK.Common;
using System.Web;
using RestSharp;

namespace Portal.SDK.ServiceClient
{
    /// <summary>
    /// 表示登录服务客户端
    /// </summary>
    public static class LoginServiceClient
    {
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="identity">用户登录名或员工编号</param>
        /// <param name="password">密码</param>
        [Obsolete("请使用Login方法")]
        public static LoginInResponse LoginIn(string identity, string password)
        {
            return Helpers.GetResultFromRequest<LoginInResponse>(string.Format("api/login/in?identity={0}&password={1}", HttpUtility.UrlEncode(identity), HttpUtility.UrlEncode(password)),
                    Method.GET);
        }

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="token">用户token</param>
        [Obsolete("请使用Logout方法")]
        public static LoginOutResponse LoginOut(string token)
        {
            return Helpers.GetResultFromRequest<LoginOutResponse>(string.Format("api/login/out/{0}", token), Method.GET);
        }

        /// <summary>
        /// 检查token有效性
        /// </summary>
        [Obsolete("请使用ValidateToken方法")]
        public static VerifyTokenResponse VerifyToken(string token)
        {
            return Helpers.GetResultFromRequest<VerifyTokenResponse>(string.Format("api/login/token/{0}", token),Method.GET);
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="identity">用户登录名或员工编号</param>
        /// <param name="password">密码</param>
        /// <param name="source">请求的Source</param>
        public static LoginInResponse Login(string identity, string password, string source)
        {
            return Helpers.GetResultFromRequest<LoginInResponse>(string.Format("api/v1/login?identity={0}&password={1}&source={2}", HttpUtility.UrlEncode(identity), HttpUtility.UrlEncode(password), HttpUtility.UrlEncode(source)),
                    Method.POST);
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="identity">用户标识</param>
        /// <param name="hash">哈希密码</param>
        /// <param name="source">请求来源</param>
        /// <returns></returns>
        public static LoginInResponse HashLogin(string identity, string hash, string source)
        {
            return Helpers.GetResultFromRequest<LoginInResponse>(string.Format("api/v1/hashlogin?identity={0}&hash={1}&source={2}", HttpUtility.UrlEncode(identity), HttpUtility.UrlEncode(hash), HttpUtility.UrlEncode(source)),
                    Method.POST);
        }

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="token">用户token</param>
        public static LoginOutResponse LogOut(string token)
        {
            return Helpers.GetResultFromRequest<LoginOutResponse>(string.Format("api/v1/logout/{0}", HttpUtility.UrlEncode(token)),
                   Method.POST);
        }

        /// <summary>
        /// 验证token有效性
        /// </summary>
        public static VerifyTokenResponse ValidateToken(string token)
        {
            return Helpers.GetResultFromRequest<VerifyTokenResponse>(string.Format("api/v1/token/{0}", HttpUtility.UrlEncode(token)),
                   Method.POST);
        }
    }
}
