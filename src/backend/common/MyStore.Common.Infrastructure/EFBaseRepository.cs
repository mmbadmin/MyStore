namespace MyStore.Common.Infrastructure
{
    using Dapper;
    using Microsoft.EntityFrameworkCore;
    using RExtension;
    using MyStore.Common.Application.Exceptions;
    using MyStore.Common.Application.Interfaces;
    using MyStore.Common.Application.Models;
    using MyStore.Common.Domain;
    using MyStore.Common.Infrastructure;
    using MyStore.Common.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class EFBaseRepository<TDbContext, TEntity, TKey> : IBaseRepository<TEntity, TKey>, IDisposable
        where TDbContext : BaseDbContext
        where TEntity : BaseEntity<TKey>, IAggregateRoot
        where TKey : struct, IComparable<TKey>, IEquatable<TKey>
    {
        public EFBaseRepository(TDbContext db)
        {
            Db = db;
            Entity = db.Set<TEntity>();
        }

        protected TDbContext Db { get; private set; }

        protected DbSet<TEntity> Entity { get; private set; }

        public IQueryable<TEntity> Queryable => Entity;

        public bool Add(TEntity entity)
        {
            Entity.Add(entity);
            return Save() > 0;
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            Entity.Add(entity);
            return await SaveAsync() > 0;
        }

        public bool AddRange(IEnumerable<TEntity> entities)
        {
            Entity.AddRange(entities);
            return Save() > 0;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Entity.AddRangeAsync(entities);
            return await SaveAsync() > 0;
        }

        public bool Any()
        {
            return Entity.Any();
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return Entity.Any(predicate);
        }

        public Task<bool> AnyAsync()
        {
            return Entity.AnyAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Entity.AnyAsync(predicate);
        }

        public int Count()
        {
            return Entity.Count();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return Entity.Count(predicate);
        }

        public Task<int> CountAsync()
        {
            return Entity.CountAsync();
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Entity.CountAsync(predicate);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return Entity.Where(predicate);
        }

        public bool Delete(TEntity entity)
        {
            if (entity == null)
            {
                return false;
            }
            foreach (var fk in Db.Entry(entity).Collections)
            {
                if (fk.Query().Count() > 0)
                {
                    throw BaseException.BadRequest("It is not possible to delete this option");
                }
            }
            if (typeof(IDeletedEntity).IsAssignableFrom(typeof(TEntity)))
            {
                Db.Entry(entity).Property(BaseDbContext.IsDeleted).CurrentValue = true;
                return Update(entity);
            }
            else
            {
                return Remove(entity);
            }
        }

        public bool Delete(TKey id)
        {
            var entity = Entity.Find(id);
            return Delete(entity);
        }

        public bool Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = Entity.FirstOrDefault(predicate);
            return Delete(entity);
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            if (entity == null)
            {
                return false;
            }
            foreach (var fk in Db.Entry(entity).Collections)
            {
                if (fk.Query().Count() > 0)
                {
                    throw BaseException.BadRequest("It is not possible to delete this option");
                }
            }
            if (typeof(IDeletedEntity).IsAssignableFrom(typeof(TEntity)))
            {
                Db.Entry(entity).Property(BaseDbContext.IsDeleted).CurrentValue = true;
                return await UpdateAsync(entity);
            }
            else
            {
                return await RemoveAsync(entity);
            }
        }

        public async Task<bool> DeleteAsync(TKey id)
        {
            var entity = await Entity.FindAsync(id);
            return await DeleteAsync(entity);
        }

        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = await Entity.FirstOrDefaultAsync(predicate);
            return await DeleteAsync(entity);
        }

        public bool DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = Entity.Where(predicate).ToList();
            return DeleteRange(entities);
        }

        public bool DeleteRange(IEnumerable<TEntity> entities)
        {
            if (entities.Count() == 0)
            {
                return false;
            }
            if (typeof(IDeletedEntity).IsAssignableFrom(typeof(TEntity)))
            {
                foreach (var entity in entities)
                {
                    foreach (var fk in Db.Entry(entity).Collections)
                    {
                        if (fk.Query().Count() > 0)
                        {
                            throw BaseException.BadRequest("It is not possible to delete this option");
                        }
                    }
                    Db.Entry(entity).Property(BaseDbContext.IsDeleted).CurrentValue = true;
                }
                return UpdateRange(entities);
            }
            else
            {
                return RemoveRange(entities);
            }
        }

        public async Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await Entity.Where(predicate).ToListAsync();
            return await DeleteRangeAsync(entities);
        }

        public Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities.Count() == 0)
            {
                return Task.FromResult(false);
            }
            if (typeof(IDeletedEntity).IsAssignableFrom(typeof(TEntity)))
            {
                foreach (var entity in entities)
                {
                    foreach (var fk in Db.Entry(entity).Collections)
                    {
                        if (fk.Query().Count() > 0)
                        {
                            throw BaseException.BadRequest("It is not possible to delete this option");
                        }
                    }
                    Db.Entry(entity).Property(BaseDbContext.IsDeleted).CurrentValue = true;
                }
                return UpdateRangeAsync(entities);
            }
            else
            {
                return RemoveRangeAsync(entities);
            }
        }

        public void Dispose()
        {
            Db?.Dispose();
        }

        public TEntity Find(Expression<Func<TEntity, bool>>? predicate = null,
                            List<Expression<Func<TEntity, object>>>? includes = null,
                            FilterCollection? filters = null,
                            IList<OrderModel>? orderModels = null)
        {
            var query = QueryBuilder(predicate: predicate,
                                     filters: filters,
                                     includes: includes,
                                     orderModels: orderModels);
            return query.FirstOrDefault();
        }

        public TSelect Find<TSelect>(Expression<Func<TEntity, TSelect>> selector,
                                     Expression<Func<TEntity, bool>>? predicate = null,
                                     List<Expression<Func<TEntity, object>>>? includes = null,
                                     FilterCollection? filters = null,
                                     IList<OrderModel>? orderModels = null)
        {
            var query = QueryBuilder(predicate: predicate,
                                     filters: filters,
                                     includes: includes,
                                     orderModels: orderModels);
            return query.Select(selector).FirstOrDefault();
        }

        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>>? predicate = null,
                                       List<Expression<Func<TEntity, object>>>? includes = null,
                                       FilterCollection? filters = null,
                                       IList<OrderModel>? orderModels = null)
        {
            var query = QueryBuilder(predicate: predicate,
                                     filters: filters,
                                     includes: includes,
                                     orderModels: orderModels);
            return query.FirstOrDefaultAsync();
        }

        public Task<TSelect> FindAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector,
                                                Expression<Func<TEntity, bool>>? predicate = null,
                                                List<Expression<Func<TEntity, object>>>? includes = null,
                                                FilterCollection? filters = null,
                                                IList<OrderModel>? orderModels = null)
        {
            var query = QueryBuilder(predicate: predicate,
                                     filters: filters,
                                     includes: includes,
                                     orderModels: orderModels);
            return query.Select(selector).FirstOrDefaultAsync();
        }

        public List<TEntity> List(Expression<Func<TEntity, bool>>? predicate = null,
                                  List<Expression<Func<TEntity, object>>>? includes = null,
                                  FilterCollection? filters = null,
                                  IList<OrderModel>? orderModels = null)
        {
            var query = QueryBuilder(predicate: predicate,
                                    filters: filters,
                                    includes: includes,
                                    orderModels: orderModels);
            return query.ToList();
        }

        public List<TSelect> List<TSelect>(Expression<Func<TEntity, TSelect>> selector,
                                           Expression<Func<TEntity, bool>>? predicate = null,
                                           List<Expression<Func<TEntity, object>>>? includes = null,
                                           FilterCollection? filters = null,
                                           IList<OrderModel>? orderModels = null)
        {
            var query = QueryBuilder(predicate: predicate,
                                     filters: filters,
                                     includes: includes,
                                     orderModels: orderModels);
            return query.Select(selector).ToList();
        }

        public Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? predicate = null,
                                             List<Expression<Func<TEntity, object>>>? includes = null,
                                             FilterCollection? filters = null,
                                             IList<OrderModel>? orderModels = null)
        {
            var query = QueryBuilder(predicate: predicate,
                                     filters: filters,
                                     includes: includes,
                                     orderModels: orderModels);
            return query.ToListAsync();
        }

        public Task<List<TSelect>> ListAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector,
                                                      Expression<Func<TEntity, bool>>? predicate = null,
                                                      List<Expression<Func<TEntity, object>>>? includes = null,
                                                      FilterCollection? filters = null,
                                                      IList<OrderModel>? orderModels = null)
        {
            var query = QueryBuilder(predicate: predicate,
                                     filters: filters,
                                     includes: includes,
                                     orderModels: orderModels);
            return query.Select(selector).ToListAsync();
        }

        public IPagedList<TSelect> PagedList<TSelect>(Expression<Func<TEntity, TSelect>> selector,
                                                      Expression<Func<TEntity, bool>>? predicate = null,
                                                      List<Expression<Func<TEntity, object>>>? includes = null,
                                                      FilterCollection? filters = null,
                                                      IList<OrderModel>? orderModels = null,
                                                      int page = 1,
                                                      int pageSize = 10)
        {
            var query = QueryBuilder(predicate: predicate,
                                   filters: filters,
                                   includes: includes,
                                   orderModels: orderModels);
            var (skip, take) = GetPage(page, pageSize);
            var total = query.Count();
            query = query.Skip(skip).Take(take);
            var data = query.Select(selector).ToList();
            return new PagedList<TSelect>
            {
                Data = data,
                Total = total,
            };
        }

        public async Task<IPagedList<TSelect>> PagedListAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector,
                                                                       Expression<Func<TEntity, bool>>? predicate = null,
                                                                       List<Expression<Func<TEntity, object>>>? includes = null,
                                                                       FilterCollection? filters = null,
                                                                       IList<OrderModel>? orderModels = null,
                                                                       int page = 1,
                                                                       int pageSize = 10)
        {
            var query = QueryBuilder(predicate: predicate,
                                     filters: filters,
                                     includes: includes,
                                     orderModels: orderModels);
            var (skip, take) = GetPage(page, pageSize);
            var total = await query.CountAsync();
            query = query.Skip(skip).Take(take);
            var data = await query.Select(selector).ToListAsync();
            return new PagedList<TSelect>
            {
                Data = data,
                Total = total,
            };
        }

        public async Task<IPagedList<TSelect>> PagedListAsync<TSelect, TGroupBy>(Expression<Func<TGroupBy, TSelect>> selector,
                                                                       Expression<Func<TEntity, bool>>? predicate = null,
                                                                       Expression<Func<TEntity, TGroupBy>>? groupby = null,
                                                                       List<Expression<Func<TEntity, object>>>? includes = null,
                                                                       FilterCollection? filters = null,
                                                                       IList<OrderModel>? orderModels = null,
                                                                       int page = 1,
                                                                       int pageSize = 10)
        {
            var query = QueryBuilder(predicate: predicate,
                                     filters: filters,
                                     includes: includes,
                                     orderModels: orderModels)
                .GroupBy(groupby).Select(e => e.Key).Select(selector);
            if (orderModels is { } && orderModels.Count() > 0)
            {
                query = query.OrderBy(orderModels);
            }
            var (skip, take) = GetPage(page, pageSize);

            var total = await query.CountAsync();

            var data = await query.Skip(skip).Take(take).ToListAsync();

            return new PagedList<TSelect>
            {
                Data = data,
                Total = total,
            };
        }


        public async Task<IPagedList<TSelect>> FromSqlPaged<TSelect>(string text, object data = null)
        {
            var conn = Db.Database.GetDbConnection();
            var list = new PagedList<TSelect>();
            using (var multi = await conn.QueryMultipleAsync(text, data, commandType: CommandType.StoredProcedure))
            {
                list.Data = multi.Read<TSelect>().AsList();
                list.Total = multi.Read<int>().SingleOrDefault();
            }
            return list;
        }

        public bool Remove(TEntity entity)
        {
            if (entity == null)
            {
                return false;
            }
            foreach (var fk in Db.Entry(entity).Collections)
            {
                if (fk.Query().Count() > 0)
                {
                    throw BaseException.BadRequest("It is not possible to delete this option");
                }
            }
            var dbEntityEntry = Db.Entry(entity);
            dbEntityEntry.State = EntityState.Deleted;
            return Save() > 0;
        }

        public bool Remove(TKey id)
        {
            var entity = Entity.Find(id);
            return Remove(entity);
        }

        public bool Remove(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = Entity.FirstOrDefault(predicate);
            return Remove(entity);
        }

        public async Task<bool> RemoveAsync(TEntity entity)
        {
            if (entity == null)
            {
                return false;
            }
            foreach (var fk in Db.Entry(entity).Collections)
            {
                if (fk.Query().Count() > 0)
                {
                    throw BaseException.BadRequest("It is not possible to delete this option");
                }
            }
            var dbEntityEntry = Db.Entry(entity);
            dbEntityEntry.State = EntityState.Deleted;
            return await SaveAsync() > 0;
        }

        public async Task<bool> RemoveAsync(TKey id)
        {
            var entity = await Entity.FindAsync(id);
            return await RemoveAsync(entity);
        }

        public async Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = await Entity.FirstOrDefaultAsync(predicate);
            return await RemoveAsync(entity);
        }

        public bool RemoveRange(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = Entity.Where(predicate).ToList();
            return RemoveRange(entities);
        }

        public bool RemoveRange(IEnumerable<TEntity> entities)
        {
            if (entities.Count() == 0)
            {
                return false;
            }
            foreach (var entity in entities)
            {
                var dbEntityEntry = Db.Entry(entity);
                dbEntityEntry.State = EntityState.Deleted;
            }
            return Save() > 0;
        }

        public async Task<bool> RemoveRangeAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await Entity.Where(predicate).ToListAsync();
            return await RemoveRangeAsync(entities);
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities.Count() == 0)
            {
                return false;
            }
            foreach (var entity in entities)
            {
                foreach (var fk in Db.Entry(entity).Collections)
                {
                    if (fk.Query().Count() > 0)
                    {
                        throw BaseException.BadRequest("It is not possible to delete this option");
                    }
                }
                var dbEntityEntry = Db.Entry(entity);
                dbEntityEntry.State = EntityState.Deleted;
            }
            return await SaveAsync() > 0;
        }

        public int Save()
        {
            return Db.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return Db.SaveChangesAsync();
        }

        public bool Update(TEntity entity)
        {
            var dbEntityEntry = Db.Entry(entity);
            dbEntityEntry.State = EntityState.Modified;
            return Save() > 0;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            var dbEntityEntry = Db.Entry(entity);
            dbEntityEntry.State = EntityState.Modified;
            return await SaveAsync() > 0;
        }

        public bool UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                var dbEntityEntry = Db.Entry(entity);
                dbEntityEntry.State = EntityState.Modified;
            }
            return Save() > 0;
        }

        public async Task<bool> UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity is null)
                {
                    continue;
                }
                var dbEntityEntry = Db.Entry(entity);
                dbEntityEntry.State = EntityState.Modified;
            }
            return await SaveAsync() > 0;
        }

        protected (int skip, int take) GetPage(int? page, int? pageSize)
        {
            if (page == null || page.Value <= 0)
            {
                page = 1;
            }
            if (pageSize == null || pageSize.Value <= 0 || pageSize.Value > 50)
            {
                pageSize = 10;
            }
            var skip = (page.Value - 1) * pageSize.Value;
            return (skip, pageSize.Value);
        }

        private IQueryable<TEntity> QueryBuilder(Expression<Func<TEntity, bool>>? predicate = null,
                                                 List<Expression<Func<TEntity, object>>>? includes = null,
                                                 FilterCollection? filters = null,
                                                 IList<OrderModel>? orderModels = null)
        {
            var query = Entity.AsQueryable();
            if (predicate is { })
            {
                query = query.Where(predicate);
            }
            if (filters is { })
            {
                query = query.Where(filters);
            }
            if (orderModels is { } && orderModels.Count() > 0)
            {
                query = query.OrderBy(orderModels);
            }
            else
            {
                query = query.OrderByDescending(x => x.Id);
            }
            if (includes is { })
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }


    }
}
