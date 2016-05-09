namespace Portal.Domain.Aggregates.PermissionAgg
{
    /// <summary>
    /// 表示功能权限
    /// </summary>
    public class FunctionPermission : Permission
    {
        public string ParentId { get; private  set; }

        public FunctionPermission(string applicationId, string parentId, string code, string name)
            : base(applicationId, code, name)
        {
            this.ParentId = parentId;
        }

        #region 方法
        public bool HasParent()
        {
            return !string.IsNullOrEmpty(this.ParentId) && this.ParentId != "0";
        } 
        #endregion
    }
}
