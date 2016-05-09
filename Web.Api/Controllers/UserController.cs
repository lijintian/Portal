using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Portal.Applications;
using Portal.Dto;
using Portal.Dto.Common;
using Portal.Dto.Request;
using Portal.Dto.Response;
using Portal.Applications.Services;
using Portal.Infrastructure.Exceptions;
using Portal.Web.Api.Core;
using Portal.Web.Api.Extends;

namespace Portal.Web.Api.Controllers
{
    /// <summary>
    /// 表示用户资源
    /// </summary>
    public class UserController : ApiController
    {
        private IUserManagerService UserManagerService
        {
            get;
            set;
        }

        private ILoginMangerService LoginService
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public UserController(IUserManagerService service, ILoginMangerService loginService)
        {
            this.UserManagerService = service;
            this.LoginService = loginService;
        }

        /// <summary>
        ///  获取用户聚合信息,包括角色与权限信息
        /// </summary>
        [Route("api/users/token/{token}")]
        [HttpGet]
        [ResponseType(typeof(GetUserResponse))]
        public IHttpActionResult GetByToken(string token)
        {
            return this.Ok(ExceptionHandleWrapper.GetResult(() =>
            {
                var response = this.LoginService.Verify(token);
                if (response.IsPassed)
                {
                    return this.UserManagerService.GetPackageUserInfo(response.Identity.LoginName);
                }
                else
                {
                    return new GetUserResponse()
                    {
                        ErrorData = new ErrorData("", "token验证失败,token已过期或不存在.请重新登录")
                    };
                }
            }));
        }

        /// <summary>
        /// 获取用户聚合信息,包括角色与权限信息
        /// </summary>
        /// <param name="identity">标识</param>
        [Route("api/users")]
        [HttpGet]
        [ResponseType(typeof(GetUserResponse))]
        public IHttpActionResult Get(string identity)
        {
            return this.Ok(ExceptionHandleWrapper.GetResult(() =>
            {
                return this.UserManagerService.GetPackageUserInfo(identity);
            }));
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="employeenos">员工号</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        [Route("api/userquery")]
        [HttpGet]
        [ResponseType(typeof(GetUsersResponse))]
        public IHttpActionResult Get(string employeenos, string name)
        {
            var employess = employeenos == null ? null : employeenos.Split(',');
            return this.Ok(ExceptionHandleWrapper.GetResult(() =>
            {
                var users = this.UserManagerService.FindUsers(new FindUserRequest() { UserType = UserType.Employee, DisplayName = name, Employees = employess });
                var query = users.Data.Select(item => new UserInfo() { EmployeeNo = item.EmployeeNo, DispalyName = item.DisplayName, LoginName = item.LoginName });
                return new GetUsersResponse() { Users = query.ToArray() };
            }));
        }


        /// <summary>
        /// 创建新用户
        /// </summary>
        [Route("api/users")]
        [HttpPost]
        [ResponseType(typeof(CreateUserResponse))]
        public IHttpActionResult Create([FromBody]CreateUserRequest request)
        {
            return this.Ok(ExceptionHandleWrapper.GetResult(() =>
            {
                this.UserManagerService.Create(request, PageUtility.GetLogger(this.Request, request.LoginName));
                return new CreateUserResponse();
            }));
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="identity">标识</param>
        /// <param name="request">用户信息</param>
        [Route("api/users")]
        [HttpPut]
        [ResponseType(typeof(UpdateUserRequestResponse))]
        public IHttpActionResult Update(string identity, [FromBody]UpdateUserRequest request)
        {
            return this.Ok(ExceptionHandleWrapper.GetResult(() =>
            {
                var response = new UpdateUserRequestResponse();
                var user = this.UserManagerService.GetByIdentity(identity);
                if (user == null)
                {
                    response.ErrorData = new ErrorData(ErrorCodes.StringCodes.IdentityNotExist,
                        string.Format(ErrorMessage.IdentityNotExist, identity));

                    return response;
                }
                user.DisplayName = request.DisplayName;
                user.Email = request.Email;
                user.ClientNo = request.ClientNo;
                user.Desc = request.Desc;
                this.UserManagerService.Save(user, PageUtility.GetLogger(this.Request, identity));

                return response;
            }));
        }

        /// <summary>
        /// 更改用户密码,需提供旧密码验证
        /// </summary>
        [Route("api/users/changepassword/matchold")]
        [HttpPost]
        [ResponseType(typeof(ChangePasswordResponse))]
        public IHttpActionResult ChangePassword(string identity, [FromBody]ChangePasswordRequest request)
        {
            return this.Ok(ExceptionHandleWrapper.GetResult(() =>
            {
                this.UserManagerService.ChangePassword(identity, request.OldPassword, request.NewPassword, PageUtility.GetLogger(this.Request, identity));
                return new ChangePasswordResponse();
            }));
        }

        /// <summary>
        /// 更改用户密码,不进行旧密码验证，直接更改密码为新密码
        /// </summary>
        [Route("api/users/changepassword/new")]
        //[HttpPost]
        [ResponseType(typeof(ChangePasswordResponse))]
        public IHttpActionResult ChangePassword(string identity, string newPassword)
        {
            return this.Ok(ExceptionHandleWrapper.GetResult(() =>
            {
                this.UserManagerService.ChangePassword(identity, newPassword, PageUtility.GetLogger(this.Request, identity));
                return new ChangePasswordResponse();
            }));
        }

        /// <summary>
        /// 重置用户密码,按特定的算法自动生成新密码
        /// </summary>
        [Route("api/users/resetpassword")]
        [HttpPost]
        [ResponseType(typeof(ResetPasswordResponse))]
        public IHttpActionResult ResetPassword(string identity)
        {
            return this.Ok(ExceptionHandleWrapper.GetResult(() =>
            {
                this.UserManagerService.ResetPassword(identity, PageUtility.GetLogger(this.Request, identity));
                return new ResetPasswordResponse();
            }));
        }

        /// <summary>
        /// 设置用户可用
        /// </summary>
        [Route("api/users/approve")]
        [HttpPost]
        [ResponseType(typeof(ApproveResponse))]
        public IHttpActionResult Approve(string identity)
        {
            return this.Ok(ExceptionHandleWrapper.GetResult(() =>
            {
                this.UserManagerService.Approve(identity, PageUtility.GetLogger(this.Request, identity));
                return new ApproveResponse();
            }));
        }

        /// <summary>
        /// 禁用客户
        /// </summary>
        [Route("api/users/disapprove")]
        [HttpPost]
        [ResponseType(typeof(DisapproveResponse))]
        public IHttpActionResult Disapprove(string identity)
        {
            return this.Ok(ExceptionHandleWrapper.GetResult(() =>
            {
                this.UserManagerService.DisApprove(identity, PageUtility.GetLogger(this.Request, identity));
                return new DisapproveResponse();
            }));
        }

        [Route("api/users/menus")]
        [HttpGet]
        [ResponseType(typeof(GetUserResponse))]
        public IHttpActionResult GetUserMenus(string appId, string identity)
        {
            if (string.IsNullOrEmpty(identity))
            {
                return BadRequest("identity can not allow empty.");
            }

            return this.Ok(new GetUserMenusResponse() { Menus = this.UserManagerService.GetUserMenus(false, appId, identity) });
        }
    }
}