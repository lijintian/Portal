using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Infrastructure.Config
{
    /// <summary>
    /// 表示用户等级描述
    /// </summary>
    public class UserLevel
    {
        private UserLevel()
        {
            
        }

        /// <summary>
        /// 等级编号
        /// </summary>
        public int Level
        {
            get;
            internal set;
        }

        /// <summary>
        /// Token过期时间, 单位分钟
        /// </summary>
        public long AccessTokenExpiredMinute
        {
            get;
            internal set;
        }

        /// <summary>
        /// 刷新Token过期时间, 单位分钟
        /// </summary>
        public long RefreshTokenExpiredMinute
        {
            get;
            internal set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc
        {
            get;
            internal set;
        }

        public static readonly UserLevel First = new UserLevel()
        {
            Level = 1,
            AccessTokenExpiredMinute = 60 * 24 * 30,
            RefreshTokenExpiredMinute = 60 * 24 * 365 * 10,
            Desc = "第一等级用户"
        };

        public static readonly UserLevel Two = new UserLevel()
        {
            Level = 2,
            AccessTokenExpiredMinute = 60 * 24 * 30 * 6,
            RefreshTokenExpiredMinute = 60 * 24 * 3650,
            Desc = "第二等级用户"
        };

        public static readonly UserLevel Three = new UserLevel()
        {
            Level = 3,
            AccessTokenExpiredMinute = 60 * 24 * 365 * 2,
            RefreshTokenExpiredMinute = 60 * 24 * 3650 * 2,
            Desc = "第三等级用户"
        };
    }
}
