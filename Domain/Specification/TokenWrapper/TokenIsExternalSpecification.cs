using System;

using EasyDDD.Core.Specification;
namespace Portal.Domain.Specification.TokenWrapper
{
    public class TokenIsExternalSpecification : Specification<Aggregates.TokenWrapperAgg.TokenWrapper>
    {
        private readonly bool _isExternal;
        public TokenIsExternalSpecification(bool isExternal)
        {
            this._isExternal = isExternal;
        }
        public override System.Linq.Expressions.Expression<Func<Aggregates.TokenWrapperAgg.TokenWrapper, bool>> GetExpression()
        {
            return item => item.IsExternal == this._isExternal;
        }
    }
}
