using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using EasyDDD.Core.Specification;
namespace Portal.Domain.Specification.TokenWrapper
{
    public class RefreshTokenSpecification : Specification<Aggregates.TokenWrapperAgg.TokenWrapper>
    {
        private readonly string _refreshToken;
        public RefreshTokenSpecification(string refreshToken)
        {
            this._refreshToken = refreshToken;
        }
        public override System.Linq.Expressions.Expression<Func<Aggregates.TokenWrapperAgg.TokenWrapper, bool>> GetExpression()
        {
            return item => item.RefreshToken == this._refreshToken;
        }
    }
}
