namespace Portal.Dto
{
    /// <summary>
    /// 表示API权限
    /// </summary>
    public class ApiPermission : ApplicationPermission
    {
        #region 属性
        /// <summary>
        /// 是否对外开的权限，即为外部应用授权的权限
        /// </summary>
        public bool IsOpened
        {
            get;
            set;
        }

        /// <summary>
        /// 该权限是否需要客户授权，通常还应该标识对外开放
        /// </summary>
        public bool IsCustomerGranted
        {
            get;
            set;
        }
        #endregion

        #region 初始化
        public ApiPermission()
        {
            this.IsCompatible = false;
        }
        #endregion
    }
}
