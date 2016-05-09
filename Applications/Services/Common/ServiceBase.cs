

using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;

namespace Portal.Applications.Services.Impl
{
    public class ServiceBase
    {
        #region 字段
        private readonly IRepositoryContext _context;
        #endregion

        #region 属性
        protected ISysLoggerManagerService LoggerService { get { return IoC.Resolve<ISysLoggerManagerService>(); } }
        protected IRepositoryContext Context
        {
            get { return this._context; }
        }
        #endregion

        #region 初始化
        public ServiceBase(IRepositoryContext context)
        {
            this._context = context;
        }
        #endregion
    }
}
