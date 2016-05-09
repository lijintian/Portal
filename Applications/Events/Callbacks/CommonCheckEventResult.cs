using System;

using Portal.Web.Core.Model;
using EasyDDD.Core.Event;

namespace Portal.Applications.Events.Callbacks
{
    [Serializable]
    public class CommonCheckEventResult<T> : ReturnModel<T>, IDomainEventResult
    {
        #region  01.字段
        /// <summary>
        /// true成功，false失败
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 数据容器
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 错误类型
        /// </summary>
        public int ErrorType { set; get; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
        #endregion

        #region 初始化
        public CommonCheckEventResult()
        { }
        public CommonCheckEventResult(bool pass)
        {
            this.Status = pass;
        }
        public CommonCheckEventResult(string errorMessage, bool status = false)
        {
            ErrorMessage = errorMessage;
            Status = status;
        }
        public CommonCheckEventResult(T data, string errorMessage, bool status)
        {
            Data = data;
            ErrorMessage = errorMessage;
            Status = status;
        }
        #endregion

        #region 方法
        #endregion
    }
}
