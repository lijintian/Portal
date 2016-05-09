using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Aggregates.SynchronizationAgg
{
    /// <summary>
    /// 表示信息修改类型
    /// </summary>
    public enum ModifiedType
    {
        UserInfo = 1,
        UserPermission = 2,
        NewPermission = 3
    }
}
