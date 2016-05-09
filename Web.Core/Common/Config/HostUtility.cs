using System.Web;
using Portal.SDK.Common;
using Portal.SDK.Security;

namespace Portal.Web.Core
{
    public class HostUtility
    {
        #region 01.字段
        public static readonly HostConfig Config = ConfigUtility<HostConfig>.GetInstance.Config;

        /// <summary>
        /// 当前登陆用户
        /// </summary>
        public static CK1Principal CurrentUser
        {
            get { return (CK1Principal)HttpContext.Current.User; }
        }
        #endregion

        /// <summary>
        /// 资源项目URL
        /// </summary>
        public static string ResourcesUrl
        {
            get { return AppSettingsUtility.Get("ResourcesUrl", "http://res.chukou1.com"); }
        }

        /// <summary>
        /// 注销URL
        /// </summary>
        public static string LoginOutUrl
        {
            get
            {
                return string.Format("{0}Account/Logout", AppSettingsUtility.Get(Constants.PortalWebAddressKey));
            }
        }

        /// <summary>
        /// 上传附件保存路径
        /// </summary>
        public static string SaveRoot
        {
            get
            {
                return FileExtent.GetMapPath(Config.SaveRoot);
            }
        }

        public static string ExcelExportPath
        {
            get
            {
                return FileExtent.GetMapPath(string.Format("{0}\\TempFiles", Config.SaveRoot));
            }
        }
    }
}
