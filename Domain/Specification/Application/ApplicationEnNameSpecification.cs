using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.Application
{
    public class ApplicationEnNameSpecification : Specification<Aggregates.ApplictionAgg.Application>
    {
        private readonly string _enName;
        public ApplicationEnNameSpecification(string enName)
        {
            _enName = enName;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.ApplictionAgg.Application, bool>> GetExpression()
        {
            return c => c.EnName == _enName;
        }
    }
}
