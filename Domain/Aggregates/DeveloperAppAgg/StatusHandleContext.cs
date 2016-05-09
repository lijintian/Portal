using EasyDDD.Infrastructure.Crosscutting.Helpers;
using System;


namespace Portal.Domain.Aggregates.DeveloperAppAgg
{
    public class StatusHandleContext
    {
        #region 属性
        /// <summary>
        /// 表示操作人
        /// </summary>
        public string Manipulator
        {
            get;
            private set;
        }

        /// <summary>
        /// 原因
        /// </summary>
        public string Remark
        {
            get;
            private set;
        }
        #endregion

        #region 初始化
        public StatusHandleContext()
        { }

        public StatusHandleContext(string manipulator, string remark)
        {
            Check.Argument.IsNotNull(manipulator, "manipulator");
            this.Manipulator = manipulator;
            this.Remark = remark;
        }
        #endregion

        #region 方法
        internal string GetMessage(AppState state)
        {
            string message = string.Format("【{0}】于{1}将开发者应用{3}，备注：{2}",
                this.Manipulator, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                this.Remark ?? "无",
                state == AppState.Approved ? "审核成功" : "驳回提审");
            return message;
        }
        #endregion
    }
}
