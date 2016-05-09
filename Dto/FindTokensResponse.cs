using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Dto
{
    /// <summary>
    /// 表示分页查询Token响应
    /// </summary>
    public class FindTokensResponse : PagerFindResponse<TokenWrapperDto>
    {
        #region 初始化
        public FindTokensResponse()
        {

        }
        public FindTokensResponse(int total, IEnumerable<TokenWrapperDto> tokens)
            : base(total, tokens)
        {

        }
        #endregion
    }
}
