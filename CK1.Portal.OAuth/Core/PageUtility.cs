using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Portal.SDK.Security;
using Portal.Web.Core;
using UrlHelper = System.Web.Mvc.UrlHelper;

namespace Portal.OAuth.Core
{
    public static class PageUtility
    {
        #region 01.字段
        /// <summary>
        /// 缓存文件的版本
        /// </summary>
        private static readonly Dictionary<string, string> FileVersion = new Dictionary<string, string>();
        #endregion

        #region 02.获取模板名称
        /// <summary>
        /// 获取模板名称
        /// </summary>
        public static string GetLayout
        {
            get
            {
                return "~/Views/Shared/_Layout.cshtml";
            }
        }
        #endregion

        #region 03.获取当前用户信息
        public static string GetUserName
        {
            get
            {
                var user = HttpContext.Current.User as PortalPrincipal;
                if (user == null)
                {
                    return string.Empty;
                }
                return !string.IsNullOrEmpty(user.DisplayName) ? user.DisplayName : user.Identity.Name;
            }
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
    }
}