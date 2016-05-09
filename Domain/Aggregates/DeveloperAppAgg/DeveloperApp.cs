using System;
using System.Collections.Generic;
using System.Linq;




using Portal.Domain.Aggregates.ApiPermissionGroupAgg.Events;
using Portal.Domain.Aggregates.ApiPermissionGroupAgg.Events.Callbacks;
using Portal.Domain.Aggregates.DeveloperAppAgg.Events;
using Portal.Domain.Aggregates.DeveloperAppAgg.Events.Callbacks;
using Portal.Domain.Aggregates.DeveloperAppAgg.Strategies;
using Portal.Domain.Model;
using Portal.Infrastructure.Config;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Aggregate;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Domain.Aggregates.DeveloperAppAgg
{

    /// <summary>
    /// 表示开发者应用
    /// </summary>
    public class DeveloperApp : AggregateRoot
    {
        #region 初始化
        /// <summary>
        /// 实例化新的开发者应用, 并默认指定为外部开发应用
        /// </summary>
        /// <param name="userId">所属账号id</param>
        /// <param name="name">应用名称</param>
        /// <param name="callbackUrl">回调地址</param>
        public DeveloperApp(string userId, string name, string callbackUrl)
            : this(userId, name, callbackUrl, true)
        {

        }

        /// <summary>
        /// 实例化新的开发者应用
        /// </summary>
        /// <param name="userId">所属账号id</param>
        /// <param name="name">应用名称</param>
        /// <param name="callbackUrl">回调地址</param>
        /// <param name="isExternal">是否外部开发者</param>
        public DeveloperApp(string userId, string name, string callbackUrl, bool isExternal)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(Resource.DeveloperApplicatinUserCannotBeNull);
            }
            if (!this.IsValidCallbackUrl(callbackUrl))
            {
                throw new ArgumentException(Resource.CallbackUrlFormatError);
            }
            this.UserId = userId;
            Set(name, callbackUrl);
            this.AccessUrl = string.Empty;
            this.Desc = string.Empty;
            this.AppType = ApplicationType.Web;
            this.State = isExternal ? AppState.Developing : AppState.Approved;
            this.CreatedOn = DateTime.Now;
            this.IsExternal = isExternal;
            this.IsDeleted = false;
            this._requestList = new List<ReferenceApiPermssionInfo>();
            this._approvedList = new List<ReferenceApiPermssionInfo>();
            this.GenreateAuthenticateTicket();
        }
        #endregion

        #region 属性
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc
        {
            get;
            set;
        }

        /// <summary>
        /// 应用访问地址
        /// </summary>
        public string AccessUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 应用回调地址
        /// </summary>
        public string CallbackUrl
        {
            get;
            private set;
        }

        /// <summary>
        /// 应用类型
        /// </summary>
        public ApplicationType AppType
        {
            get;
            set;
        }

        /// <summary>
        /// 标识是否外部App
        /// </summary>
        public bool IsExternal
        {
            get;
            set;
        }

        /// <summary>
        /// 应用所属开发者账号Id
        /// </summary>
        public string UserId
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn
        {
            get;
            private set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 应用状态
        /// </summary>
        public AppState State
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsDeleted { get; private set; }

        private List<string> _requestGroupList;

        /// <summary>
        /// 请求审核API权限分组列表
        /// </summary>
        public IReadOnlyList<string> RequestGroupList
        {
            get
            {
                return this._requestGroupList == null ? null : this._requestGroupList.AsReadOnly();
            }
            private set
            {
                _requestGroupList = value == null ? new List<string>() : value.ToList();
            }
        }

        private List<ReferenceApiPermssionInfo> _requestList;
        /// <summary>
        /// 请求审核权限列表
        /// </summary>
        public IReadOnlyList<ReferenceApiPermssionInfo> RequestList
        {
            get
            {
                return this._requestList.AsReadOnly();
            }
            private set
            {
                _requestList = value == null ? new List<ReferenceApiPermssionInfo>() : value.ToList();
            }
        }

        private List<string> _approvedGroupList;
        /// <summary>
        /// 审核通过的API权限分组列表
        /// </summary>
        public IReadOnlyList<string> ApprovedGroupList
        {
            get
            {
                return this._approvedGroupList == null ? null : this._approvedGroupList.AsReadOnly();
            }
            private set
            {
                _approvedGroupList = value == null ? new List<string>() : value.ToList();
            }
        }

        private List<ReferenceApiPermssionInfo> _approvedList;
        /// <summary>
        /// 审核通过的权限列表
        /// </summary>
        public IReadOnlyList<ReferenceApiPermssionInfo> ApprovedList
        {
            get
            {
                return this._approvedList.AsReadOnly();
            }
            private set
            {
                _approvedList = value == null ? new List<ReferenceApiPermssionInfo>() : value.ToList();
            }
        }


        /// <summary>
        /// 应用标识Id
        /// </summary>
        public string ClientId
        {
            get;
            private set;
        }

        /// <summary>
        /// 应用密码
        /// </summary>
        public string Secret
        {
            get;
            private set;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 设置名称和回调URL
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callbackUrl"></param>
        /// <param name="checkState">检查状态是否在开发中</param>
        public void Set(string name, string callbackUrl,bool checkState=false)
        {
            if (checkState && this.State != AppState.Developing)
            {
                throw new PortalException(ErrorCodes.StringCodes.DeveloperApplicatinInvalidStateCannotUpdate, ErrorMessage.DeveloperApplicatinInvalidStateCannotUpdate);
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(Resource.DeveloperApplicatinNameCannotBeNull);
            }
            if (string.IsNullOrEmpty(callbackUrl))
            {
                throw new ArgumentException(Resource.DeveloperApplicatinCallbackUrlCannotBeNull);
            }
            if (!this.IsValidCallbackUrl(callbackUrl))
            {
                throw new ArgumentException(Resource.CallbackUrlFormatError);
            }
            if (this.Name != name)
            {
                DomainEvent.Publish<ValidateDeveloperAppExistsSameNameEvent, ValidateDeveloperAppExistsSameNameEventResult>
                    (
                        new ValidateDeveloperAppExistsSameNameEvent(this, name),
                        (e) =>
                        {
                            if (e != null)
                            {
                                if (e.Exists)
                                {
                                    throw new PortalException(ErrorCodes.StringCodes.ExistsSameDeveloperAppName, ErrorMessage.ExistsSameDeveloperAppName.FormatWith(name));
                                }
                                this.Name = name;
                            }
                        }
                    );
            }
            this.CallbackUrl = callbackUrl;
        }

        public void Delete()
        {
            this.IsDeleted = true;
            this.State = AppState.Disable;
        }

        public void AddGroupRequestPermissions(string codes)
        {
            CheckArgument.IsNotNullOrEmpty(codes, "codes");
            DomainEvent.Publish<ValidateApiPermissionGroupExistsEvent, ValidateApiPermissionGroupExistsEventResult>
           (
               new ValidateApiPermissionGroupExistsEvent(codes),
               (e) =>
               {
                   if (e != null)
                   {
                       if (!e.Success)
                       {
                           string code = string.Join(",", e.NoFoundApiPermissionGroupCodes);
                           throw new PortalException(ErrorCodes.StringCodes.NoFoundApiPermissionGroupCode, ErrorMessage.NoFoundApiPermissionGroupCode.FormatWith(code));
                       }
                       if (_requestGroupList == null)
                       {
                           _requestGroupList = new List<string>();
                       }
                       else
                       {
                           this._requestGroupList.Clear();
                       }
                       this._requestGroupList.AddRange(e.GroupCode);
                       AddRequestPermissions(e.PermissionCodes);
                   }
               }
           );
        }


        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsEnable()
        {
            return this.State == AppState.Approved;
        }

        /// <summary>
        /// 判断能否提交API权限信息
        /// </summary>
        public void CanSumit()
        {
            if (this.State != AppState.Developing)
            {
                throw new PortalException(ErrorCodes.StringCodes.DeveloperApplicatinInvalidStateCannotUpdatePermission, ErrorMessage.DeveloperApplicatinInvalidStateCannotUpdatePermission);
            }
        }

        /// <summary>
        /// 提交审核 
        /// </summary>
        public void SubmitToApprove()
        {
            if (this.RequestGroupList == null || !this.RequestGroupList.Any())
            {
                throw new PortalException(ErrorCodes.StringCodes.DeveloperApplicatinUnGroupCannotSubmitToApprove, ErrorMessage.DeveloperApplicatinUnGroupCannotSubmitToApprove);
            }
            if (this.State == AppState.Developing)
            {
                this.State = AppState.Verifying;
                this.Remarks = string.Format("{0}提交审核", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                throw new PortalException(ErrorCodes.StringCodes.DeveloperApplicatinInvalidStateCannotSubmitToApprove, ErrorMessage.DeveloperApplicatinInvalidStateCannotSubmitToApprove);
            }
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        public void Approve(StatusHandleContext context)
        {
            if (this.State == AppState.Verifying || this.State == AppState.Approved)
            {
                this.State = AppState.Approved;
                this._approvedGroupList.Clear();
                this._approvedGroupList.AddRange(this._requestGroupList);
                this._approvedList.Clear();
                this._approvedList.AddRange(this._requestList);
                this._requestGroupList.Clear();
                this._requestList.Clear();
                this.Remarks = string.Format("{0}{1}{2}", this.Remarks, Environment.NewLine, context.GetMessage(this.State));
            }
            else
            {
                throw new PortalException(ErrorCodes.StringCodes.DeveloperApplicatinInvalidStateCannotBeApproved, ErrorMessage.DeveloperApplicatinInvalidStateCannotBeApproved);
            }
        }

        /// <summary>
        /// 驳回
        /// </summary>
        public void Reject(StatusHandleContext context)
        {
            if (this.State == AppState.Verifying)
            {
                this.State = AppState.Developing;
                this.Remarks = string.Format("{0}{1}{2}", this.Remarks, Environment.NewLine, context.GetMessage(this.State));
            }
            else
            {
                throw new PortalException(ErrorCodes.StringCodes.DeveloperApplicatinInvalidStateToBeReject, ErrorMessage.DeveloperApplicatinInvalidStateToBeReject);
            }
        }

        /// <summary>
        /// 获取用户级别
        /// </summary>
        /// <returns></returns>
        public UserLevel GetUserLevel()
        {
            //外部token默认一级用户， 内部默认三级用户
            return this.IsExternal ? UserLevel.First : UserLevel.Three;
        }

        internal void AddApprovedPermissions(IEnumerable<ReferenceApiPermssionInfo> permssions)
        {
            this._approvedList.Clear();
            this._approvedList.AddRange(permssions);
        }

        internal void ClearApprovedPermissions()
        {
            this._approvedList.Clear();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 添加请求审核权限列表
        /// </summary>
        /// <param name="permssions">请求的权限列表</param>
        private void AddRequestPermissions(IEnumerable<ReferenceApiPermssionInfo> permssions)
        {
            this._requestList.Clear();
            this._requestList.AddRange(permssions);
        }

        private void GenreateAuthenticateTicket()
        {
            var strategy = IoC.Resolve<IAuthenticateTicketGenerateStategy>();
            this.ClientId = strategy.GenerateClientId();
            this.Secret = strategy.GenreateSecret();
        }

        private bool IsValidCallbackUrl(string url)
        {
            try
            {
                var uri = new Uri(url);
                if (AppConfig.CallbackUrlSecureHttps)
                {
                    return uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    return uri.Scheme.Equals(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
                           || uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase);
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
