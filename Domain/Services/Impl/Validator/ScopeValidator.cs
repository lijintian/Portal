using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.DeveloperApp;
using Portal.Domain.Specification.Permission;
using Portal.Infrastructure.Exceptions;

namespace Portal.Domain.Services.Impl.Validator
{
    /// <summary>
    /// 表示授权范围验证
    /// </summary>
    class ScopeValidator : Validator
    {
        private readonly string _scope;
        private readonly string[] _appCodes;

        public ScopeValidator(string scope, string[] appCodes)
        {
            this._scope = scope;
            this._appCodes = appCodes;
        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(this._scope))
            {
                throw new PortalException(ErrorCodes.StringCodes.CustomerAuthorizationScopeMissing, ErrorMessage.CustomerAuthorizationScopeMissing);
            }
            var codes = this._scope.Split(',');
            if (codes.Length == 0)
            {
                throw new PortalException(ErrorCodes.StringCodes.CustomerAuthorizationScopeMustHasVal, ErrorMessage.CustomerAuthorizationScopeMustHasVal);
            }

            var matchCount = this._appCodes.Count(item => codes.Contains(item));
            if (matchCount != codes.Length)
            {
                throw new PortalException(ErrorCodes.StringCodes.CustomerAuthorizationScopeNotMatch, ErrorMessage.CustomerAuthorizationScopeNotMatch);
            }
        }
    }
}
