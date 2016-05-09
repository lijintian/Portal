using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Portal.Dto.Request;
using Portal.Dto.Response;
using Portal.SDK.Common;
using RestSharp;

namespace Portal.SDK.ServiceClient
{
    /// <summary>
    /// 表示用户服务客户端
    /// </summary>
    public static class UserServiceClient
    {
        /// <summary>
        /// 获取指定用户信息
        /// </summary>
        public static GetUserResponse GetUserByToken(string token)
        {
            var request = HttpClientFactory.CreateRequest(
                    string.Format("api/users/token/{0}", token),
                    Method.GET);

            return Helpers.GetResultFromRequest<GetUserResponse>(request);
        }

        /// <summary>
        /// 获取指定用户信息
        /// </summary>
        public static GetUserResponse GetUser(string identity)
        {
            var request = HttpClientFactory.CreateRequest(
                    string.Format("api/users?identity={0}", HttpUtility.UrlEncode(identity)),
                    Method.GET);

            return Helpers.GetResultFromRequest<GetUserResponse>(request);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        public static GetUsersResponse GetUsers(GetUsersRequest request)
        {
            var httpRequest = HttpClientFactory.CreateRequest(
                  string.Format("api/userquery?employeenos={0}&name={1}", 
                    HttpUtility.UrlEncode(string.Join(",", request.Employees)),
                    HttpUtility.UrlEncode(request.Name)),
                  Method.GET);

            return Helpers.GetResultFromRequest<GetUsersResponse>(httpRequest);
        }

        /// <summary>
        /// 创建新用户
        /// </summary>
        public static CreateUserResponse Create(CreateUserRequest request)
        {
            var httpRequest = HttpClientFactory.CreateRequest(
                    "api/users",
                    Method.POST);
            httpRequest.AddBody(request);
            return Helpers.GetResultFromRequest<CreateUserResponse>(httpRequest);   
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        public static UpdateUserRequestResponse Update(string identity, UpdateUserRequest request)
        {
            var httpRequest = HttpClientFactory.CreateRequest(
                    string.Format("api/users?identity={0}", HttpUtility.UrlEncode(identity)),
                    Method.PUT);
            httpRequest.AddBody(request);

            return Helpers.GetResultFromRequest<UpdateUserRequestResponse>(httpRequest);   
        }

        /// <summary>
        /// 更改密码，需提供旧密码进行验证
        /// </summary>
        public static ChangePasswordResponse ChangePassword(string identity, ChangePasswordRequest request)
        {
            var httpRequest = HttpClientFactory.CreateRequest(
                    string.Format("api/users/changepassword/matchold?identity={0}", HttpUtility.UrlEncode(identity)),
                    Method.POST);
            httpRequest.AddBody(request);
            return Helpers.GetResultFromRequest<ChangePasswordResponse>(httpRequest);
        }

        /// <summary>
        /// 更改密码，不需验证旧密码，直接更改为新密码
        /// </summary>
        public static ChangePasswordResponse ChangePassword(string identity, string newPassword)
        {
            var httpRequest = HttpClientFactory.CreateRequest(
                    string.Format("api/users/changepassword/new?identity={0}&newPassword={1}", HttpUtility.UrlEncode(identity), HttpUtility.UrlEncode(newPassword)),
                    Method.POST);
            return Helpers.GetResultFromRequest<ChangePasswordResponse>(httpRequest);
        }

        /// <summary>
        /// 重置密码，密码根据特定的算法生成
        /// </summary>
        public static ResetPasswordResponse ResetPassword(string identity)
        {
            var httpRequest = HttpClientFactory.CreateRequest(
                    string.Format("api/users/resetpassword?identity={0}", HttpUtility.UrlEncode(identity)),
                    Method.POST);
            return Helpers.GetResultFromRequest<ResetPasswordResponse>(httpRequest);
        }

        /// <summary>
        /// 启用指定用户
        /// </summary>
        public static ApproveResponse Approve(string identity)
        {
            var httpRequest = HttpClientFactory.CreateRequest(
                    string.Format("api/users/approve?identity={0}", HttpUtility.UrlEncode(identity)),
                    Method.POST);

            return Helpers.GetResultFromRequest<ApproveResponse>(httpRequest);
        }

        /// <summary>
        /// 禁用指定用户
        /// </summary>
        public static DisapproveResponse DisApprove(string identity)
        {
            var httpRequest = HttpClientFactory.CreateRequest(
                    string.Format("api/users/disapprove?identity={0}", HttpUtility.UrlEncode(identity)),
                    Method.POST);

            return Helpers.GetResultFromRequest<DisapproveResponse>(httpRequest);
        }

      
        /// <summary>
        /// 获取指定用户菜单
        /// </summary>
        /// <param name="identity">用户标识</param>
        /// <param name="appId">应用程序员id, 未指定返回所有的菜单</param>
        /// <returns></returns>
        public static GetUserMenusResponse GetUserMenus(string identity, string appId)
        {
            var httpRequest = HttpClientFactory.CreateRequest(
                    string.Format("api/users/menus?identity={0}&appId={1}", HttpUtility.UrlEncode(identity), HttpUtility.UrlDecode(appId)),
                    Method.GET);

            return Helpers.GetResultFromRequest<GetUserMenusResponse>(httpRequest);
        }
    }
}
