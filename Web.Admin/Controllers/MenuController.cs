/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  控制层
创建日期：  2015-07-29

内容摘要：  菜单管理
*/
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Portal.Applications.Services;
using Portal.Dto;
using Portal.Mvc;
using Portal.Web.Admin.Core;
using Portal.Web.Admin.Model;
using Portal.Web.Core;
using Portal.Web.Core.Extends;
using Portal.Web.Core.Model;
using PermissionAuthorizationAttribute = Portal.Mvc.PermissionAuthorizationAttribute;

namespace Portal.Web.Admin.Controllers
{
    [PermissionAuthorization(PermissionCodes.Menu)]
    public class MenuController : BaseController
    {
        #region 初始化
        private IMenuManagerService _service;
        private IApplicationManagerService _appManagerService;
        private IPermissionManagerService _permissionService;
        public MenuController(IMenuManagerService menuService, IApplicationManagerService appService, IPermissionManagerService permissionService)
        {
            this._service = menuService;
            this._appManagerService = appService;
            this._permissionService = permissionService;
        }
        #endregion

        #region 菜单列表
        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ActionResult Index(SysMenuTreeParameter parameter)
        {
            parameter.Type = (int)TreeType.Common;
            parameter.JsName = "EditDailog";
            var appList = GetAppList();
            //var list = _service.GetList().ToList();
            List<TreeModel> tree = GetTreeList();
            ViewBag.TreeSource = TreeSource.GetSouce(tree, parameter);
            //获取编辑信息
            Menu info = string.IsNullOrEmpty(parameter.Id) ? new Menu() : _service.GetById(parameter.Id);
            Detail2(info, appList);
            return ReturnView(info, parameter.IsPostBack, "List");
        }
        #endregion

        #region 菜单信息编辑页面
        /// <summary>
        /// 菜单信息编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.Menu_Add, PermissionCodes.Menu_Update })]
        public ActionResult Detail(string id)
        {
            Menu info = string.IsNullOrEmpty(id) ? new Menu() : _service.GetById(id);
            var appList = GetAppList();
            return Detail2(info, appList);
        }
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.Menu_Add, PermissionCodes.Menu_Update })]
        protected ActionResult Detail2(Menu info, List<EnumModel> appList)
        {
            ViewBag.ApplicationList = ListSource.GetSource(appList, true, info.ApplicationId);
            ViewBag.ParentList = GetParentList(info.Id, info.ApplicationId, info.ParentId);
            ViewBag.PermissionList = GetPermissionList(info.PermissionCode);
            return PartialView("Detail", info);
        }
        #endregion

        #region 操作菜单信息
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [PermissionAuthorization(PermissionCodes.Menu_Delete)]
        [JsonException]
        public string Delete(string id)
        {
            _service.Remove(id, PageUtility.GetLogger());
            return ReturnJson("删除成功!", true);
        }

        /// <summary>
        /// 保存菜单信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [PermissionAuthorization(LinkType.Or, new string[] { PermissionCodes.Menu_Add, PermissionCodes.Menu_Update })]
        [JsonException]
        public string Save(Menu info)
        {
            info.InitOperateInfo();
            _service.Save(info, PageUtility.GetLogger());
            return new ReturnModel<string>(info.Id, "保存成功！", true).ToJson();
        }
        ///// <summary>
        ///// 修复数据(删除顶级目录，将2级目录菜单设置为顶级并将应用系统ID设置为applicationId)
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="applicationId"></param>
        ///// <returns></returns>
        //public string Repair(string id, string applicationId)
        //{
        //    string message = _service.Repair(id, applicationId);
        //    if (string.IsNullOrEmpty(message))
        //    {
        //        return ReturnJson("修复成功！", true);
        //    }
        //    return ReturnJson(message, false);
        //}
        #endregion

        #region 事件
        /// <summary>
        /// 应用系统ID改变时的事件
        /// </summary>
        /// <returns></returns>
        public string ApplicationIdChange(Menu info)
        {
            MenuAppChangeInfo source = new MenuAppChangeInfo()
            {
                ParentSource = GetParentList(info.Id, info.ApplicationId, info.ParentId),
                PermissionSource = GetPermissionList(info.PermissionCode)
            };
            return source.ToJson();
        }
        #endregion

        #region 方法
        private List<TreeModel> GetTreeList()
        {
            List<TreeModel> result = new List<TreeModel>();
            List<Menu> menuList = _service.GetList().ToList();
            List<EnumModel> appList = GetAppList();
            var parentId = "0";
            var otherParentId = "none999";
            foreach (var item in appList)
            {
                var tempList = GetTreeList(menuList.Where(u => u.ApplicationId == item.Value), item.Value);
                if (tempList.Any())
                {
                    result.Add(new TreeModel(item.Value, item.Text, parentId, true) { Open = true });
                    result.AddRange(tempList);
                }
            }
            var tempList2 = GetTreeList(menuList.Where(u => string.IsNullOrEmpty(u.ApplicationId)), otherParentId);
            if (tempList2.Any())
            {
                result.Add(new TreeModel(otherParentId, "其他", parentId, true) { Open = true });
                result.AddRange(tempList2);
            }
            return result;
        }
        /// <summary>
        /// 根据parentid获取TreeModel数据源
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<TreeModel> GetTreeList(IEnumerable<Menu> list, string parentId)
        {
            if (list.Any())
            {
                return list.OrderBy(u => u.Order).Select(u => new TreeModel(u.Id, u.GetNameCode(), u.IsFirstParent() ? parentId : u.ParentId.ToString(), u.HasChild(list))).ToList();
            }
            return new List<TreeModel>();
        }

        /// <summary>
        /// 获取上级菜单下拉框数据源
        /// </summary>
        /// <returns></returns>
        private string GetParentList(string id, string applicationId, string parentId)
        {
            var list = string.IsNullOrEmpty(applicationId) ? new Menu[0] : _service.GetList(applicationId).ToArray();
            return GetParentList(list, id, parentId);
        }

        /// <summary>
        /// 获取上级菜单下拉框数据源
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private string GetParentList(IEnumerable<Menu> list, string id, string parentId)
        {
            if (!string.IsNullOrEmpty(id))
            {
                list = list.Where(u => u.Id != id);
            }
            List<IDropDownListInfo> ddlList = list.Select(u => new IDropDownListInfo(u.Id, u.Name, u.ParentId, u.Order)).ToList();
            List<EnumModel> enumList = IDropDownListSource.GetChildList(ddlList, "", true);
            return ListSource.GetSource(enumList, true, parentId);
        }
        /// <summary>
        /// 获取权限码列表
        /// </summary>
        /// <returns></returns>
        private string GetPermissionList(string permissionId)
        {
            IEnumerable<PagePermission> list = _permissionService.GetPagePermissionList(string.Empty);
            if (!list.IsNullOrEmpty())
            {
                List<EnumModel> enumList = new List<EnumModel>();
                List<EnumModel> appList = list.Select(u => new { ApplicationId = u.ApplicationId, ApplictionName = u.ApplictionName }).Distinct().Select(u => new EnumModel(u.ApplicationId, u.ApplictionName) { IsGroup = true }).OrderBy(u => u.Value).ToList();
                foreach (var item in appList)
                {
                    enumList.Add(item);
                    enumList.AddRange(list.Where(u => u.ApplicationId == item.Value).OrderBy(u => u.Order).Select(u => new EnumModel(u.Code ?? string.Empty, string.Format("|--{0}", u.Name))).ToList());
                }
                var html = ListSource.GetSource(enumList, true, permissionId);
                return html;
            }
            return ListSource.GetSource(null, true, permissionId);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取应用系统列表
        /// </summary>
        /// <returns></returns>
        public List<EnumModel> GetAppList()
        {
            IEnumerable<Application> appList = this._appManagerService.GetAll();
            return appList.Any() ? appList.Select(u => new EnumModel(u.Id, u.CnName)).ToList() : null;
        }
        #endregion
    }
}
