/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  控制层
创建日期：  2015-11-20

内容摘要：  授权码管理
*/
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Portal.Applications.Services;
using Portal.Dto;
using Portal.Web.Core;
using Portal.Web.Core.Model;
using Portal.Mvc;

namespace Portal.Web.Admin.Controllers
{
    [Mvc.PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.AuthorizationCode, PermissionCodes.DeveloperApp_AuthCode })]
    public class AuthorizationCodeController : BaseController
    {
        #region 初始化
        private readonly IAuthorizationCodeManagerService _authoService;
        private readonly IUserManagerService _UserService;
        public AuthorizationCodeController(IAuthorizationCodeManagerService authoService, IUserManagerService userService)
        {
            this._authoService = authoService;
            this._UserService = userService;
        }
        #endregion

        #region 页面
        public ActionResult Index(FindAuthorizationCodesRequest parameter)
        {
            var souce = GetPageList(_authoService.FindTokens(parameter), parameter, "AuthPager");
            souce.Where = new Dictionary<string, string> { { "ClientId", parameter.ClientId } };
            return ReturnView(souce, parameter.IsPostBack, "List");
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
