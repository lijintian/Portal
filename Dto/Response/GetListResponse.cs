using System.Collections.Generic;
using Portal.Dto.Common;

namespace Portal.Dto.Response
{
    public class GetListResponse<T> : ResponseBase
    {
        #region 属性
        /// <summary>
        /// 查询记录数
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// 总的页面数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public IEnumerable<T> Data { get; set; }
        #endregion

        #region 初始化
        public GetListResponse()
        {
        }

        public GetListResponse(PagerFindResponse<T> data)
        {
            this.TotalRecords = data.TotalRecords;
            this.TotalPages = data.TotalPages;
            this.Data = data.Data;
        }
        #endregion
    }
}
