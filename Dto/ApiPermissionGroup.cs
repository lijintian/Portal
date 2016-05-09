namespace Portal.Dto
{
    /// <summary>
    /// API权限分组
    /// </summary>
    public class ApiPermissionGroup : DomainDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Desc { get; set; }
        
        /// <summary>
        /// 是否已分配权限，如果已经分配，不允许设置权限
        /// </summary>
        public bool HasPermission;

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Checked;

        /// <summary>
        /// 是否已审
        /// </summary>
        public bool Approved;
    }
}
