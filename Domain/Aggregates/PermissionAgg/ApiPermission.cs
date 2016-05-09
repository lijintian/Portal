namespace Portal.Domain.Aggregates.PermissionAgg
{
    /// <summary>
    /// 表示API权限
    /// </summary>
    public class ApiPermission : Permission
    {
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

        public ApiPermission(string applicationId, string code, string name)
            : base(applicationId, code, name, true)
        {
            this.IsCompatible = false;
            this.IsOpened = false;
            this.IsCustomerGranted = false;
        }
    }
}
