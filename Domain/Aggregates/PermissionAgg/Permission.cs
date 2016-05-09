using System;
using System.Linq;



using Portal.Domain.Aggregates.ApplictionAgg.Events;
using Portal.Domain.Aggregates.ApplictionAgg.Events.Callbacks;
using Portal.Domain.Aggregates.PermissionAgg.Events;
using Portal.Domain.Aggregates.PermissionAgg.Events.Callbacks;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Aggregate;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Domain.Aggregates.PermissionAgg
{
    /// <summary>
    /// Represents permission of system
    /// </summary>
    public class Permission : AggregateRoot
    {
        private Permission()
        {
            this.GenerateNewIdentity();
        }

        public Permission(string applicationId, string code, string name, bool isApi = false)
        {
            CheckArgument.IsNotNullOrEmpty(applicationId, "applicationId");
            CheckArgument.IsNotNullOrEmpty(code, "code");
            CheckArgument.IsNotNullOrEmpty(name, "name");
            Validate(applicationId, code, name, isApi);

        }

        #region 属性
        /// <summary>
        /// 所属应用程序
        /// </summary>
        public string ApplicationId { get; private set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 权限附属标志
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 是否兼容老的业务系统
        /// </summary>
        public bool IsCompatible { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Desc { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        #endregion

        #region 方法


        #endregion

        #region 私有方法

        private void Validate(string applicationId, string code, string name, bool isApi)
        {
            DomainEvent.Publish<ValidateApplicationExistsEvent, ValidateApplicationExistsEventResult>(new ValidateApplicationExistsEvent(applicationId),
                (e) =>
                {
                    if (e != null)
                    {
                        if (!e.Exists)
                        {
                            throw new PortalException(ErrorCodes.StringCodes.NoFoundApplication, ErrorMessage.NoFoundApplication);
                        }
                        SetName(applicationId, name, isApi);
                        DomainEvent.Publish<ValidatePermissionExistsEvent, ValidatePermissionExistsEventResult>(new ValidatePermissionExistsEvent(new string[] { code }),
                        (x) =>
                        {
                            if (x != null)
                            {
                                if (x.Success)
                                {
                                    throw new PortalException(ErrorCodes.StringCodes.ExistsSamePermissionCode, ErrorMessage.ExistsSamePermissionCode.FormatWith(code));
                                }
                                else
                                {
                                    this.ApplicationId = applicationId;
                                    this.Code = code;
                                    this.Name = name;
                                }

                            }
                        });
                    }
                });
        }

        /// <summary>
        /// 设置名称
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="name"></param>
        /// <param name="isApi"></param>
        public void SetName(string applicationId, string name, bool isApi=false)
        {
            DomainEvent.Publish<ValidatePermissionExistsSameNameEvent, ValidatePermissionExistsSameNameEventResult>
            (
                new ValidatePermissionExistsSameNameEvent(this, applicationId, name, isApi),
                (e) =>
                {
                    if (e != null)
                    {
                        if (e.Exists)
                        {
                            throw new PortalException(ErrorCodes.StringCodes.ExistsSamePermissionName, ErrorMessage.ExistsSamePermissionName.FormatWith(name));
                        }
                        this.Name = name;
                    }
                }
            );
        }
        #endregion
    }
}
