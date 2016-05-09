/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  Domain层
创建日期：  2016-03-02

内容摘要：  系统日志表资源层接口
*/

using EasyDDD.Core.Repository;
using Portal.Domain.Aggregates;

namespace Portal.Domain.Repositories
{
    public interface ISysLoggerRepository : IRepository<SysLogger>
    {

    }
}
