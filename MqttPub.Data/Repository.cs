using CachedEfCore.Cache.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using MqttPub.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MqttPub.Data
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<T> _dbset;
        private readonly IDbQueryCacheHelper _dbQueryCacheHelper;

        public Repository(AppDbContext dataContext, 
            IDbQueryCacheHelper dbQueryCacheHelper)
        {
            _dbContext = dataContext;
            _dbset = _dbContext.Set<T>();
            _dbQueryCacheHelper = dbQueryCacheHelper;
        }

        public virtual bool Any(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.AsNoTrackingWithIdentityResolution().Any(where);
            }

            var result = _dbQueryCacheHelper.GetOrAdd<bool, T>(_dbContext, () => _dbset.AsNoTrackingWithIdentityResolution().Any(where), where);
            return result;
        }

        public virtual Task<bool> AnyAsync(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.AsNoTrackingWithIdentityResolution().AnyAsync(where);
            }
            var result = _dbQueryCacheHelper.GetOrAddAsync<bool, T>(_dbContext, () => _dbset.AsNoTrackingWithIdentityResolution().AnyAsync(where), where);
            return result;
        }

        public virtual T? FirstOrDefault(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.FirstOrDefault(where);
            }
            var result = _dbQueryCacheHelper.GetOrAdd<T, T>(_dbContext, () => _dbset.FirstOrDefault(where), where);
            return result;
        }

        public virtual Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.FirstOrDefaultAsync(where);
            }

            var result = _dbQueryCacheHelper.GetOrAddAsync<T, T>(_dbContext, () => _dbset.FirstOrDefaultAsync(where), where);
            return result;
        }

        public virtual T? FirstOrDefaultAsNoTracking(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.AsNoTrackingWithIdentityResolution().FirstOrDefault(where);
            }

            var result = _dbQueryCacheHelper.GetOrAdd<T, T>(_dbContext, () => _dbset.AsNoTrackingWithIdentityResolution().FirstOrDefault(where), where);
            return result;
        }

        public virtual Task<T?> FirstOrDefaultAsNoTrackingAsync(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(where);
            }

            var result = _dbQueryCacheHelper.GetOrAddAsync<T, T>(_dbContext, () => _dbset.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(where), where);
            return result;
        }

        public virtual T? GetById(int id, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.Find(id);
            }

            var result = _dbQueryCacheHelper.GetOrAdd<T, T>(_dbContext, () => _dbset.Find(id), id.ToString());
            return result;
        }

        public virtual Task<T?> GetByIdAsync(int id, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.FindAsync(id).AsTask();
            }

            var result = _dbQueryCacheHelper.GetOrAddAsync<T, T>(_dbContext, () => _dbset.FindAsync(id).AsTask(), id.ToString());
            return result;
        }

        public virtual IEnumerable<T> Where(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.Where(where).ToList();
            }

            var result = _dbQueryCacheHelper.GetOrAdd<IEnumerable<T>, T>(_dbContext, () => _dbset.Where(where).ToList(), where)!;
            return result;
        }

        public virtual async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return await _dbset.Where(where).ToListAsync();
            }

            var result = await _dbQueryCacheHelper.GetOrAddAsync<IEnumerable<T>, T>(_dbContext, async () => await _dbset.Where(where).ToListAsync(), where);
            return result!;
        }

        public virtual IEnumerable<T> WhereAsNoTracking(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.AsNoTrackingWithIdentityResolution().Where(where).ToList();
            }

            var result = _dbQueryCacheHelper.GetOrAdd<IEnumerable<T>, T>(_dbContext, () => _dbset.AsNoTrackingWithIdentityResolution().Where(where).ToList(), where)!;
            return result;
        }

        public virtual async Task<IEnumerable<T>> WhereAsNoTrackingAsync(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return await _dbset.AsNoTrackingWithIdentityResolution().Where(where).ToListAsync();
            }

            var result = await _dbQueryCacheHelper.GetOrAddAsync<IEnumerable<T>, T>(_dbContext, async () => await _dbset.AsNoTrackingWithIdentityResolution().Where(where).ToListAsync(), where);
            return result!;
        }

        public virtual IQueryable<T> WhereAsQueryable(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where);
        }

        public virtual IQueryable<T> WhereAsQueryableAndAsNoTracking(Expression<Func<T, bool>> where)
        {
            return _dbset.AsNoTrackingWithIdentityResolution().Where(where);
        }

        public virtual TResult? SelectFirst<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.Where(where).Select(selector).FirstOrDefault();
            }

            var result = _dbQueryCacheHelper.GetOrAdd<TResult?, T>(_dbContext, () =>
            {
                var firstOrDefault = _dbset.Where(where).Select(selector).FirstOrDefault();
                return firstOrDefault;
            }, [where, selector]);

            return result;
        }

        public virtual Task<TResult?> SelectFirstAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.Where(where).Select(selector).FirstOrDefaultAsync();
            }

            var result = _dbQueryCacheHelper.GetOrAddAsync<TResult?, T>(_dbContext, () =>
            {
                var firstOrDefault = _dbset.Where(where).Select(selector).FirstOrDefaultAsync();
                return firstOrDefault;
            }, [where, selector]);

            return result;
        }

        public virtual TResult? SelectFirstAsNoTracking<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.AsNoTrackingWithIdentityResolution().Where(where).Select(selector).FirstOrDefault();
            }

            var result = _dbQueryCacheHelper.GetOrAdd<TResult?, T>(_dbContext, () =>
            {
                var firstOrDefault = _dbset.AsNoTrackingWithIdentityResolution().Where(where).Select(selector).FirstOrDefault();
                return firstOrDefault;
            }, [where, selector]);

            return result;
        }

        public virtual Task<TResult?> SelectFirstAsNoTrackingAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.AsNoTrackingWithIdentityResolution().Where(where).Select(selector).FirstOrDefaultAsync();
            }

            var result = _dbQueryCacheHelper.GetOrAddAsync<TResult?, T>(_dbContext, () =>
            {
                var firstOrDefault = _dbset.AsNoTrackingWithIdentityResolution().Where(where).Select(selector).FirstOrDefaultAsync();
                return firstOrDefault;
            }, [where, selector]);

            return result;
        }

        public virtual IEnumerable<TResult> WhereSelect<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.Where(where).Select(selector).ToList();
            }

            var result = _dbQueryCacheHelper.GetOrAdd<IEnumerable<TResult>, T>(_dbContext, () => _dbset.Where(where).Select(selector).ToList(), [where, selector])!;
            return result;
        }

        public virtual async Task<IEnumerable<TResult>> WhereSelectAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true)
        {
            if (useCache == false)
            {
                return await _dbset.Where(where).Select(selector).ToListAsync();
            }

            var result = await _dbQueryCacheHelper.GetOrAddAsync<IEnumerable<TResult>, T>(_dbContext, async () => await _dbset.Where(where).Select(selector).ToListAsync(), [where, selector]);
            return result!;
        }

        public virtual IEnumerable<TResult> WhereSelectAsNoTracking<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.AsNoTrackingWithIdentityResolution().Where(where).Select(selector).ToList();
            }

            var result = _dbQueryCacheHelper.GetOrAdd<IEnumerable<TResult>, T>(_dbContext, () => _dbset.AsNoTrackingWithIdentityResolution().Where(where).Select(selector).ToList(), [where, selector])!;
            return result;
        }

        public virtual async Task<IEnumerable<TResult>> WhereSelectAsNoTrackingAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true)
        {
            if (useCache == false)
            {
                return await _dbset.AsNoTrackingWithIdentityResolution().Where(where).Select(selector).ToListAsync();
            }

            var result = await _dbQueryCacheHelper.GetOrAddAsync<IEnumerable<TResult>, T>(_dbContext, async () => await _dbset.AsNoTrackingWithIdentityResolution().Where(where).Select(selector).ToListAsync(), [where, selector])!;
            return result!;
        }

        public virtual IQueryable<TResult> WhereSelectAsQueryable<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector)
        {
            return _dbset.Where(where).Select(selector);
        }

        public virtual IQueryable<TResult> WhereSelectAsQueryableAndAsNoTracking<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector)
        {
            return _dbset.AsNoTrackingWithIdentityResolution().Where(where).Select(selector);
        }

        public virtual EntityEntry<T> Add(T entity)
        {
            return _dbset.Add(entity);
        }

        public virtual ValueTask<EntityEntry<T>> AddAsync(T entity)
        {
            return _dbset.AddAsync(entity);
        }

        public virtual EntityEntry<T> Attach(T entity)
        {
            return _dbset.Attach(entity);
        }

        public virtual EntityEntry<T> Update(T entity)
        {
            return _dbset.Update(entity);
        }

        public virtual EntityEntry<T> Update(T entity, params Expression<Func<T, object>>[] propertiesToIgnore)
        {
            var entry = _dbset.Update(entity);

            foreach (var propertyExpr in propertiesToIgnore)
            {
                entry.Property(propertyExpr).IsModified = false;
            }

            return entry;
        }

        public virtual EntityEntry<T> Delete(T entity)
        {
            return _dbset.Remove(entity);
        }

        public virtual int ExecuteDelete(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).ExecuteDelete();
        }

        public virtual Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).ExecuteDeleteAsync();
        }

        public virtual void Transaction(Action<IDbContextTransaction> action)
        {
            var currentTransaction = _dbContext.Database.CurrentTransaction;

            if (currentTransaction != null)
            {
                action(currentTransaction);
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    action(transaction);
                    if (_dbContext.Database.CurrentTransaction != null)
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (_dbContext.Database.CurrentTransaction != null)
                    {
                        transaction.Rollback();
                    }
                    throw;
                }
            }
        }

        public virtual async Task TransactionAsync(Func<IDbContextTransaction, Task> action)
        {
            var currentTransaction = _dbContext.Database.CurrentTransaction;

            if (currentTransaction != null)
            {
                await action(currentTransaction);
                return;
            }

            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await action(transaction);

                if (_dbContext.Database.CurrentTransaction != null)
                {
                    await transaction.CommitAsync();
                }
            }
            catch
            {
                if (_dbContext.Database.CurrentTransaction != null)
                {
                    await transaction.RollbackAsync();
                }

                throw;
            }
        }

        public virtual void Transaction(Action action)
        {
            Transaction(transaction => action());
        }

        public virtual Task TransactionAsync(Func<Task> action)
        {
            return TransactionAsync(transaction => action());
        }

        public virtual int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public virtual Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}