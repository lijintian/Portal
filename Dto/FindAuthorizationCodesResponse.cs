using System.Collections.Generic;

namespace Portal.Dto
{
    /// <summary>
    /// 表示查询授权码响应
    /// </summary>
    public class FindAuthorizationCodesResponse : PagerFindResponse<AuthorizationCodeDto>
    {
         #region 初始化
        public FindAuthorizationCodesResponse()
        {

        }
        public FindAuthorizationCodesResponse(int total, IEnumerable<AuthorizationCodeDto> codes)
            : base(total, codes)
        {

        }
        #endregion
    }
}
