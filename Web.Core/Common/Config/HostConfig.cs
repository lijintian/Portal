namespace Portal.Web.Core
{
    public class HostConfig
    {
        #region 属性
        /// <summary>
        /// Portal门户系统管理平台应用系统ID
        /// </summary>
        public string ApplicationId { get; set; }
        /// <summary>
        /// 无权限访问页面
        /// </summary>
        public string NoPermissionPage { get; set; }
        /// <summary>
        /// 404页面
        /// </summary>
        public string NotFoundPage { get; set; }
        /// <summary>
        /// 错误页面
        /// </summary>
        public string ErrorPage { get; set; }

        /// <summary>
        /// 附件保存路径
        /// </summary>
        public string SaveRoot { get; set; }
        #endregion

        #region 02.初始化
        public HostConfig()
        {
            ApplicationId = "55c879ab46b9f621c8d768a7";
            NoPermissionPage = "/Admin/Control/UnPermission";
            NotFoundPage = "/Admin/Control/NotFound";
            ErrorPage = "/Admin/Control/ServerError";
            SaveRoot = "SaveFiles";
        }
        #endregion
    }
}
