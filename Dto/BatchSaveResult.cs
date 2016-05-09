using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Dto
{
    /// <summary>
    /// 表示批量保存返回结果
    /// </summary>
    public class BatchSaveResult
    {
        public BatchSaveResult(int updated, int created)
        {
            this.Updated = updated;
            this.Created = created;
        }

        /// <summary>
        /// 表示批量更新数
        /// </summary>
        public int Updated { get; private set; }
        /// <summary>
        /// 表示批量创建数
        /// </summary>
        public int Created { get; private set; }
    }
}
