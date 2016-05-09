using System;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification.User
{
    public class DisplayNameSpecification : Specification<Aggregates.UserAgg.User>
    {
        #region 字段
        private readonly string _displayName;
        #endregion

        #region 初始化
        public DisplayNameSpecification(string displayName)
        {
            _displayName = displayName;
        }
        #endregion

        #region 方法
        public override System.Linq.Expressions.Expression<Func<Aggregates.UserAgg.User, bool>> GetExpression()
        {
            return c => c.DisplayName == _displayName;
        }
        #endregion
    }
}
