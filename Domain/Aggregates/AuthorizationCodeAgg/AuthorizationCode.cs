using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Portal.Domain.Aggregates.AuthorizationCodeAgg.Strategies;
using Portal.Domain.Aggregates.DeveloperAppAgg;
using Portal.Domain.Model;
using Portal.Infrastructure.Config;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Aggregate;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;

namespace Portal.Domain.Aggregates.AuthorizationCodeAgg
{
    /// <summary>
    /// 表示授权码对象
    /// </summary>
    public class AuthorizationCode : AggregateRoot
    {
        /// <summary>
        /// 授权码
        /// </summary>
        public string Code
        {
            get;
            private set;
        }

        private List<ReferenceApiPermssionInfo> _referenceApiPermssionInfos;
        /// <summary>
        /// 授码码列表
        /// </summary>
        public IReadOnlyList<ReferenceApiPermssionInfo> Permssions
        {
            get
            {
                return this._referenceApiPermssionInfos.AsReadOnly();
            }
            private set
            {
                this._referenceApiPermssionInfos = value == null
                    ? new List<ReferenceApiPermssionInfo>()
                    : value.ToList();
            }
        }

        /// <summary>
        /// 授权客户
        /// </summary>
        public string CustomerIdentity
        {
            get;
            private set;
        }

        /// <summary>
        /// 开发者应用标识
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
        /// 授权时间
        /// </summary>
        public DateTime AuthorizationTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiredTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 授权码是否使用
        /// </summary>
        public bool IsUsed
        {
            get;
            private set;
        }

        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime UsedTime
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

        /// <summary>
        /// 授权码是否有效
        /// </summary>
        /// <returns></returns>
        public bool IsInValid()
        {
            return this.IsExpired() || this.IsDisabled || this.IsUsed;
        }

        /// <summary>
        /// 启用授权码
        /// </summary>
        public void SetEnable()
        {
            if (this.IsExpired() || this.IsUsed)
            {
                throw new PortalException(ErrorCodes.StringCodes.AuthorizationCodeCannotSetEnable, ErrorMessage.AuthorizationCodeCannotSetEnable);
            }
            this.IsDisabled = false;
        }

        /// <summary>
        /// 禁用授权码
        /// </summary>
        public void SetDisable()
        {
            this.IsDisabled = true;
        }

        /// <summary>
        /// 使用授权码
        /// </summary>
        public void UseCode()
        {
            this.IsUsed = true;
            this.UsedTime = DateTime.Now;
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow > this.ExpiredTime;
        }

        /// <summary>
        /// 实例化一个新的授权码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="clientId"></param>
        /// <param name="customerIdentity"></param>
        /// <param name="permssions"></param>
        protected AuthorizationCode(string code, string clientId, string appName, string customerIdentity, params ReferenceApiPermssionInfo[] permssions)
        {
            this.Code = code;
            this._referenceApiPermssionInfos = new List<ReferenceApiPermssionInfo>();
            this._referenceApiPermssionInfos.AddRange(permssions);
            this.IsUsed = false;
            this.IsDisabled = false;
            this.AuthorizationTime = DateTime.UtcNow;
            this.ClientId = clientId;
            this.DeveloperAppName = appName;
            this.CustomerIdentity = customerIdentity;
        }

        /// <summary>
        /// 产生新的授权码
        /// </summary>
        /// <param name="app">开发者应用</param>
        /// <param name="customerIdentity">授权客户标识</param>
        /// <param name="permssions">权限列表</param>
        /// <returns></returns>
        public static AuthorizationCode New(DeveloperApp app, string customerIdentity, params ReferenceApiPermssionInfo[] permssions)
        {
            if (app == null)
            {
                throw new ArgumentException("app");
            }
            if (string.IsNullOrEmpty(customerIdentity))
            {
                throw new ArgumentException(Resource.CustomerIdentityCannotBeNull);
            }
            if (permssions == null || permssions.Length == 0)
            {
                throw new ArgumentException(Resource.PermissionCodeCannotNull);
            }

            var codeStrategy = IoC.Resolve<ICodeGenreateStrategy>();
            var authCode = new AuthorizationCode(codeStrategy.Generate(), app.ClientId, app.Name, customerIdentity, permssions);
            authCode.ExpiredTime = authCode.AuthorizationTime.AddMinutes(AppConfig.AuthorizationCodeExpiredTime);
            return authCode;
        }
    }
}
