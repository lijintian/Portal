using System.Collections.Generic;

namespace Portal.Web.Core.Model
{
    public class PageListSource<T>
    {
        #region 字段
        /// <summary>
        /// 第几页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页显示多少条
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public long TotalRows { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// 分页的HTML
        /// </summary>
        public string Pageing { get; set; }
        /// <summary>
        /// 绑定的数据源
        /// </summary>
        public List<T> Source { get; set; }
        /// <summary>
        /// Where条件
        /// </summary>
        public Dictionary<string, string> Where { get; set; }
        #endregion

        #region 初始化
        public PageListSource<T1> GetSource<T1, T2>(PageListSource<T2> oldSource, List<T1> souce)
        {
            PageListSource<T1> source1 = new PageListSource<T1>
            {
                Sort = oldSource.Sort,
                PageIndex = oldSource.PageIndex,
                PageSize = oldSource.PageSize,
                TotalRows = oldSource.TotalRows,
                TotalPages = oldSource.TotalPages,
                Pageing = oldSource.Pageing,
                Where = oldSource.Where,
                Source = souce
            };
            return source1;
        }
        #endregion

        #region 方法
        public string GetValue(string key)
        {
            return this.Where != null && this.Where.ContainsKey(key) ? this.Where[key] : "";
        }
        #endregion
    }
}
