using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.SDK.Security
{
    /// <summary>
    /// 表示权限检查接口
    /// </summary>
    public interface IPermissionCheck
    {
        /// <summary>
        /// 是否拥有指定权限
        /// </summary>
        bool HasPermission(string code);

        /// <summary>
        /// 是否拥有指定权限列表中的任一权限
        /// </summary>
        bool HasAnyPermission(params string[] codes);

        /// <summary>
        /// 是否拥有指定权限列表中所有权限
        /// </summary>
        bool HasAllPermissions(params string[] codes);
    }
}
