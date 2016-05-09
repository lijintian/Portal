using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


using Portal.Domain.Aggregates.DeveloperAppAgg;
using Portal.Domain.Aggregates.TokenWrapperAgg.Strategies;
using Portal.Domain.Model;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Aggregate;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;

namespace Portal.Domain.Aggregates.TokenWrapperAgg
{
    /// <summary>
    /// 表示一个包含AccessToken和RefrsehToken信息的包装
    /// </summary>
    public class TokenWrapper : AggregateRoot
    {
        /// <summary>
        /// 表示Access Token
        /// </summary>
        public string AccessToken
        {
            get;
            private set;
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        public string RefreshToken
        {
            get;
            private set;
        }

        /// <summary>
        /// Token类型，外部或内部
        /// </summary>
        public bool IsExternal
        {
            get;
            set;
        }

        /// <summary>
        /// 客户标识
        /// </summary>
        public string CustomerIdentity
        {
            get;
            private set;
        }

        /// <summary>
        /// 表示开发者应用 标识
        /// </summary>
        public string ClientId
        {
            get;
            private set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string DeveloperAppName
        {
            get;
            private set;
        }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime AccessTokenExpiredTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 刷新Token过期时间
        /// </summary>
        public DateTime RefreshTokenExpiredTime
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
        /// 是否禁用
        /// </summary>
        public bool IsDisabled
        {
            get;
            private set;
        }

        private List<ReferenceApiPermssionInfo> _referenceApiPermssionInfos; 
        /// <summary>
        /// 引用的API权限信息
        /// </summary>
        public ReadOnlyCollection<ReferenceApiPermssionInfo> ApiPermissionCodes
        {
            get
            {
                return this._referenceApiPermssionInfos.AsReadOnly();
            }
            private set
            {
                this._referenceApiPermssionInfos = value == null ? new List<ReferenceApiPermssionInfo>() : value.ToList();
            }
        }

        /// <summary>
        /// 表示备注
        /// </summary>
        public string Remark
        {
            get;
            set;
        }

        protected TokenWrapper(string accessToken, string refreshToken)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
            this.IsExternal = true;
            this.CreatedOn = DateTime.UtcNow;
            this.IsDisabled = false;
            this._referenceApiPermssionInfos = new List<ReferenceApiPermssionInfo>();
        }

        /// <summary>
        /// 是否过期
        /// </summary>
        /// <returns></returns>
        public bool IsExpired()
        {
            return DateTime.UtcNow > this.AccessTokenExpiredTime;
        }

        /// <summary>
        /// 设置授权的客户标识
        /// </summary>
        public void SetCustomerIdentity(string identity)
        {
            if (string.IsNullOrWhiteSpace(identity))
            {
                //todo validate identity exist
                this.CustomerIdentity = identity;
            }
        }

        /// <summary>
        /// 设置Token可用
        /// </summary>
        public void SetEnable()
        {
            if (this.IsExpired())
            {
                throw new PortalException(ErrorCodes.StringCodes.TokenExpiredCannotSetEnable, ErrorMessage.TokenExpiredCannotSetEnable);
            }

            this.IsDisabled = false;
        }

        /// <summary>
        /// 禁用Token
        /// </summary>
        public void SetDisabled()
        {
            this.IsDisabled = true;
        }

        /// <summary>
        /// 验证是否允许使用刷新Token以获取新的访问Token
        /// </summary>
        /// <returns></returns>
        public bool IsAllowUseRefreshToken()
        {
            if (this.IsDisabled)
            {
                return false;
            }
            return DateTime.UtcNow < this.RefreshTokenExpiredTime;
        }

        /// <summary>
        /// 设置Token对应的权限码
        /// </summary>
        /// <param name="permissions"></param>
        internal void AddPermissions(params ReferenceApiPermssionInfo[] permissions)
        {
            //todo validate code exists
            this._referenceApiPermssionInfos.AddRange(permissions);
        }

        internal void ResetPermissions(params ReferenceApiPermssionInfo[] permissions)
        {
            this._referenceApiPermssionInfos.Clear();
            this._referenceApiPermssionInfos.AddRange(permissions);
        }

        /// <summary>
        /// 表示重新刷新Token
        /// </summary>
        public void Refresh(DeveloperApp app)
        {
            //todo add refresh event
            var userLevel = app.GetUserLevel();
            var accessTokenStrategy = IoC.Resolve<IAccessTokenValueGenerateStrategy>();
            this.AccessToken = accessTokenStrategy.Generate();
            this.AccessTokenExpiredTime = DateTime.UtcNow.AddMinutes(userLevel.AccessTokenExpiredMinute);
        }

        /// <summary>
        /// 验证Token权限
        /// </summary>
        /// <returns></returns>
        public bool IsAllowedAccess(string permissionCode)
        {
            if (string.IsNullOrEmpty(permissionCode))
            {
                throw new ArgumentException(Resource.PermissionCodeCannotNull);
            }

            //是否禁用 和 过期
            if (this.IsDisabled || this.IsExpired())
            {
                return false;
            }

            //检查授权权限码
            var apiPermssion = this._referenceApiPermssionInfos.FirstOrDefault(item => item.Code == permissionCode);
            return apiPermssion != null;

        }

        /// <summary>
        /// 创建一个新的Token, 本方法应该为内部开发者应用调用
        /// </summary>
        /// <param name="app">开发者应用</param>
        /// <param name="codes">API权限</param>
        /// <returns></returns>
        public static TokenWrapper New(DeveloperApp app, params ReferenceApiPermssionInfo[] codes)
        {
            return TokenWrapper.New(app, string.Empty, codes);
        }

        /// <summary>
        /// 创建一个新的Token,
        /// </summary>
        /// <param name="app">开发者应用</param>
        /// <param name="customerIdentity">客户标识</param>
        /// <param name="codes">API权限</param>
        /// <returns></returns>
        public static TokenWrapper New(DeveloperApp app, string customerIdentity, params ReferenceApiPermssionInfo[] codes)
        {
            if (app == null)
            {
                throw new ArgumentException("app");
            }
            if (codes == null || codes.Length == 0)
            {
                throw new ArgumentException(Resource.ApiPermissionMustNotNull);
            }
            if (codes.ToList().Exists(item => item.IsCustomerGranted) && string.IsNullOrEmpty(customerIdentity))
            {
                throw new ArgumentException(Resource.CreateTokenNotProviderClientIdentity);
            }

            var accessTokenStrategy = IoC.Resolve<IAccessTokenValueGenerateStrategy>();
            var refreshTokenStrategy = IoC.Resolve<IRefreshTokenValueGenerateStrategy>();
            var t = new TokenWrapper(accessTokenStrategy.Generate(), refreshTokenStrategy.Generate());

            var userLevel = app.GetUserLevel();
            t.AccessTokenExpiredTime = t.CreatedOn.AddMinutes(userLevel.AccessTokenExpiredMinute);
            t.RefreshTokenExpiredTime = t.CreatedOn.AddMinutes(userLevel.RefreshTokenExpiredMinute);
            t.IsExternal = app.IsExternal;
            t.ClientId = app.ClientId;
            t.DeveloperAppName = app.Name;
            t.AddPermissions(codes);
            t.CustomerIdentity = customerIdentity;

            return t;
        }
    }
}
