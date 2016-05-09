using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Portal.Dto.Common;

namespace Portal.Dto.Response
{
    /// <summary>
    /// 表示获取用户菜单响应
    /// </summary>
    public class GetUserMenusResponse : ResponseBase
    {
        public IEnumerable<Menu> Menus
        {
            get;
            set;
        }
    }
}
