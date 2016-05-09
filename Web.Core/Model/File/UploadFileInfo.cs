namespace Portal.Web.Core
{
    /// <summary>
    /// 表示上传的文件信息
    /// </summary>
    public class UploadFileInfo
    {
        #region 属性
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 保存文件名称
        /// </summary>
        public string SaveName { get; set; }

        /// <summary>
        /// 上传文件名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 附件类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 文件内容
        /// </summary>
        public byte[] Content { get; set; }
        #endregion
    }
}
