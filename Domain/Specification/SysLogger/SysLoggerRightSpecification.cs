/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  Domain层
创建日期：  2016-03-02

内容摘要：  系统日志表Linq表达式
*/
using System;
using System.Linq;

using EasyDDD.Core.Specification;
using Portal.Domain.Aggregates;

namespace Portal.Domain.Specification
{
    public class SysLoggerRightSpecification : Specification<SysLogger>
    {
		#region 字段
        private readonly SysLoggerRight _right;
		#endregion

		#region 初始化
        public SysLoggerRightSpecification(SysLoggerRight right)
        {
			
            this._right = right;
        }
		
		#endregion

		#region 方法
        public override System.Linq.Expressions.Expression<Func<SysLogger, bool>> GetExpression()
        {
			return item => item.Right == this._right;
        }
		#endregion
    }
}
