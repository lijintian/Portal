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
    public class SysLoggerCreatedOnStartSpecification : Specification<SysLogger>
    {
        public DateTime Start { get; private set; }

        public SysLoggerCreatedOnStartSpecification(DateTime start)
        {
            this.Start = start.Date;
        }

        public override System.Linq.Expressions.Expression<Func<SysLogger, bool>> GetExpression()
        {
            return item => item.CreatedOn >= this.Start;
        }
    }
}
