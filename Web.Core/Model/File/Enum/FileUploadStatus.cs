using System.ComponentModel;

namespace Portal.Web.Core.Model
{
    #region 文件上传状态
    public enum FileUploadStatus
    {

        [Description("上传失败")]
        Fail = 0,

        [Description("SUCCESS")]
        Success = 1,

        [Description("文件格式不正确！")]
        TypeFail = 2,

        [Description("文件大小超出网站限制")]
        SizeFail = 3,

        [Description("未知错误")]
        UnKnowFail = 4,

        [Description("请选择要上传的文件")]
        UnFile = 5
    }
    #endregion
}
