using System.Collections.Generic;
using System.Linq;
using Portal.Dto.Response;

namespace Portal.Dto
{
    /// <summary>
    /// 表示分页查询响应
    /// </summary>
    public abstract class PagerFindResponse<T>
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
        public PagerFindResponse()
        {

        }
        public PagerFindResponse(int total, IEnumerable<T> roles)
        {
            this.TotalRecords = total;
            this.Data = roles ?? Enumerable.Empty<T>();
        }
        public PagerFindResponse(GetListResponse<T> data)
        {
            this.TotalRecords = data.TotalRecords;
            this.TotalPages = data.TotalPages;
            this.Data = data.Data;
        }
        #endregion

        #region 方法
        public void Init(int total, int page, IEnumerable<T> data)
        {
            this.TotalRecords = total;
            this.TotalPages = page;
            this.Data = data ?? Enumerable.Empty<T>();
        }
        #endregion
    }
}
