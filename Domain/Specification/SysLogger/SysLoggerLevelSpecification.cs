/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  Domain层
创建日期：  2016-03-02

内容摘要：  系统日志表Linq表达式
*/
using System;
using System.Linq;


using Portal.Domain.Aggregates;
using EasyDDD.Core.Specification;
namespace Portal.Domain.Specification
{
    public class SysLoggerLevelSpecification : Specification<SysLogger>
    {
		#region 字段
        private readonly SysLoggerLevel _level;
		#endregion

		#region 初始化
        public SysLoggerLevelSpecification(SysLoggerLevel level)
        {
			
            this._level = level;
        }
		
		#endregion

		#region 方法
        public override System.Linq.Expressions.Expression<Func<SysLogger, bool>> GetExpression()
        {
			return item => item.Level == this._level;
        }
		#endregion
    }
}
