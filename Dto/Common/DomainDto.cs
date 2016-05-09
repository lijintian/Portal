using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Dto
{
    /// <summary>
    /// 表示领域对象dto
    /// </summary>
    public abstract class DomainDto : AggregateDto
    {
        #region 属性
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 创建人员账号
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
        /// <summary>
        /// 更新人员账号
        /// </summary>
        public string UpdatedBy { get; set; }
        #endregion

        #region 方法
        #endregion
    }
}
