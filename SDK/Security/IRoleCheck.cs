using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Portal.SDK.Security
{
    /// <summary>
    /// 表示角色检查接口
    /// </summary>
    public interface IRoleCheck
    {
        /// <summary>
        /// 是否拥有指定角色列表的所有角色
        /// </summary>
        bool IsInAllRole(params string[] roles);

        /// <summary>
        /// 是否拥有指定角色列表的任一角色
        /// </summary>
        bool IsInAnyRole(params string[] roles);
    }
}
