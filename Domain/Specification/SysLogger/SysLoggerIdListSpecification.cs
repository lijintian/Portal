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
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Specification;
namespace Portal.Domain.Specification
{
    public class SysLoggerIdListSpecification : Specification<SysLogger>
    {
        #region 字段
        private readonly string[] _list;
        #endregion

        #region 初始化
        public SysLoggerIdListSpecification(string[] list)
        {
            CheckArgument.IsNotNullOrEmpty(list, "list");
            this._list = list;
        }
        public SysLoggerIdListSpecification(string ids)
        {
            CheckArgument.IsNotNullOrEmpty(ids, "ids");
            this._list = ids.Split(',');
        }

        #endregion

        #region 方法
        public override System.Linq.Expressions.Expression<Func<SysLogger, bool>> GetExpression()
        {
            return item => this._list.Contains(item.Id);
        }
        #endregion
    }
}
