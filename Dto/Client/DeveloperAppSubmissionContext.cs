namespace Portal.Dto.Client
{
    public class DeveloperAppSubmissionContext
    {
        #region 属性
        /// <summary>
        /// 开发者应用Id
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// 表示操作人
        /// </summary>
        public string Manipulator
        {
            get;
            set;
        }

        /// <summary>
        /// 原因
        /// </summary>
        public string Remark
        {
            get;
            set;
        }
        #endregion
    }
}
