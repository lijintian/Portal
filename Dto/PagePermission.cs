using System.Collections.Generic;

namespace Portal.Dto
{
    /// <summary>
    /// 表示页面权限
    /// </summary>
    public class PagePermission : ApplicationPermission
    {
        private readonly List<FunctionPermission> _functionPermissions = new List<FunctionPermission>();

        public PagePermission()
        {

        }

        public IEnumerable<FunctionPermission> FunctionPermissions
        {
            get
            {
                return this._functionPermissions;
            }
        }

        public void AddFuncPermission(FunctionPermission funcPer)
        {
            this._functionPermissions.Add(funcPer);
        }
        /// <summary>
        /// 功能权限
        /// 系统页面权限列表，返回的格式如：新增[PageCreate]&编辑[PageUpdate]&删除[PageDelete]
        /// 新增时，返回的格式如：[{"Id":"1","Name":"新增","Code":"UserAdd"},{"Id":"2","Name":"编辑","Code":"UserUpdate"}]
        /// </summary>
        public string FunctionPermissionSummary { get; set; }
    }
}
