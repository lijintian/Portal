using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.Application
{
    public class ApplicationNameSpecification : Specification<Aggregates.ApplictionAgg.Application>
    {
        private readonly string _name;
        public ApplicationNameSpecification(string name)
        {
            _name = name;
        }

        public override System.Linq.Expressions.Expression<Func<Aggregates.ApplictionAgg.Application, bool>> GetExpression()
        {
            return c => c.Name == _name;
        }
    }
}
