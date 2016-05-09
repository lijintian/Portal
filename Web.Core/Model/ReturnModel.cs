using System;

namespace Portal.Web.Core.Model
{
    [Serializable]
    public class ReturnModel<T>
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
        public ReturnModel()
        { }

        public ReturnModel(T data, string errorMessage, bool status)
        {
            Data = data;
            ErrorMessage = errorMessage;
            Status = status;
        }
        public ReturnModel(string errorMessage, bool status)
        {
            ErrorMessage = errorMessage;
            Status = status;
        }
        public ReturnModel(bool status)
        {
            Status = status;
        }
        #endregion

        #region 方法
        #endregion
    }
}
