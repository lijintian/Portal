/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  Domain层
创建日期：  2016-03-02

内容摘要：  系统日志表Linq表达式
*/
using System;


using Portal.Domain.Aggregates;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Specification;

namespace Portal.Domain.Specification
{
    public class SysLoggerContainCreatedBySpecification : Specification<SysLogger>
    {
        #region 字段
        private readonly string _createdBy;
        #endregion

        #region 初始化
        public SysLoggerContainCreatedBySpecification(string createdBy)
        {
            CheckArgument.IsNotNullOrEmpty(createdBy, "createdBy");
            this._createdBy = createdBy.ToLower();
        }

        #endregion

        #region 方法
        public override System.Linq.Expressions.Expression<Func<SysLogger, bool>> GetExpression()
        {
            return item => item.CreatedBy.ToLower().Contains(this._createdBy);
        }
        #endregion
    }
}
