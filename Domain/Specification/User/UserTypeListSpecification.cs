using System;
using System.Collections.Generic;

using Portal.Domain.Aggregates.UserAgg;
using EasyDDD.Core.Specification;
namespace Portal.Domain.Specification.User
{
    public class UserTypeListSpecification : Specification<Aggregates.UserAgg.User>
    {
        #region 属性
        private readonly List<UserType> _type;
        #endregion

        #region 初始化
        public UserTypeListSpecification(List<UserType> types)
        {
            this._type = new List<UserType>();
            this._type = types;
        }

        public UserTypeListSpecification(bool isApi)
        {
            if (isApi)
            {
                this._type = new List<UserType> { UserType.InternalApi, UserType.ExternalApi };
            }
        }
        #endregion

        public override System.Linq.Expressions.Expression<Func<Aggregates.UserAgg.User, bool>> GetExpression()
        {
            return item => this._type.Contains(item.UserType);
        }
    }
}
