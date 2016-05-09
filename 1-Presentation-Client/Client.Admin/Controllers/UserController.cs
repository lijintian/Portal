/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  控制层
创建日期：  2015-07-29

内容摘要：  外部开发者账号管理
*/

using System.Web.Mvc;
using Portal.Applications.Services;
using Portal.Client.Core;
using Portal.Dto;
using Portal.Web.Core;
using Portal.Web.Core.Extends;

namespace Portal.Client.Controllers
{
    public class UserController : BaseController
    {
        #region 初始化
        private IUserManagerService _service;
        //private const string _registeredValidateCode = "ValidateCode";//注册验证码
        public UserController(IUserManagerService appService)
        {
            this._service = appService;
        }
        #endregion

        #region 页面
        /// <summary>
        /// 注册
        /// </summary>
        [Mvc.PermissionAuthorization(ClientPermissionCodes.ExternalUser)]
        public ActionResult Index()
        {
            if (PageUtility.IsLogin())
            {
                User info = _service.GetByIdentity(CurrentUser.Identity.Name);
                return View("View", info);
            }
            else
            {
                return View("Create", GetModel());
            }
        }

        [AllowAnonymous]
        public ActionResult Create()
        {
            return View(GetModel());
        }

        [AllowAnonymous]
        public ActionResult View(string identity)
        {
            if (string.IsNullOrEmpty(identity) && PageUtility.IsLogin())
            {
                identity = CurrentUser.Identity.Name;
            }
            if (!string.IsNullOrEmpty(identity))
            {
                return View(GetModel(identity));
            }
            return View("Create", GetModel());
        }

        [Mvc.PermissionAuthorization(ClientPermissionCodes.ExternalUser_Update)]
        public ActionResult Detail(string identity)
        {
            if (string.IsNullOrEmpty(identity) && PageUtility.IsLogin())
            {
                identity = CurrentUser.Identity.Name;
            }
            return View(GetModel(identity));
        }
        #endregion

        #region 操作
        [AllowAnonymous]
        [JsonException]
        public string Registered(User info, string validateKey, string code)
        {
            info.UserType = UserType.ExternalApi;
            if (Session[validateKey] == null)
            {
                return ReturnJson("验证码已过期！");
            }
            string msg = ValidateCodeHelper.CheckCode(validateKey, code);
            if (!string.IsNullOrEmpty(msg))
            {
                return ReturnJson(msg);
            }
            if (!_service.IsUniqueLoginName(info.LoginName))
            {
                return ReturnJson("保存失败，已存在相同的用户名称！");
            }
            info.InitOperateInfo(info.LoginName);
            _service.Save(info, PageUtility.GetLogger(), true);
            return ReturnJson();
        }

        [Mvc.PermissionAuthorization(ClientPermissionCodes.ExternalUser_Update)]
        [JsonException]
        public string Save(User info, string code)
        {
            info.UserType = UserType.ExternalApi;
            info.InitOperateInfo();
            _service.Save(info, PageUtility.GetLogger(), true);
            return ReturnJson();
        }
        #endregion

        #region 私有方法
        private User GetModel()
        {
            return new User();
        }
        private User GetModel(string identity)
        {
            User info = new User();
            if (!string.IsNullOrEmpty(identity))
            {
                info = _service.GetByIdentity(identity);
                //if (info.CreatedBy != CurrentUser.Identity.Name)
                //{
                //    //拋出异常：没有权限查看账号信息
                //}
            }
            return info;
        }
        #endregion
    }
}
