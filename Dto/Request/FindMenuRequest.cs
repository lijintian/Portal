using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Dto
{
    public class FindMenuRequest : PagerFindRequest
    {
        #region 属性
        /// <summary>
        /// 是否共享
        /// </summary>
        public bool? IsShare { get; set; }

        /// <summary>
        /// 所属应用程序
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// 在指定权限范围内
        /// </summary>
        public List<string> PermissionCodeList { get; set; }
        #endregion
    }
}
