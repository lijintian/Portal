using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Dto
{
    public class FlowInfo
    {
        /// <summary>
        /// 步骤
        /// </summary>
        public int StepId { get; set; }
        /// <summary>
        /// 流程环节
        /// </summary>
        public List<FlowDetailInfo> Data { get; set; }
        /// <summary>
        /// 流程环节
        /// </summary>
        public string Json { get; set; }
    }

    public class FlowDetailInfo
    {
        #region 属性
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
        #endregion

        #region 初始化
        public FlowDetailInfo()
        { }

        public FlowDetailInfo(string title, string content)
        {
            this.title = title;
            this.content = content;
        }
        #endregion
    }
}
