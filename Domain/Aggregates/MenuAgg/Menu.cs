using System;


using Portal.Domain.Aggregates.ApplictionAgg.Events;
using Portal.Domain.Aggregates.ApplictionAgg.Events.Callbacks;
using Portal.Domain.Aggregates.MenuAgg.Events;
using Portal.Domain.Aggregates.MenuAgg.Events.Callbacks;
using Portal.Domain.Aggregates.PermissionAgg.Events;
using Portal.Domain.Aggregates.PermissionAgg.Events.Callbacks;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Aggregate;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Domain.Aggregates.MenuAgg
{
    public class Menu : AggregateRoot
    {
        #region 初始化
        protected Menu()
        {
            this.GenerateNewIdentity();
        }
        public Menu(string applicationId, string parentId, string url, string permissionCode)
            : this()
        {
            SetParentId(applicationId, parentId);
            SetUrl(applicationId, url);
            SetPermissionCode(applicationId, permissionCode);
        }
        #endregion

        #region 属性
        public string Name { get; set; }
        public string Url { get; set; }
        /// <summary>
        /// 是否是绝对地址
        /// </summary>
        public bool IsAbsoluteUrl { get; private set; }
        /// <summary>
        /// 所属应用程序
        /// </summary>
        public string ApplicationId { get; private set; }
        public string ParentId { get; private set; }
        /// <summary>
        /// 打开方式
        /// </summary>
        public int Target { get; set; }
        /// <summary>
        /// 是否共享（所有用户可见）
        /// </summary>
        public bool IsShare { get; set; }
        public string Desc { get; set; }
        public int Sequence { get; set; }
        public string PermissionCode { get; private set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        #endregion

        #region 方法
        public void SetParentId(string applicationId, string parentId)
        {
            if (string.IsNullOrWhiteSpace(parentId))
            {
                this.ParentId = parentId;
                return;
            }

            if (this.ApplicationId == applicationId && this.ParentId == parentId)
            {
                return;
            }

            DomainEvent.Publish<ValidateMenuExistsParentIdEvent, ValidateMenuExistsParentIdEventResult>(new ValidateMenuExistsParentIdEvent(this, applicationId, parentId),
                 e =>
                 {
                     if (e != null)
                     {
                         if (!e.Exists)
                         {
                             throw new PortalException(ErrorCodes.StringCodes.NoFoundMenuId, ErrorMessage.NoFoundMenuId.FormatWith(parentId));
                         }
                         this.ParentId = parentId;
                     }
                 });
        }

        public void SetUrl(string applicationId, string url)
        {
            this.Url = url;
            this.IsAbsoluteUrl = false;
            if (string.IsNullOrEmpty(url))
            {
                return;
            }
            url = url.ToLower();
            if (url.StartsWith("http://") || url.StartsWith("https://"))
            {
                this.IsAbsoluteUrl = true;
            }
        }

        public void SetPermissionCode(string applicationId, string permissionCode)
        {
            SetApplicationId(applicationId);
            if (string.IsNullOrEmpty(permissionCode))
            {
                this.PermissionCode = "";
                return;
            }

            if (this.PermissionCode == permissionCode)
            {
                return;
            }
            DomainEvent.Publish<ValidatePermissionExistsEvent, ValidatePermissionExistsEventResult>(new ValidatePermissionExistsEvent(new[] { permissionCode }),
               e =>
               {
                   if (e != null)
                   {
                       if (!e.Success)
                       {
                           throw new PortalException(ErrorCodes.StringCodes.NoFoundPermissionCode, e.ErrorMessage);
                       }
                       this.PermissionCode = permissionCode;
                   }
               });
        }

        public void SetApplicationId(string applicationId)
        {
            CheckArgument.IsNotNullOrEmpty(applicationId, "applicationId");
            if (this.ApplicationId != applicationId)
            {
                DomainEvent.Publish<ValidateApplicationExistsEvent, ValidateApplicationExistsEventResult>(
                    new ValidateApplicationExistsEvent(applicationId),
                    (e) =>
                    {
                        if (e != null)
                        {
                            if (e.Exists)
                            {
                                this.ApplicationId = applicationId;
                                if (!string.IsNullOrEmpty(this.Id))
                                {
                                    DomainEvent.Publish<MenuApplicationIdChangedEvent>(
                                        new MenuApplicationIdChangedEvent(this, applicationId));
                                }
                            }
                            else
                            {
                                throw new PortalException(ErrorCodes.StringCodes.NoFoundApplication,
                                    ErrorMessage.NoFoundApplication);
                            }
                        }
                    });
            }
        }

        public void SetChildApplicationId(string applicationId)
        {
            this.ApplicationId = applicationId;
        }

        /// <summary>
        /// 是否为一级节点
        /// </summary>
        /// <returns></returns>
        public bool IsFirstParent()
        {
            return string.IsNullOrEmpty(this.ParentId) || this.ParentId == "0";
        }
        #endregion

        #region 私有方法

        #endregion
    }
}
