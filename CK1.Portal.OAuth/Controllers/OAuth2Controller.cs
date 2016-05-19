using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Portal.Applications.Services;
using Portal.Dto.Request;
using Portal.Dto.Response;
using Portal.Infrastructure.Exceptions;
using Portal.SDK.Security;

namespace Portal.OAuth.Controllers
{
    /// <summary>
    /// 表示Oauth
    /// </summary>
    [Authorize]
    public class OAuth2Controller : Controller
    {
        private readonly ICustomerAuthorizationManagerService _camService;
        private readonly IDeveloperAppManagerService _appService;
        private readonly IApiPermissionGroupManagerService _apgService;
        private PortalIdentity _identity;
        public OAuth2Controller(ICustomerAuthorizationManagerService camService, 
            IDeveloperAppManagerService appService, 
            IApiPermissionGroupManagerService apgService)
        {
            this._camService = camService;
            this._appService = appService;
            this._apgService = apgService;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this._identity = (PortalIdentity)this.User.Identity;
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 客户授权界面
        /// 测试地址1：http://openapilocal.test-abc.cn/OAuth2/Authorization?scope=afds_safdsafd,bbcc&redirect_uri=http://safdsa.com/back&response_type=code&client_id=ZGZmZjllMGQtMjg5MS00MGUyLThmZjMtZjVkN2VkMjhiMjNl&state=9ac8a7a074d5a866b57ac1fa63617a66
        /// 测试地址2：http://openapilocal.test-abc.cn/OAuth2/Authorization?scope=afds_safdsafd,bbcc&redirect_uri=http://safdsa.com/back&response_type=token&client_id=ZGZmZjllMGQtMjg5MS00MGUyLThmZjMtZjVkN2VkMjhiMjNl&state=9ac8a7a074d5a866b57ac1fa63617a66
        /// </summary>
        /// <returns></returns>
        public ActionResult Authorization(string client_id, string scope)
        {
            if (string.IsNullOrWhiteSpace(client_id))
            {
                this.ModelState.AddModelError("client_id", "应用ClientId不能为空");
            }
            if (string.IsNullOrWhiteSpace(scope))
            {
                this.ModelState.AddModelError("scope", "授权范围scope必须指定");
            }
            this.SetViewBag(client_id, scope);
            
            return View();
        }

        /// <summary>
        /// 执行授权
        /// </summary>
        [HttpPost]
        public ActionResult DoAuthorization(string response_type, string redirect_uri, string client_id, string scope)
        {
            try
            {
                var response = this._camService.RequestAuthorization(new AuthorizationRequest()
                {
                    ClientId = client_id,
                    CustomerId = this._identity.Name,
                    RedirectUri = redirect_uri,
                    ResponseType = response_type,
                    Scope = scope
                });

                return new RedirectResult(this.AppendAppQuery(redirect_uri, response));
            }
            catch (PortalException ex)
            {
                this.ModelState.AddModelError(ex.ErrorCode, ex.Message);
                this.SetViewBag(client_id, scope);
                return this.View("Authorization");
            }
        }

        private void SetViewBag(string clientId, string scope)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                this.ViewBag.AppName = string.Empty;
                this.ViewBag.ApiGroups = new List<Dto.ApiPermissionGroup>();
            }
            else
            {
                var app = this._appService.GetByClientId(clientId);
                var scopes = scope.Split(',');
                this.ViewBag.AppName = app.Name;
                this.ViewBag.ApiGroups = this._apgService.GetList(app.ApprovedGroupList.ToArray()).Where(item => scopes.Contains(item.Code)).ToList();
            }
            
        }
        private string AppendAppQuery(string callbackUrl, AuthorizationResponse response)
        {
            Func<string, string> getLimit = url => url.Contains("?") ? "&" : "?";
            var excludeKeys = new string[] { "response_type", "redirect_uri", "client_id", "scope" };
            foreach (var el in this.Request.QueryString.AllKeys)
            {
                if (!excludeKeys.Contains(el))
                {
                    callbackUrl += string.Format("{0}{1}={2}", getLimit(callbackUrl), el, this.Request.QueryString[el]);
                }
            }

            if (response.IsUseAuthorizationCode)
            {
                callbackUrl += string.Format("{0}code={1}", getLimit(callbackUrl), response.CodeOrToken);
            }
            else
            {
                callbackUrl += string.Format("#{0}", response.CodeOrToken);
            }

            return callbackUrl;
        }
	}
}