/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  控制层
创建日期：  2015-11-20

内容摘要：  Token管理
*/
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Portal.Applications.Services;
using Portal.Dto;
using Portal.Web.Admin.Core;
using Portal.Web.Core;
using Portal.Web.Core.Extends;
using Portal.Web.Core.Model;
using Portal.Mvc;

namespace Portal.Web.Admin.Controllers
{
    [Mvc.PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.Token, PermissionCodes.DeveloperApp_Token })]
    public class TokenController : BaseController
    {
        #region 初始化
        private readonly ITokenManagerService _TokenService;
        private readonly IUserManagerService _UserService;
        public TokenController(ITokenManagerService tokenService, IUserManagerService userService)
        {
            this._TokenService = tokenService;
            this._UserService = userService;
        }
        #endregion

        #region 页面
        public ActionResult Index(FindTokensRequest parameter)
        {
            var souce = GetPageList(_TokenService.FindTokens(parameter), parameter, "TokenPager");
            souce.Where = new Dictionary<string, string> { { "ClientId", parameter.ClientId } };
            return ReturnView(souce, parameter.IsPostBack, "List");
        }
        #endregion

        #region 操作
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [Mvc.PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.Token_Approve, PermissionCodes.Token_DisApprove })]
        [JsonException]
        public string Delete(string id, string state)
        {
            if (state == "1")//启用
            {
                _TokenService.SetEnable(id, PageUtility.GetLogger());
            }
            else//禁用
            {
                _TokenService.SetDisabled(id, PageUtility.GetLogger());
            }
            return ReturnJson();
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
