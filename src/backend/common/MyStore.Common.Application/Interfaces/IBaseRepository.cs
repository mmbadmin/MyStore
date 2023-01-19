namespace MyStore.Common.Application.Interfaces
{
    using MyStore.Common.Application.Models;
    using MyStore.Common.Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IBaseRepository<TEntity, TKey> : IDisposable
        where TEntity : BaseEntity<TKey>, IAggregateRoot
        where TKey : struct, IComparable<TKey>, IEquatable<TKey>
    {
        IQueryable<TEntity> Queryable { get; }

        bool Add(TEntity entity);

        Task<bool> AddAsync(TEntity entity);

        bool AddRange(IEnumerable<TEntity> entities);

        Task<bool> AddRangeAsync(IEnumerable<TEntity> entities);

        bool Any();

        bool Any(Expression<Func<TEntity, bool>> predicate);

        Task<bool> AnyAsync();

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        int Count();

        int Count(Expression<Func<TEntity, bool>> predicate);

        Task<int> CountAsync();

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);

        bool Delete(TEntity entity);

        bool Delete(TKey id);

        bool Delete(Expression<Func<TEntity, bool>> predicate);

        Task<bool> DeleteAsync(TEntity entity);

        Task<bool> DeleteAsync(TKey id);

        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        bool DeleteRange(Expression<Func<TEntity, bool>> predicate);

        bool DeleteRange(IEnumerable<TEntity> entities);

        Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate);

        Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities);

        TEntity Find(Expression<Func<TEntity, bool>>? predicate = null,
                     List<Expression<Func<TEntity, object>>>? includes = null,
                     FilterCollection? filters = null,
                     IList<OrderModel>? orderModels = null);

        TSelect Find<TSelect>(Expression<Func<TEntity, TSelect>> selector,
                              Expression<Func<TEntity, bool>>? predicate = null,
                              List<Expression<Func<TEntity, object>>>? includes = null,
                              FilterCollection? filters = null,
                              IList<OrderModel>? orderModels = null);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>>? predicate = null,
                                List<Expression<Func<TEntity, object>>>? includes = null,
                                FilterCollection? filters = null,
                                IList<OrderModel>? orderModels = null);

        Task<TSelect> FindAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector,
                                         Expression<Func<TEntity, bool>>? predicate = null,
                                         List<Expression<Func<TEntity, object>>>? includes = null,
                                         FilterCollection? filters = null,
                                         IList<OrderModel>? orderModels = null);

        List<TEntity> List(Expression<Func<TEntity, bool>>? predicate = null,
                           List<Expression<Func<TEntity, object>>>? includes = null,
                           FilterCollection? filters = null,
                           IList<OrderModel>? orderModels = null);

        List<TSelect> List<TSelect>(Expression<Func<TEntity, TSelect>> selector,
                                    Expression<Func<TEntity, bool>>? predicate = null,
                                    List<Expression<Func<TEntity, object>>>? includes = null,
                                    FilterCollection? filters = null,
                                    IList<OrderModel>? orderModels = null);

        Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? predicate = null,
                                      List<Expression<Func<TEntity, object>>>? includes = null,
                                      FilterCollection? filters = null,
                                      IList<OrderModel>? orderModels = null);

        Task<List<TSelect>> ListAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector,
                                               Expression<Func<TEntity, bool>>? predicate = null,
                                               List<Expression<Func<TEntity, object>>>? includes = null,
                                               FilterCollection? filters = null,
                                               IList<OrderModel>? orderModels = null);

        IPagedList<TSelect> PagedList<TSelect>(Expression<Func<TEntity, TSelect>> selector,
                                               Expression<Func<TEntity, bool>>? predicate = null,
                                               List<Expression<Func<TEntity, object>>>? includes = null,
                                               FilterCollection? filters = null,
                                               IList<OrderModel>? orderModels = null,
                                               int page = 1,
                                               int pageSize = 10);

        Task<IPagedList<TSelect>> PagedListAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector,
                                                          Expression<Func<TEntity, bool>>? predicate = null,
                                                          List<Expression<Func<TEntity, object>>>? includes = null,
                                                          FilterCollection? filters = null,
                                                          IList<OrderModel>? orderModels = null,
                                                          int page = 1,
                                                          int pageSize = 10);

        Task<IPagedList<TSelect>> PagedListAsync<TSelect, TGroupBy>(Expression<Func<TGroupBy, TSelect>> selector,
                                                          Expression<Func<TEntity, bool>>? predicate = null,
                                                          Expression<Func<TEntity, TGroupBy>>? groupby = null,
                                                          List<Expression<Func<TEntity, object>>>? includes = null,
                                                          FilterCollection? filters = null,
                                                          IList<OrderModel>? orderModels = null,
                                                          int page = 1,
                                                          int pageSize = 10);

        Task<IPagedList<T>> FromSqlPaged<T>(string text, object data = null);

        bool Remove(TEntity entity);

        bool Remove(TKey id);

        bool Remove(Expression<Func<TEntity, bool>> predicate);

        Task<bool> RemoveAsync(TEntity entity);

        Task<bool> RemoveAsync(TKey id);

        Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> predicate);

        bool RemoveRange(Expression<Func<TEntity, bool>> predicate);

        bool RemoveRange(IEnumerable<TEntity> entities);

        Task<bool> RemoveRangeAsync(Expression<Func<TEntity, bool>> predicate);

        Task<bool> RemoveRangeAsync(IEnumerable<TEntity> entities);

        int Save();

        Task<int> SaveAsync();

        bool Update(TEntity entity);

        Task<bool> UpdateAsync(TEntity entity);

        bool UpdateRange(IEnumerable<TEntity> entities);

        Task<bool> UpdateRangeAsync(IEnumerable<TEntity> entities);
    }
}
