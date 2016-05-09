using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Aggregates.AuthorizationCodeAgg;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.AuthorizationCode;
using Portal.Dto;
using SortOrder = EasyDDD.Infrastructure.Crosscutting.Paged.SortOrder;
using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using EasyDDD.Core.Specification;

namespace Portal.Applications.Services.Impl
{
    /// <summary>
    /// 表示授权码管理服务实现
    /// </summary>
    public class AuthorizationCodeManagerService : IAuthorizationCodeManagerService
    {
        #region 初始化
        private readonly IAuthorizationCodeRepository _authRepository;
        private readonly IRepositoryContext _repositoryContext;
        public AuthorizationCodeManagerService(IAuthorizationCodeRepository authRepository,
            IRepositoryContext repositoryContext)
        {
            this._authRepository = authRepository;
            this._repositoryContext = repositoryContext;
        }
        #endregion

        public FindAuthorizationCodesResponse FindTokens(FindAuthorizationCodesRequest request)
        {
            Check.Argument.IsNotNull(request, "request");
            var order = new Dictionary<Expression<Func<AuthorizationCode, object>>, SortOrder>();
            order.Add(item => item.Code, SortOrder.Ascending);
            var result = this._authRepository.FindInPage(request.PageIndex, request.PageSize, this.ConvertToSpec(request), order);
            return new FindAuthorizationCodesResponse(result.TotalRecords, Array.ConvertAll(result.Data.ToArray(), item => DtoDomainMapper.ConvertToDto(item)));
        }

        #region 私有方法
        private ISpecification<AuthorizationCode> ConvertToSpec(FindAuthorizationCodesRequest request)
        {
            var specs = new List<ISpecification<AuthorizationCode>>();
            if (!string.IsNullOrEmpty(request.ClientId))
            {
                specs.Add(new AuthorizationCodeClientIdSpecification(request.ClientId));
            }
            if (!string.IsNullOrEmpty(request.Code))
            {
                specs.Add(new AuthorizationCodeContainCodeSpecification(request.Code));
            }
            if (!string.IsNullOrEmpty(request.UserId))
            {
                specs.Add(new AuthorizationCodeCustomerIdentitySpecification(request.UserId));
            }
            if (!string.IsNullOrEmpty(request.UserName))
            {
                specs.Add(new AuthorizationCodeContainCustomerIdentitySpecification(request.UserName));
            }
            if (request.StartTime.HasValue)
            {
                specs.Add(new AuthorizationCodeAuthorizationTimeStartSpecification(request.StartTime.Value));
            }
            if (request.EndTime.HasValue)
            {
                specs.Add(new AuthorizationCodeAuthorizationTimeEndSpecification(request.EndTime.Value));
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
        #endregion
    }
}
