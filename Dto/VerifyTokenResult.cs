using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Dto
{
    /// <summary>
    /// 表示token验证结果
    /// </summary>
    public class VerifyTokenResult
    {
        public VerifyTokenResult(bool isPassed, UserIdentity identity)
        {
            this.IsPassed = isPassed;
            this.Identity = identity;
        }

        /// <summary>
        /// 表示是否通过验证
        /// </summary>
        public bool IsPassed { get; private set; }

        /// <summary>
        /// 表示当前用户标识
        /// </summary>
        public UserIdentity Identity { get; private set; }
    }
}
