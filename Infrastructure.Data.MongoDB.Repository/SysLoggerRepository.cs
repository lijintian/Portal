/*
系统名称：  Portal门户系统管理平台
模块名称：  资源层
创建日期：  2016-03-02

内容摘要：  系统日志表资源类
*/


using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Data.MongoDB;
using Portal.Domain.Aggregates;
using Portal.Domain.Repositories;


namespace Portal.Infrastructure.Data.MongoDB.Repository
{
    public class SysLoggerRepository : MongoDBRepository<SysLogger>, ISysLoggerRepository
    {
        public SysLoggerRepository(IRepositoryContext context)
            : base(context)
        {
        }
    }
}
