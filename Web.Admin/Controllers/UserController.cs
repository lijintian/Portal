/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  控制层
创建日期：  2015-07-29

内容摘要：  用户管理、客户帐号管理、API帐号管理
*/

using System.Collections.Generic;
using System.Data;
using System.Web.Configuration;
using System.Web.Mvc;
using Portal.Applications.Services;
using Portal.Dto;
using Portal.Dto.Request.Enum;
using Portal.Mvc;
using Portal.Web.Admin.Core;
using Portal.Web.Admin.Model;
using Portal.Web.Core;
using Portal.Web.Core.Common;
using Portal.Web.Core.Extends;
using PermissionAuthorizationAttribute = Portal.Mvc.PermissionAuthorizationAttribute;

namespace Portal.Web.Admin.Controllers
{
    [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.User, PermissionCodes.Customer, PermissionCodes.APIUser })]
    public class UserController : BaseController
    {
        #region 初始化
        private IUserManagerService _service;
        public UserController(IUserManagerService appService)
        {
            this._service = appService;
        }
        #endregion

        #region 用户列表
        private FindUserResponse GetList(FindUserRequest parameter, UserType type)
        {
            parameter.UserType = type;
            var result = _service.FindUsers(parameter);
            return result;
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.User)]
        public ActionResult Index(FindUserRequest parameter)
        {
            if (!parameter.IsPostBack)
            {
                ViewBag.BatchImportSource = GetBatchImportHtml();
                ViewBag.SearchTypeSource = EnumSource<UserSearchType>.Options(((int)UserSearchType.LoginName).ToString());
            }
            return ReturnView(GetList(parameter, UserType.Employee), parameter, "List", "SysUserPager");
        }

        /// <summary>
        /// 客户帐号管理
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.Customer)]
        public ActionResult Customer(FindUserRequest parameter)
        {
            if (!parameter.IsPostBack)
            {
                ViewBag.BatchImportSource = GetCustomerBatchImportHtml();
            }
            return ReturnView(GetList(parameter, UserType.Customer), parameter, "CustomerList", "SysUserPager");
        }

        /// <summary>
        /// API帐号列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.APIUser)]
        public ActionResult Api(FindUserRequest parameter)
        {
            parameter.IsApi = true;
            return ReturnView(_service.FindUsers(parameter), parameter, "APIList", "SysUserPager");
        }

        /// <summary>
        /// 获取用户信息的json
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.User_SetPermission, PermissionCodes.Customer_SetPermission, PermissionCodes.APIUser_SetPermission })]
        public ActionResult GetUserJson(FindUserRequest parameter)
        {
            parameter.PageIndex = 1;
            parameter.PageSize = 10;
            if (parameter.UserType == UserType.InternalApi)
            {
                parameter.IsApi = true;
                FindUserResponse users = _service.FindUsers(parameter);
                return this.Json(users.Data);
            }
            else
            {
                FindUserResponse users = GetList(parameter, parameter.UserType);
                return this.Json(users.Data);
            }
        }
        #endregion

        #region 用户信息编辑页面
        /// <summary>
        /// 用户信息编辑页面
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.APIUser_Add, PermissionCodes.APIUser_Update })]
        public ActionResult Detail(string userId)
        {
            User info = string.IsNullOrEmpty(userId) ? new User() : _service.GetById(userId);//{ LoginName = "732320850@qq.com", Email = "732320850@qq.com",Phone = "13111111111"}
            return PartialView(info);
        }
        #endregion

        #region 操作用户信息
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.User_Approve, PermissionCodes.User_DisApprove, PermissionCodes.Customer_Approve, PermissionCodes.Customer_DisApprove, PermissionCodes.APIUser_Approve, PermissionCodes.APIUser_DisApprove })]
        [JsonException]
        public string Delete(SysUserParameter parameter)
        {
            if (parameter.State == 1)//启用
            {
                _service.Approve(parameter.LoginName, PageUtility.GetLogger());
            }
            else//禁用
            {
                _service.DisApprove(parameter.LoginName, PageUtility.GetLogger());
            }
            return ReturnJson();
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.APIUser_SetRole)]
        [JsonException]
        public string ResetPassword(string loginName)
        {
            _service.ResetPassword(loginName, PageUtility.GetLogger());
            return ReturnJson();
        }
        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.APIUser_Add, PermissionCodes.APIUser_Update })]
        [JsonException]
        public string Save(User info)
        {
            info.UserType = UserType.InternalApi;
            if (string.IsNullOrEmpty(info.Id))
            {
                if (!_service.IsUniqueLoginName(info.LoginName))
                {
                    return ReturnJson("保存失败，已存在相同的用户名称！");
                }
            }
            info.InitOperateInfo();
            bool isSetApproved = info.IsApproved;
            _service.Save(info, PageUtility.GetLogger(), isSetApproved);
            return ReturnJson();
            //return ReturnJson("保存成功，同时为您生成的访问Token为XXXXXXX, 请妥善保存", true);
        }
        /// <summary>
        /// 检测用户名称是否重复
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        [JsonException]
        public string Check(string loginName)
        {
            return _service.IsUniqueLoginName(loginName) ? ReturnJson() : ReturnJson("已存在相同的用户名称！");
        }

        /// <summary>
        /// 导出
        /// </summary>
        public void Export(FindUserRequest parameter)
        {
            parameter.UserType = UserType.Employee;
            var list = _service.GetList(parameter);
            DataTable table = new DataTable();
            table.Columns.Add("用户名称");
            table.Columns.Add("员工号");
            table.Columns.Add("员工名称");
            table.Columns.Add("Email");
            table.Columns.Add("创建时间");
            table.Columns.Add("状态");
            if (!LengthUtility.IsNullOrEmpty(list))
            {
                foreach (var item in list)
                {
                    DataRow row = table.NewRow();
                    row["用户名称"] = item.LoginName;
                    row["员工号"] = item.EmployeeNo;
                    row["员工名称"] = item.DisplayName;
                    row["Email"] = item.Email;
                    row["创建时间"] = item.CreatedOn.Show("yyyy-MM-dd HH:mm");
                    row["状态"] = item.IsApproved ? "启用" : "禁用";
                    table.Rows.Add(row);
                }
            }
            ExcelUtility.Export(table.DefaultView, "用户信息.xls");
        }
        #endregion

        #region 私有方法
        private string GetBatchImportHtml()
        {
            List<BatchImportSelect> list = new List<BatchImportSelect>
            {
                new BatchImportSelect(TemplateType.CreateUserRole, PermissionCodes.CreateUserRole),
                new BatchImportSelect(TemplateType.DeleteUserRole, PermissionCodes.DeleteUserRole),
                new BatchImportSelect(TemplateType.CreateUserPermission, PermissionCodes.CreateUserPermission),
                new BatchImportSelect(TemplateType.DeleteUserPermission, PermissionCodes.DeleteUserPermission)
            };
            return PageUtility.GetBatchImportHtml(list);
        }
        private string GetCustomerBatchImportHtml()
        {
            List<BatchImportSelect> list = new List<BatchImportSelect>
            {
                new BatchImportSelect(TemplateType.CreateCustomerRole, PermissionCodes.CreateCustomerRole),
                new BatchImportSelect(TemplateType.DeleteCustomerRole, PermissionCodes.DeleteCustomerRole),
                new BatchImportSelect(TemplateType.CreateCustomerPermission, PermissionCodes.CreateCustomerPermission),
                new BatchImportSelect(TemplateType.DeleteCustomerPermission, PermissionCodes.DeleteCustomerPermission)
            };
            return PageUtility.GetBatchImportHtml(list);
        }
        #endregion
    }
}
