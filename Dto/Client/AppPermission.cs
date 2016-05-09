using System.Collections.Generic;

namespace Portal.Dto
{
    public class AppPermission
    {
        #region 属性
        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 序列编号
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// 是否已审
        /// </summary>
        public bool Approved { get; set; }
        #endregion

        #region 初始化
        #endregion
    }
}
