using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Portal.Domain;
using Portal.Domain.Aggregates.TokenWrapperAgg;
using Portal.Domain.Specification.AuthorizationCode;
using Portal.Domain.Specification.TokenWrapper;
using Portal.Domain.Model;
using Portal.Domain.Repositories;
using Portal.Domain.Services;
using Portal.Dto;
using SortOrder = EasyDDD.Infrastructure.Crosscutting.Paged.SortOrder;
using EasyDDD.Core.Specification;
using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Applications.Services.Impl
{
    /// <summary>
    /// 表示Token管理服务
    /// </summary>
    public class TokenManagerService : ITokenManagerService
    {
        #region 初始化
        private readonly ITokenWrapperRepository _tokenRepository;
        private readonly IRepositoryContext _repositoryContext;
        private readonly IRequstAccessTokenValidateService _ratValidateService;
        private readonly IDirectGetAccessTokenValidateService _dgtValidateService;
        private readonly ICustomerProviderService _customerProviderService;
        private readonly IAuthorizationCodeRepository _authRepository;
        private readonly IValidateTokenValidateService _validateTokenValidateService;
        public TokenManagerService(ITokenWrapperRepository tokenRepository, 
            IRepositoryContext repositoryContext,
            IRequstAccessTokenValidateService ratValidateService, 
            IDirectGetAccessTokenValidateService dgtValidateService,
            ICustomerProviderService customerProviderService,
            IAuthorizationCodeRepository authRepository,
            IValidateTokenValidateService validateTokenValidateService)
        {
            this._tokenRepository = tokenRepository;
            this._repositoryContext = repositoryContext;
            this._ratValidateService = ratValidateService;
            this._dgtValidateService = dgtValidateService;
            this._customerProviderService = customerProviderService;
            this._authRepository = authRepository;
            this._validateTokenValidateService = validateTokenValidateService;
        }

        public FindTokensResponse FindTokens(FindTokensRequest request)
        {
            Check.Argument.IsNotNull(request, "request");
            var order = new Dictionary<Expression<Func<TokenWrapper, object>>, SortOrder>();
            order.Add(item => item.CreatedOn, SortOrder.Descending);
            var result = this._tokenRepository.FindInPage(request.PageIndex, request.PageSize, this.ConvertToSpec(request), order);
            return new FindTokensResponse(result.TotalRecords, Array.ConvertAll(result.Data.ToArray(), item => DtoDomainMapper.ConvertToDto(item)));
        }

        /// <summary>
        /// 表示跳过授权码直接获取Token
        /// </summary>
        /// <returns></returns>
        public TokenWrapperDto DirectGetAccessToken(string clientId, string customerId, string redirectUrl, string scope)
        {
            var result = this._dgtValidateService.Validate(clientId, redirectUrl, scope);
            var newToken = TokenWrapper.New(result.App, customerId, result.ApiPermssionInfos);
            this._tokenRepository.Add(newToken);
            this._repositoryContext.Commit();

            return this.ConvertToDto(newToken);
        }

        /// <summary>
        /// 请求访问Token, 通过授权码或刷新Token方式 
        /// </summary>
        public TokenWrapperDto RequstAccessToken(string clientId, string
            clientSecret, string redirectUrl, string grantType,
            string code = "", string refreshToken = "")
        {
            var result = this._ratValidateService.Validate(clientId, clientSecret, redirectUrl, grantType, code, refreshToken);
            TokenWrapper token;
            if (result.GrantType == GrantType.RefreshToken)
            {
                token = this._tokenRepository.Get(new RefreshTokenSpecification(refreshToken));
                token.Refresh(result.App);
                this._tokenRepository.Update(token);
            }
            else
            {
                token = TokenWrapper.New(result.App, result.CustomerId, result.ApiPermssionInfos);
                this._tokenRepository.Add(token);

                var authCode = this._authRepository.Get(new AuthorizationCodeSpecification(code));
                authCode.UseCode();
                this._authRepository.Update(authCode);
            }
            this._repositoryContext.Commit();

            return this.ConvertToDto(token);
        }

        public TokenValidateResult ValidateToken(string accessToken, string apiPermissionCode)
        {
            this._validateTokenValidateService.Validate(accessToken, apiPermissionCode);

            var token = this._tokenRepository.Get(new AccessTokenSpecification(accessToken));
            var customer = this._customerProviderService.GetCustomerInfoByLoginName(token.CustomerIdentity) ?? CustomerInfo.NullCustomer;
            return new TokenValidateResult(true)
            {
                CustomerNo = customer.CustomerNo,
                CustomerName = customer.CustomerName,
                AppClientId = token.ClientId
            };
        }

        private TokenWrapperDto ConvertToDto(TokenWrapper toke)
        {
            return new TokenWrapperDto()
            {
                AccessToken = toke.AccessToken,
                AccessTokenExpiredTime = toke.AccessTokenExpiredTime,
                ClientId = toke.ClientId,
                DeveloperAppName = toke.DeveloperAppName,
                Codes = toke.ApiPermissionCodes.Select(item => item.Code).ToArray(),
                CreatedOn = toke.CreatedOn,
                CustomerIdentity = toke.CustomerIdentity,
                IsDisabled = toke.IsDisabled,
                IsExternal = toke.IsExternal,
                RefreshToken = toke.RefreshToken,
                RefreshTokenExpiredTime = toke.RefreshTokenExpiredTime
            };
        }
        #endregion

        #region 属性
        protected ISysLoggerManagerService LoggerService { get { return IoC.Resolve<ISysLoggerManagerService>(); } }
        #endregion

        #region 操作
        public void SetEnable(string id, SysLoggerDto logger)
        {
            this.DoAction(id, token => token.SetEnable());
            LoggerService.Create(SysLoggerType.Update, logger, "启用管理访问Token", "启用管理访问Token成功：Token【{1}】", id);
        }

        public void SetDisabled(string id, SysLoggerDto logger)
        {
            this.DoAction(id, token => token.SetDisabled());
            LoggerService.Create(SysLoggerType.Update, logger, "禁用管理访问Token", "禁用管理访问Token成功：Token【{1}】", id);
        }
        #endregion

        #region 私有方法
        private ISpecification<TokenWrapper> ConvertToSpec(FindTokensRequest request)
        {
            var specs = new List<ISpecification<TokenWrapper>>();
            if (!string.IsNullOrEmpty(request.AccessToken))
            {
                specs.Add(new TokenContainAccessTokenSpecification(request.AccessToken));
            }
            if (!string.IsNullOrEmpty(request.ClientId))
            {
                specs.Add(new TokenClientIdSpecification(request.ClientId));
            }
            if (!string.IsNullOrEmpty(request.UserId))
            {
                specs.Add(new TokenCustomerIdentitySpecification(request.UserId));
            }
            if (!string.IsNullOrEmpty(request.UserName))
            {
                specs.Add(new TokenContainCustomerIdentitySpecification(request.UserName));
            }
            if (request.IsExternal.HasValue)
            {
                specs.Add(new TokenIsExternalSpecification(request.IsExternal.Value));
            }
            if (request.StartTime.HasValue)
            {
                specs.Add(new TokenCreateOnStartSpecification(request.StartTime.Value));
            }
            if (request.EndTime.HasValue)
            {
                specs.Add(new TokenCreateOnEndSpecification(request.EndTime.Value));
            }
            if (specs.Count > 0)
            {
                var result = specs[0];
                for (int i = 1; i < specs.Count; i++)
                {
                    result = result.And(specs[i]);
                }
                return result;
            }
            return null;
        }
        private void DoAction(string id, Action<TokenWrapper> action)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("id");
            }
            var token = this._tokenRepository.GetByKey(id);
            action(token);
            this._tokenRepository.Update(token);
            this._repositoryContext.Commit();
        }
        #endregion
    }
}
