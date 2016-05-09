using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

using Portal.Domain.Aggregates.AuthorizationCodeAgg;
using Portal.Domain.Aggregates.TokenWrapperAgg;
using Portal.Domain.Model;
using Portal.Domain.Repositories;
using Portal.Domain.Services;
using Portal.Dto;
using Portal.Dto.Request;
using Portal.Dto.Response;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Repository;

namespace Portal.Applications.Services.Impl
{
    /// <summary>
    /// 表示Oauth授权服务实现
    /// </summary>
    public class CustomerAuthorizationManagerService : ICustomerAuthorizationManagerService
    {
        private readonly ICustomerAuthorizationValidateService _validateService;
        private readonly ITokenWrapperRepository _tokenWrapperRepository;
        private readonly IAuthorizationCodeRepository _codeRepository;
        private readonly IRepositoryContext _repositoryContext;

        public CustomerAuthorizationManagerService(ICustomerAuthorizationValidateService validateService, 
            ITokenWrapperRepository tokenWrapperRepository, 
            IAuthorizationCodeRepository codeRepository,
            IRepositoryContext context)
        {
            this._validateService = validateService;
            this._tokenWrapperRepository = tokenWrapperRepository;
            this._codeRepository = codeRepository;
            this._repositoryContext = context;
        }

        public AuthorizationRequestValidateResult RequestValidate(AuthorizationRequest request)
        {
            var result = new AuthorizationRequestValidateResult();
            try
            {
                this._validateService.Validate(request.ResponseType, request.RedirectUri,
                    request.ClientId, request.Scope);
            }
            catch (PortalException ex)
            {
                result.IsOk = false;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 请求授权
        /// </summary>
        public AuthorizationResponse RequestAuthorization(AuthorizationRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("request");
            }

            var validateResult = this._validateService.Validate(request.ResponseType, request.RedirectUri,
                request.ClientId, request.Scope);

            string codeOrToken;
            if (validateResult.ResponseType == ResponseType.Code)
            {
                var code = AuthorizationCode.New(validateResult.App, request.CustomerId, validateResult.RefApiPermssionInfos);
                this._codeRepository.Add(code);
                codeOrToken = code.Code;
            }
            else 
            {
                var token = TokenWrapper.New(validateResult.App, request.CustomerId, validateResult.RefApiPermssionInfos);
                this._tokenWrapperRepository.Add(token);
                codeOrToken = token.AccessToken;
            }
            this._repositoryContext.Commit();

            return new AuthorizationResponse()
            {
                IsUseAuthorizationCode = validateResult.ResponseType == ResponseType.Code,
                CodeOrToken = codeOrToken,
            };
        }

    }
}
