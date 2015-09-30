﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Shoy.Core.Domain.Entities;
using Shoy.Utility;

namespace Shoy.Core.Domain.Repositories
{
    public interface IRepository<TDbContext, TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : DEntity<TKey>
        where TDbContext : IUnitOfWork
    {
    }

    /// <summary> 数据基础仓储接口 </summary>
    public interface IRepository<TEntity, TKey> : IDependency
        where TEntity : DEntity<TKey>
    {
        /// <summary> 数据操作单元 </summary>
        IUnitOfWork UnitOfWork { get; }

        IQueryable<TEntity> Table { get; }

        TKey Insert(TEntity entity);

        int Insert(IEnumerable<TEntity> entities);

        int Delete(TEntity entity);

        int Delete(TKey key);

        int Delete(Expression<Func<TEntity, bool>> expression);

        int Update(TEntity entity);

        int Update(TEntity entity, Expression<Func<TEntity, bool>> expression);

        bool Exists(Expression<Func<TEntity, bool>> expression);

        TEntity Load(TKey key);

        TEntity First(Expression<Func<TEntity, bool>> expression);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression);
        TEntity Single(Expression<Func<TEntity, bool>> expression);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> expression);

        IQueryable<TEntity> List(IEnumerable<TKey> keys);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);

        DResults<TEntity> PageList(IOrderedQueryable<TEntity> ordered, DPage page);

        int Count();

        int Count(Expression<Func<TEntity, bool>> predicate);

        long LongCount();

        long LongCount(Expression<Func<TEntity, bool>> predicate);

        TValue Max<TValue>(Func<TEntity, TValue> perdicate, Expression<Func<TEntity, bool>> condition = null);

        TValue Min<TValue>(Func<TEntity, TValue> perdicate, Expression<Func<TEntity, bool>> condition = null);

        float Average(Func<TEntity, float> perdicate, Expression<Func<TEntity, bool>> condition = null);
        double Average(Func<TEntity, int> perdicate, Expression<Func<TEntity, bool>> condition = null);
        double Average(Func<TEntity, long> perdicate, Expression<Func<TEntity, bool>> condition = null);
        double Average(Func<TEntity, double> perdicate, Expression<Func<TEntity, bool>> condition = null);
        decimal Average(Func<TEntity, decimal> perdicate, Expression<Func<TEntity, bool>> condition = null);

#if NET45
        Task<TKey> InsertAsync(TEntity entity);

        Task<int> InsertAsync(IEnumerable<TEntity> entities);

        Task<int> DeleteAsync(TEntity entity);

        Task<int> DeleteAsync(TKey key);

        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> expression);

        Task<int> UpdateAsync(TEntity entity);

        Task<int> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> expression);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> LoadAsync(TKey key);

        Task<IQueryable<TEntity>> ListAsync(IEnumerable<TKey> keys);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression);

        Task<IQueryable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> expression);

        Task<DResults<TEntity>> PageListAsync(IOrderedQueryable<TEntity> ordered, DPage page);
        
        Task<int> CountAsync();

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        Task<long> LongCountAsync();

        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);
#endif
    }
}
