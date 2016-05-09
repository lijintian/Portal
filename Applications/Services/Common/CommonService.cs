using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Portal.Dto;
using SortOrder = EasyDDD.Infrastructure.Crosscutting.Paged.SortOrder;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;
using EasyDDD.Core.Specification;
using EasyDDD.Core.Aggregate;
using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using EasyDDD.Infrastructure.Crosscutting.Paged;

namespace Portal.Applications.Services
{
    public abstract class CommonService<TDto, TRequest, TDomain>
        where TDto : class, new()
        where TDomain : AggregateRoot
        where TRequest : PagerFindRequest
    {
        #region 初始化
        protected readonly IRepository<TDomain> _repository;
        public CommonService(IRepository<TDomain> repository)
        {
            this._repository = repository;
        }
        #endregion

        #region 属性
        protected ISysLoggerManagerService LoggerService { get { return IoC.Resolve<ISysLoggerManagerService>(); } }
        #endregion

        #region 获取
        /// <summary>
        /// 根据Id获取Dto对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TDto GetModel(string id)
        {
            var data = GetDomain(id);
            return Map(data);
        }
        /// <summary>
        /// 根据Id获取Domain对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected TDomain GetDomain(string id)
        {
            return _repository.GetByKey(id);
        }
        /// <summary>
        /// 根据条件获取Domain对象
        /// </summary>
        /// <param name="specs"></param>
        /// <returns></returns>
        protected TDomain GetDomain(ISpecification<TDomain> specs)
        {
            return _repository.Get(specs);
        }
        /// <summary>
        /// 根据PagerXXXFindRequest对象和排序字段获取分页查询结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public T Search<T>(TRequest request, Dictionary<Expression<Func<TDomain, object>>, SortOrder> order) where T : PagerFindResponse<TDto>
        {
            Check.Argument.IsNotNull(request, "request");
            PagedResult<TDomain> paged = this._repository.FindInPage(request.PageIndex, request.PageSize, this.ConvertToSpec(request), order);
            T result = Activator.CreateInstance<T>();
            result.Init(paged.TotalRecords, paged.TotalPages, MapList(paged.Data));
            return result;
        }

        /// <summary>
        /// 获取Dto集合列表
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TDto> GetList()
        {
            var list = this._repository.GetList();
            return MapList(list);
        }

        /// <summary>
        /// 获取Dto集合列表
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TDomain> GetDomainList(TRequest request)
        {
            return this._repository.GetList(ConvertToSpec(request));
        }

        /// <summary>
        /// 根据PagerXXXFindRequest对象获取Dto集合列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual IEnumerable<TDto> GetList(TRequest request)
        {
            var list = this._repository.GetList(ConvertToSpec(request));
            return MapList(list);
        }

        /// <summary>
        /// 根据条件获取Dto集合列表
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TDto> GetList(ISpecification<TDomain> specs)
        {
            var list = this._repository.GetList(specs);
            return MapList(list);
        }

        /// <summary>
        /// 根据条件获取Domain集合列表
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<TDomain> GetDomainList(ISpecification<TDomain> specs)
        {
            return this._repository.GetList(specs);
        }

        /// <summary>
        /// 根据条件获取是否存在记录
        /// </summary>
        /// <param name="specs"></param>
        /// <returns></returns>
        protected bool Exists(ISpecification<TDomain> specs)
        {
            return this._repository.Exists(specs);
        }

        /// <summary>
        /// 设置条件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected abstract ISpecification<TDomain> ConvertToSpec(TRequest request);

        #endregion

        #region 操作
        public void Create(TDomain info)
        {
            this._repository.Add(info);
        }
        public void Update(TDomain info)
        {
            this._repository.Update(info);
        }
        public void Delete(TDomain info)
        {
            this._repository.Remove(info);
        }
        #endregion

        #region 其他方法
        protected ISpecification<TDomain> GetSpecs(List<ISpecification<TDomain>> specs)
        {
            if (specs.Count > 0)
            {
                var result = specs[0];
                for (int i = 1; i < specs.Count; i++)
                {
                    result = result.And(specs[i]);
                }
                return result;
            }
            return null;
        }

        protected virtual TDto Map(TDomain source)
        {
            //return source == null ? null : TypeAdapter.Adapt<Domain, Dto>(source);
            return source == null ? null : Mapper.Map<TDomain, TDto>(source);
        }
        protected IEnumerable<TDto> MapList(IEnumerable<TDomain> list)
        {
            return Array.ConvertAll(list.ToArray(), item => Map(item));
        }
        protected IEnumerable<TDomain> EmptyDomainList()
        {
            return Enumerable.Empty<TDomain>();
        }
        protected IEnumerable<TDto> EmptyList()
        {
            return Enumerable.Empty<TDto>();
        }
        #endregion
    }
}
