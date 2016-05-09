using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Dto.Response
{
    /// <summary>
    /// 表示获取Token出错响应
    /// </summary>
    public class GetTokenErrorResponse
    {
        public string Code
        {
            get;
            set;
        }

        public string Msg
        {
            get;
            set;
        }

        public string Request
        {
            get;
            set;
        }

        public string ToJson()
        {
            return string.Format("{{ code:{0}, msg:{1}, request:{2} }}", this.Code, this.Msg, this.Request);
        }
    }
}
