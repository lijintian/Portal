using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;




using MongoDB.Driver;
using EasyDDD.Infrastructure.Data.MongoDB;
using EasyDDD.Core.Aggregate;
using EasyDDD.Core.Repository;
using EasyDDD.Core.Specification;
using EasyDDD.Infrastructure.Crosscutting.Paged;

namespace Portal.Infrastructure.Data.MongoDB.Repository
{
    public class BaseRepository<TAggregateRoot> : MongoDBRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        #region 属性
        private readonly string _tableName;
        #endregion

        #region 初始化
        public BaseRepository(IRepositoryContext context, string tableName)
            : base(context)
        {
            this._tableName = tableName;
        }
        #endregion

        public T FindOne<T>(IMongoQuery query)
        {
            var permission = base._mongoDBRepositoryContext.MongoDatabase.GetCollection<T>(this._tableName).FindOne(query);
            return permission;
        }

        public IEnumerable<T> Find<T>(IMongoQuery query)
        {
            var permission = base._mongoDBRepositoryContext.MongoDatabase.GetCollection<T>(this._tableName).Find(query);
            return permission;
        }

        public IQueryable<T> Find<T>(IMongoQuery query, ISpecification<T> spceification)
        {
            var collection = _mongoDBRepositoryContext.MongoDatabase.GetCollection<T>(this._tableName);
            IQueryable<T> list = null;
            if (spceification == null)
            {
                list = collection.Find(query).AsQueryable<T>();
            }
            else
            {
                list = collection.Find(query).AsQueryable<T>().Where(spceification.GetExpression());
            }
            return list;
        }

        public PagedResult<TAggregateRoot> FindInPage(
            int pageNumber,
            int pageSize,
            IQueryable<TAggregateRoot> list,
            Dictionary<Expression<Func<TAggregateRoot, dynamic>>, SortOrder> orderBys)
        {
            return FindInPage(pageNumber, pageSize, list, orderBys);
        }

        public PagedResult<T> FindInPage<T>(
          int pageNumber,
          int pageSize,
          IQueryable<T> list,
          Dictionary<Expression<Func<T, dynamic>>, SortOrder> orderBys) where T : class, IAggregateRoot
        {
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "The pageNumber is one-based and should be larger than zero.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "The pageSize is one-based and should be larger than zero.");
            int skip = (pageNumber - 1) * pageSize;
            int take = pageSize;
            int totalCount = list.Count();
            int totalPages = (totalCount + pageSize - 1) / pageSize;
            IOrderedQueryable<T> orderlist = null;
            if (orderBys != null && orderBys.Count > 0)
            {
                foreach (var item in orderBys)
                {
                    if (item.Value == SortOrder.Descending)
                    {
                        orderlist = orderlist == null ? list.OrderByDescending(item.Key) : orderlist.ThenByDescending(item.Key);
                    }
                    else
                    {
                        orderlist = orderlist == null ? list.OrderBy(item.Key) : orderlist.ThenBy(item.Key);
                    }
                }
            }
            else
            {
                orderlist = list.OrderBy(c => c.Id);
            }
            List<T> pageList = null;
            if (orderlist != null) pageList = orderlist.Skip(skip).Take(take).ToList();
            if (pageList == null) pageList = new List<T>();
            return new PagedResult<T>(totalCount, totalPages, pageSize, pageNumber, pageList);
        }
    }
}
