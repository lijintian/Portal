/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  Domain层
创建日期：  2016-03-02

内容摘要：  系统日志表信息类
*/
using System;
using System.Web;


using Portal.Domain.Aggregates.ApplictionAgg.Events;
using Portal.Domain.Aggregates.ApplictionAgg.Events.Callbacks;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates
{
    [Serializable]
    public class SysLogger : DomainRoot
    {
        #region 01.属性
        /// <summary>
        /// 应用系统ID
        /// </summary>
        public string ApplicationName { get; private set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string Content { get; private set; }
        /// <summary>
        /// 访问用户IP
        /// </summary>
        public string Ip { get; private set; }
        /// <summary>
        /// 访问的地址
        /// </summary>
        public string Url { get; private set; }
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
        #endregion

        #region 02.初始化
        public SysLogger()
            : base()
        { }
        public SysLogger(string applicationName, string ip, string url, string title, string content, string updateBy)
            : base(updateBy)
        {
            CheckArgument.IsNotNullOrEmpty(updateBy, "updateBy");
            CheckArgument.IsNotNullOrEmpty(title, "title");
            CheckArgument.IsNotNullOrEmpty(content, "content");
            this.ApplicationName = applicationName;
            //this.Validate(applicationId);
            this.Ip = ip;
            this.Url = url;
            this.Title = title;
            this.Content = content;
        }
        #endregion

        #region 03.方法

        #endregion

        #region 04.私有方法
        private void Validate(string applicationId)
        {
            DomainEvent.Publish<ValidateApplicationExistsEvent, ValidateApplicationExistsEventResult>(new ValidateApplicationExistsEvent(applicationId),
                (e) =>
                {
                    if (e != null)
                    {
                        if (e.Exists)
                        {
                            //this.ApplicationName = e.Name;
                        }
                        else
                        {
                            throw new PortalException(ErrorCodes.StringCodes.NoFoundApplication, ErrorMessage.NoFoundApplication);
                        }
                    }
                });
        }
        #endregion
    }

}