using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Dto
{
    /// <summary>
    /// 表示分页查找请求
    /// </summary>
    public abstract class PagerFindRequest
    {
        #region 属性
        private readonly List<OrderField> _orderFields = new List<OrderField>(); 
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 是否去掉分页，为true则不分页，为false则分页。默认分页
        /// </summary>
        public bool NoPage;

        /// <summary>
        /// 是否不是第一次加载
        /// </summary>
        public bool IsPostBack { get; set; }

        /// <summary>
        /// 排序字段列表
        /// </summary>
        public IEnumerable<OrderField> OrderFields 
        {
            get
            {
                return this._orderFields;
            }
        }
        #endregion

        #region 初始化
        public PagerFindRequest()
        {
            PageIndex = 1;
            PageSize = 20;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 设置升序
        /// </summary>
        public void SetAscOrder(string field)
        {
            this.SetOrder(OrderFlag.Asc, field);
        }
        /// <summary>
        /// 设置降序
        /// </summary>
        public void SetDescOrder(string field)
        {
            this.SetOrder(OrderFlag.Asc, field);
        }
        private void SetOrder(OrderFlag flag, string field)
        {
            this._orderFields.Add(new OrderField(){ Flag = flag, Field = field });
        }
        #endregion
    }

    /// <summary>
    /// 排序标志
    /// </summary>
    public enum OrderFlag
    {
        Asc,
        Desc
    }

    /// <summary>
    /// 排序字段信息
    /// </summary>
    public class OrderField
    {
        public OrderFlag Flag { get; set; }
        public string Field { get; set; }
    }
}
