using System.ComponentModel;

namespace Portal.Web.Core.Model
{
    /// <summary>
    /// 树类型枚举
    /// </summary>
    public enum TreeType
    {
        [Description("一般的树形结构")]
        Common = 1,

        [Description("多选树")]
        CheckBox = 2,

        [Description("单选树")]
        RadioButton = 3
    }
}
