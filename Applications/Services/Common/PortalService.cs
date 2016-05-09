
using EasyDDD.Core.Aggregate;
using EasyDDD.Core.Repository;
using Portal.Dto;

namespace Portal.Applications.Services
{
    public abstract class PortalService<TDto, TRequest, TDomain> : CommonService<TDto, TRequest, TDomain>
        where TDto : class, new()
        where TDomain : AggregateRoot
        where TRequest : PagerFindRequest
    {
        #region 初始化
        private readonly IRepositoryContext _context;
        public PortalService(IRepositoryContext context, IRepository<TDomain> repository)
            : base(repository)
        {
            this._context = context;
        }
        #endregion

        #region 属性
        protected IRepositoryContext Context
        {
            get { return this._context; }
        }
        #endregion
    }
}
