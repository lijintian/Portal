/*
 EasyDDD
系统名称：  用户中心管理平台
模块名称：  控制层
创建日期：  2015-07-29

内容摘要：  公用配置资源类
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;
using Portal.Applications.Helper;
using Portal.Dto;
using Portal.Dto.Request.Enum;
using Portal.SDK.Security;
using Portal.Web.Admin.Model;
using Portal.Web.Core;
using Portal.Web.Core.Model;
using UrlHelper = System.Web.Mvc.UrlHelper;

namespace Portal.Web.Admin.Core
{
    public static class PageUtility
    {
        #region 01.字段
        /// <summary>
        /// 缓存文件的版本
        /// </summary>
        private static readonly Dictionary<string, string> FileVersion = new Dictionary<string, string>();
        public static readonly CreatePagingInfo PageInfo = new CreatePagingInfo(5);
        private static readonly string[] Layouts = new[] { "~/Views/Shared/_Layout.cshtml", "~/Views/Shared/_EmptyLayout.cshtml" };

        /// <summary>
        /// 是否已经登陆
        /// </summary>
        public static bool IsAuthenticated
        {
            get { return HttpContext.Current.User.Identity.IsAuthenticated; }
        }

        /// <summary>
        /// 当前登陆用户
        /// </summary>
        public static PortalPrincipal CurrentUser
        {
            get { return (PortalPrincipal)HttpContext.Current.User; }
        }

        public static string GetUserName
        {
            get
            {
                var user = HttpContext.Current.User as PortalPrincipal;
                if (user == null)
                {
                    return "匿名用户";
                }
                return !string.IsNullOrEmpty(user.DisplayName) ? user.DisplayName : user.Identity.Name;
            }
        }
        #endregion

        #region 02.获取模板名称
        /// <summary>
        /// 获取模板名称
        /// </summary>
        public static string GetLayout
        {
            get
            {
                return Layouts[1];
            }
        }
        ///// <summary>
        ///// 获取模板名称
        ///// </summary>
        //public static string GetLayout2
        //{
        //    get
        //    {
        //        string portalFrame = HttpContext.Current.Request.QueryString[CK1PortalAuthenticationConfig.PortalFrameName];
        //        return Layouts[string.IsNullOrEmpty(portalFrame) ? 0 : 1];
        //    }
        //}
        #endregion

        #region 03.获取菜单名称
        /// <summary>
        /// 获取菜单名称
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="defaultTitle">默认标题</param>
        /// <returns>菜单名称</returns>
        public static string GetMenuName(RouteData data, string defaultTitle)
        {
            //string url = data.Values["controller"].ToString();
            return defaultTitle;
        }
        #endregion

        #region 04.将虚拟（相对）路径转换为应用程序绝对路径
        /// <summary>
        /// 将虚拟（相对）路径转换为应用程序绝对路径
        /// </summary>
        /// <param name="url">已呈现的页的 URL</param>
        /// <param name="contentPath">内容的虚拟路径</param>
        /// <returns>应用程序绝对路径</returns>
        public static string Content2(this UrlHelper url, string contentPath)
        {
            // 防止低级错误
            if (url == null || HttpContext.Current == null) throw new Exception("非http请求不可调用此方法");
            if (string.IsNullOrEmpty(contentPath))
            {
                return url.Content(contentPath);
            }
            string lowerUrl = contentPath.ToLower();
            // 给 js 和 css 文件加上版本号
            if (IsTrans(lowerUrl))
            {
                // 读取缓存的(本地的不缓存，以便调试)
                if (FileVersion.ContainsKey(lowerUrl) && IsLocal() == false)
                {
                    return FileVersion[lowerUrl];
                }
                // 将虚拟路径，转换为物理路径
                var path = HttpContext.Current.Server.MapPath(contentPath);
                var file = new System.IO.FileInfo(path);
                // 获取文件的最后修改时间
                var version = file.LastWriteTime.ToString("yyyyMMddHHmmss");
                if (string.IsNullOrEmpty(version))
                {
                    return url.Content(contentPath);
                }
                // 给文件加上版本号
                var newPath = url.Content(contentPath + "?v=" + version);
                // 缓存起来
                FileVersion[lowerUrl] = newPath;
                return newPath;
            }
            return url.Content(contentPath);
        }
        /// <summary>
        /// 是否本地访问(本机及局域网内都返回true,否则返回false)
        /// </summary>
        /// <returns></returns>
        private static bool IsLocal()
        {
            // 防止低级错误
            if (HttpContext.Current == null) throw new Exception("非http请求不可调用此方法");
            var url = HttpContext.Current.Request.Url.OriginalString;
            if (!string.IsNullOrEmpty(url))
            {
                url = url.Trim().ToLower();
                // 本机或局域网的,判断为本地访问
                if (url.StartsWith("http://localhost") ||
                    url.StartsWith("http://127.0.0.") ||
                    url.StartsWith("http://192.168."))
                {
                    return true;
                }
            }
            return false;
        }
        private static bool IsTrans(string path)
        {
            return path.EndsWith(".js") || path.EndsWith(".css") || path.EndsWith(".png");
        }
        /// <summary>
        /// 将虚拟（相对）路径转换为应用程序绝对路径
        /// </summary>
        /// <param name="url">已呈现的页的 URL</param>
        /// <param name="contentPath">内容的虚拟路径</param>
        /// <returns>应用程序绝对路径</returns>
        public static string Ck1Content(this UrlHelper url, string contentPath)
        {
            // 防止低级错误
            if (url == null || HttpContext.Current == null) throw new Exception("非http请求不可调用此方法");
            return string.Format("{0}/{1}", HostUtility.ResourcesUrl, contentPath);
        }
        #endregion

        #region 05.获取权限
        public static bool HasPermission(string permissionCode)
        {
            return CurrentUser.HasPermission(permissionCode);
        }
        #endregion

        #region 06.获取批量操作数据源
        public static string GetBatchImportHtml(List<BatchImportSelect> list)
        {
            StringBuilder builder = new StringBuilder();
            int index = 0;
            string formats = "<li><a href=\"#\" onclick=\"Tab$.ImportDailog({0},'{1}')\">{1}</a></li>";
            if (list != null)
            {
                List<BatchImportSelect> result = list.Where(u => u.HasPermission).ToList();
                if (result.Any())
                {
                    index = result.Count;
                    builder.Append("<div class=\"btn-group\">");
                    builder.Append("<a class=\"btn btn-pink dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"false\">批量操作");
                    builder.Append("<i class=\"ace-icon fa fa-caret-down icon-on-right\"></i></a>");
                    builder.Append("<ul class=\"dropdown-menu\">");
                    foreach (var item in result)
                    {
                        builder.AppendFormat(formats, (int)item.Type, item.Type.GetDescription());
                        if (index > 1)
                        {
                            builder.Append("<li class=\"divider\"></li>");
                        }
                        index++;
                    }
                    builder.Append("</ul>");
                    builder.Append("</div>");
                }
            }
            return builder.ToString();
        }

        public static string GetBatchImportHtml(TemplateType type, string permissionCode)
        {
            bool basPermission = PageUtility.HasPermission(permissionCode);
            return basPermission ? string.Format("<a title=\"批量导入\" class='btn btn-pink' onclick=\"Tab$.ImportDailog({0},'{1}');\">批量导入</a>", (int)type, type.GetDescription()) : string.Empty;
        }
        #endregion

        #region 07.初始化日志
        /// <summary>
        /// 初始化日志
        /// </summary>
        public static SysLoggerDto GetLogger()
        {
            SysLoggerDto logger = new SysLoggerDto
            {
                ApplicationName = PortalAuthenticationConfig.ApplicationName,
                Ip = WebHelper.GetUserIP(),
                CreatedBy = GetUserName,
            };
            if (string.IsNullOrEmpty(logger.ApplicationName))
            {
                logger.ApplicationName = Environment.MachineName;
            }
            #region 获取访问地址及浏览器信息
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request != null)
            {
                var Request = System.Web.HttpContext.Current.Request;
                // 设访问页面
                if (Request.Url != null)
                {
                    logger.Url = logger.Url ?? Request.Url.ToString();
                }
                // 设浏览器
                if (Request.Browser != null)
                {
                    logger.Browser = logger.Browser ?? (Request.Browser.Browser + Request.Browser.Version);
                }
                string userAgent = Request.UserAgent;
                logger.OS = logger.OS ?? WebHelper.GetOsName(userAgent);
            }
            #endregion
            return logger;
        }
        #endregion
    }
}
