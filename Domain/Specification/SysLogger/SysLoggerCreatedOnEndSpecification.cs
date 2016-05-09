/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  Domain层
创建日期：  2016-03-02

内容摘要：  系统日志表Linq表达式
*/
using System;

using EasyDDD.Core.Specification;
using Portal.Domain.Aggregates;

namespace Portal.Domain.Specification
{
    public class SysLoggerCreatedOnEndSpecification : Specification<SysLogger>
    {
        public DateTime End { get; private set; }

        public SysLoggerCreatedOnEndSpecification(DateTime end)
        {
            this.End = end.Date.AddDays(1);
        }

        public override System.Linq.Expressions.Expression<Func<SysLogger, bool>> GetExpression()
        {
            return item => item.CreatedOn < this.End;
        }
    }
}
