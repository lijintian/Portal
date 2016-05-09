using System;
using System.Collections.Generic;
using System.Linq;
using Portal.Dto;
using Portal.Web.Core.Model;

namespace Portal.Web.Core
{
    public class PageingCreator
    {
        #region 01.分页结果
        /// <summary>
        /// 分页结果
        /// </summary>
        /// <returns></returns>
        public static PageListSource<T1> GetList<T1>(PagerFindResponse<T1> source, PagerFindRequest parameter, CreatePagingInfo pageInfo, string jsname) where T1 : new()
        {
            parameter.PageIndex = parameter.PageIndex > 0 ? parameter.PageIndex : 1;
            BaseParameter parameter2 = new BaseParameter(parameter) { TotalPages = source.TotalPages, TotalRows = source.TotalRecords };
            var list= GetList(source.Data.ToList(), parameter2, pageInfo, jsname);
            parameter.PageIndex = parameter2.PageIndex;
            return list;
        }

        /// <summary>
        /// 分页结果
        /// </summary>
        /// <returns></returns>
        public static PageListSource<T1> GetList<T1>(List<T1> source, BaseParameter parameter, CreatePagingInfo pageInfo, string jsname) where T1 : new()
        {
            PageListSource<T1> dataSource = new PageListSource<T1> { Source = source };
            if (parameter.NoPage)
            {
                dataSource.TotalRows = source == null ? 0 : source.Count;
            }
            else
            {
                dataSource.Sort = parameter.Sort;
                dataSource.PageIndex = parameter.PageIndex;
                dataSource.PageSize = parameter.PageSize;
                dataSource.TotalRows = parameter.TotalRows;
                dataSource.TotalPages = parameter.TotalPages = GetTotalPages(parameter);
                if (!string.IsNullOrEmpty(jsname) && parameter.TotalRows > 0)
                {
                    PagingCalc(parameter);
                    dataSource.Pageing = PagingUtility.Paging(parameter, pageInfo, jsname);
                }
            }
            return dataSource;
        }
        /// <summary>
        /// 设置PageInfo
        /// </summary>
        /// <param name="parameter"></param>
        private static void InitPageInfo(BaseParameter parameter)
        {
            parameter.TotalPages = GetTotalPages(parameter);
            PagingCalc(parameter);
        }
        #endregion

        #region 02.计算页码
        /// <summary>
        /// 计算分页
        /// </summary>
        private static void PagingCalc(BaseParameter condition)
        {
            if (condition.TotalRows > 0)
            {
                //condition.TotalPages = Convert.ToInt32(condition.TotalRows / condition.PageSize + (condition.TotalRows % condition.PageSize > 0 ? 1 : 0));
                condition.PageIndex = Math.Min(condition.PageIndex, condition.TotalPages);
                condition.Start = Math.Max((condition.PageIndex - 1) * condition.PageSize, 0);
                condition.End = Convert.ToInt32(Math.Min(condition.PageIndex * condition.PageSize - 1, condition.TotalRows - 1));
            }
            else
            {
                condition.PageIndex = 0;
                condition.TotalPages = 0;
                condition.Start = -1;
                condition.End = -1;
            }
        }
        #endregion

        #region 03.计算出共分多少页
        /// <summary>
        /// 计算出共分多少页
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private static int GetTotalPages(BaseParameter condition)
        {
            return GetTotalPages(condition.PageSize, condition.TotalRows);
        }
        /// <summary>
        /// 计算出共分多少页
        /// </summary>
        /// <param name="pagesize">每页显示多少行</param>
        /// <param name="totalRows">共有多少行</param>
        /// <returns>计算出共分多少页</returns>
        private static int GetTotalPages(int pagesize, long totalRows)
        {
            if (totalRows <= 0 || pagesize <= 0)
            {
                return 0;
            }
            return Convert.ToInt32((totalRows + pagesize - 1) / pagesize);
        }
        #endregion
    }
}
