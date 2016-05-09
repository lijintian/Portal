using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Web.Admin.Model
{
    public class MenuAppChangeInfo
    {
        #region 属性
        /// <summary>
        /// 父节点下拉框数据源
        /// </summary>
        public string ParentSource { get; set; }

        /// <summary>
        /// 权限下拉框数据源
        /// </summary>
        public string PermissionSource { get; set; }
        #endregion
    }
}
