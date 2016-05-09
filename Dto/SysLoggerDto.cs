/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  Dto层
创建日期：  2016-03-02

内容摘要：  系统日志表信息类
*/
using System;
using System.ComponentModel;

namespace Portal.Dto
{
    [Serializable]
    public class SysLoggerDto : DomainDto
    {
        #region 01.属性
        /// <summary>
        /// 应用系统名称
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 访问用户IP
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 访问的地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 客户端使用的浏览器名称和版本号
        /// </summary>
        public string Browser { get; set; }
        /// <summary>
        /// 客户端使用的系统
        /// </summary>
        public string OS { get; set; }
        /// <summary>
        /// 重要等级: 1Debug异常, 2Info提示, 3Warning警告, 4Error错误, 5Critical严重的
        /// </summary>
        public SysLoggerLevel Level { get; set; }
        /// <summary>
        /// 日志类型：1增, 2改, 3删, 4查, 5登录, 6其它
        /// </summary>
        public SysLoggerType Type { get; set; }
        /// <summary>
        /// 查看权限: 所有人可见(包括客户), 仅内部帐号可见，仅内部管理员可见
        /// </summary>
        public SysLoggerRight Right { get; set; }

        /// <summary>
        /// 是否是创建
        /// </summary>
        public bool IsCreate { get; set; }
        #endregion

        #region 02.初始化
        public SysLoggerDto()
            : base()
        { }
        public SysLoggerDto(string title, string content, SysLoggerType type,
            SysLoggerLevel level = SysLoggerLevel.Info, SysLoggerRight right = SysLoggerRight.All)
        {
            InitInfo(title, content, type, level, right);
        }
        #endregion

        #region 03.方法
        public void InitInfo(string title, string content, SysLoggerType type,
          SysLoggerLevel level = SysLoggerLevel.Info, SysLoggerRight right = SysLoggerRight.All)
        {
            this.Title = title;
            this.Content = content;
            this.Type = type;
            this.Level = level;
            this.Right = right;
        }
        #endregion
    }

    #region 枚举
    /// <summary>
    /// 重要等级：1Debug异常, 2Info提示, 3Warning警告, 4Error错误, 5Critical严重的
    /// </summary>
    public enum SysLoggerLevel
    {
        /// <summary>
        /// 异常
        /// </summary>
        [Description("异常")]
        Debug,
        /// <summary>
        /// 提示
        /// </summary>
        [Description("提示")]
        Info,
        /// <summary>
        /// 警告
        /// </summary>
        [Description("警告")]
        Warning,
        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        Error,
        /// <summary>
        /// 严重
        /// </summary>
        [Description("严重")]
        Critical,
    }

    /// <summary>
    /// 日志类型：1增, 2改, 3删, 4查, 5登录, 6其它
    /// </summary>
    public enum SysLoggerType
    {
        /// <summary>
        /// 新增
        /// </summary>
        [Description("新增")]
        Create,

        /// <summary>
        /// 修改
        /// </summary>
        [Description("修改")]
        Update,

        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete,

        /// <summary>
        /// 查询
        /// </summary>
        [Description("查询")]
        Search,

        /// <summary>
        /// 登陆
        /// </summary>
        [Description("登陆")]
        Login,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other,
    }

    /// <summary>
    /// 查看权限: 所有人可见(包括客户), 仅内部帐号可见，仅内部管理员可见
    /// </summary>
    public enum SysLoggerRight
    {
        /// <summary>
        /// 所有人可见(包括客户)
        /// </summary>
        [Description("所有人可见")]
        All,

        /// <summary>
        /// 仅内部帐号可见
        /// </summary>
        [Description("仅内部帐号可见")]
        Employee,

        /// <summary>
        /// 仅内部管理员可见
        /// </summary>
        [Description("仅内部管理员可见")]
        Admin,
    }
    #endregion
}