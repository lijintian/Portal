/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  控制管理层
创建日期：  2016-03-02

内容摘要：  系统日志表控制管理
*/

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Portal.Applications.Services;
using Portal.Mvc;
using Portal.Dto;
using Portal.Web.Admin.Controllers;
using Portal.Web.Admin.Core;
using Portal.Web.Core;
using Portal.Web.Core.Extends;
using Portal.Web.Core.Model;
using PermissionAuthorizationAttribute = Portal.Mvc.PermissionAuthorizationAttribute;

namespace Portal.Admin.Controllers
{
    [PermissionAuthorization(PermissionCodes.SysLogger)]
    public class SysLoggerController : BaseController
    {
        #region 初始化
        private readonly ISysLoggerManagerService _service;
        private readonly IApplicationManagerService _appService;
        public SysLoggerController(ISysLoggerManagerService service, IApplicationManagerService appService)
        {
            this._service = service;
            this._appService = appService;
        }
        #endregion

        #region 页面
        public ActionResult Index(FindSysLoggerRequest parameter)
        {
            if (!parameter.IsPostBack)
            {
                //ViewBag.ApplicationSelect = GetApplicationList(parameter.ApplicationId);
                ViewBag.TypeSelect = EnumSource<SysLoggerType>.Options(true);
                ViewBag.LevelSelect = EnumSource<SysLoggerLevel>.Options(true);
            }
            return ReturnView(this._service.Search(parameter), parameter, "List", "SysLoggerPager");
        }

        /// <summary>
        /// 查看页
        /// </summary>
        public ActionResult View(string id)
        {
            return PartialView(GetModel(id));
        }

        /// <summary>
        /// 修改页
        /// </summary>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.SysLogger_Create, PermissionCodes.SysLogger_Update })]
        public ActionResult Detail(string id)
        {
            return PartialView(GetModel(id));
        }
        #endregion

        #region 操作
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.SysLogger_Delete)]
        [JsonException]
        public string Delete(string id)
        {
            this._service.Delete(id);
            return ReturnJson("删除成功！", true);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.SysLogger_Create, PermissionCodes.SysLogger_Update })]
        [JsonException]
        public string Save(SysLoggerDto request)
        {
            request.InitOperateInfo();
            this._service.Save(request);
            return ReturnJson();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取系统下拉框数据源
        /// </summary>
        /// <returns></returns>
        private SelectList GetApplicationList(string systemId = "")
        {
            var appList = _appService.GetAll();
            List<EnumModel> list = !appList.IsNullOrEmpty() ? appList.Select(u => new EnumModel(u.Id, u.CnName)).ToList() : null;
            return ListSource.Options(list, true, systemId);
        }
        private SysLoggerDto GetModel(string id)
        {
            var info = string.IsNullOrEmpty(id) ? new SysLoggerDto() : this._service.GetModel(id);
            return info;
        }
        #endregion
    }
}
