using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Web.Core
{
    public class WebConst
    {
        #region 字段
        /// <summary>
        /// 最小限制时间
        /// </summary>
        public static readonly DateTime Mindatetime = new DateTime(1970, 1, 1);

        /// <summary>
        /// 最大限制时间
        /// </summary>
        public static readonly DateTime Maxdatetime = new DateTime(2212, 1, 1);

        /// <summary>
        /// 登陆失败缓存KEY
        /// </summary>
        public static readonly string LoginFailCacheKey = "_LoginFailCache";
        #endregion
    }
}
