using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MqttPub.Infrastructure
{
    internal partial class Repository<T>
    {
        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return await _dbset.AsNoTrackingWithIdentityResolution().AnyAsync(where);
            }
            var result = await _dbQueryCacheHelper.GetOrAddAsync<bool, T>(_dbContext, () => _dbset.AsNoTrackingWithIdentityResolution().AnyAsync(where), where);
            return result;
        }

        public virtual async Task<T> FirstAsync(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return await _dbset.FirstAsync(where);
            }

            var result = await _dbQueryCacheHelper.GetOrAddAsync<T, T>(_dbContext, () => _dbset.FirstAsync(where), where);
            return result;
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return await _dbset.FirstOrDefaultAsync(where);
            }

            var result = await _dbQueryCacheHelper.GetOrAddAsync<T?, T>(_dbContext, () => _dbset.FirstOrDefaultAsync(where), where);
            return result;
        }

        public virtual async Task<T?> FirstOrDefaultAsNoTrackingAsync(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return await _dbset.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(where);
            }

            var result = await _dbQueryCacheHelper.GetOrAddAsync<T?, T>(_dbContext, () => _dbset.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(where), where);
            return result;
        }

        public virtual async Task<T> FirstAsNoTrackingAsync(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return await _dbset.AsNoTrackingWithIdentityResolution().FirstAsync(where);
            }

            var result = await _dbQueryCacheHelper.GetOrAddAsync<T, T>(_dbContext, () => _dbset.AsNoTrackingWithIdentityResolution().FirstAsync(where), where);
            return result;
        }

        public virtual ValueTask<T?> GetByIdAsync(int id, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.FindAsync(id);
            }

            var result = _dbQueryCacheHelper.GetOrAddAsync<T?, T>(_dbContext, async () => await _dbset.FindAsync(id), id.ToString());
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

        public virtual async Task<IEnumerable<T>> WhereAsNoTrackingAsync(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return await _dbset.AsNoTrackingWithIdentityResolution().Where(where).ToListAsync();
            }

            var result = await _dbQueryCacheHelper.GetOrAddAsync<IEnumerable<T>, T>(_dbContext, async () => await _dbset.AsNoTrackingWithIdentityResolution().Where(where).ToListAsync(), where);
            return result!;
        }

        public virtual async Task<TResult?> SelectFirstOrDefaultAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true)
        {
            if (useCache == false)
            {
                return await _dbset.Where(where).Select(selector).FirstOrDefaultAsync();
            }

            var result = await _dbQueryCacheHelper.GetOrAddAsync<TResult?, T>(_dbContext, () =>
            {
                var firstOrDefault = _dbset.Where(where).Select(selector).FirstOrDefaultAsync();
                return firstOrDefault;
            }, [where, selector]);

            return result;
        }

        public virtual async Task<TResult> SelectFirstAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true)
        {
            if (useCache == false)
            {
                return await _dbset.Where(where).Select(selector).FirstAsync();
            }

            var result = await _dbQueryCacheHelper.GetOrAddAsync<TResult, T>(_dbContext, () =>
            {
                var firstOrDefault = _dbset.Where(where).Select(selector).FirstAsync();
                return firstOrDefault;
            }, [where, selector]);

            return result;
        }

        public virtual async Task<TResult?> SelectFirstOrDefaultAsNoTrackingAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true)
        {
            if (useCache == false)
            {
                return await _dbset.AsNoTrackingWithIdentityResolution().Where(where).Select(selector).FirstOrDefaultAsync();
            }

            var result = await _dbQueryCacheHelper.GetOrAddAsync<TResult?, T>(_dbContext, () =>
            {
                var firstOrDefault = _dbset.AsNoTrackingWithIdentityResolution().Where(where).Select(selector).FirstOrDefaultAsync();
                return firstOrDefault;
            }, [where, selector]);

            return result;
        }

        public virtual async Task<TResult> SelectFirstAsNoTrackingAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true)
        {
            if (useCache == false)
            {
                return await _dbset.AsNoTrackingWithIdentityResolution().Where(where).Select(selector).FirstAsync();
            }

            var result = await _dbQueryCacheHelper.GetOrAddAsync<TResult, T>(_dbContext, () =>
            {
                var firstOrDefault = _dbset.AsNoTrackingWithIdentityResolution().Where(where).Select(selector).FirstAsync();
                return firstOrDefault;
            }, [where, selector]);

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

        public virtual async Task<IEnumerable<TResult>> WhereSelectAsNoTrackingAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true)
        {
            if (useCache == false)
            {
                return await _dbset.AsNoTrackingWithIdentityResolution().Where(where).Select(selector).ToListAsync();
            }

            var result = await _dbQueryCacheHelper.GetOrAddAsync<IEnumerable<TResult>, T>(_dbContext, async () => await _dbset.AsNoTrackingWithIdentityResolution().Where(where).Select(selector).ToListAsync(), [where, selector])!;
            return result!;
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbset.AddAsync(entity);
        }

        public virtual Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).ExecuteDeleteAsync();
        }

        public virtual Task TransactionAsync(Func<Task> action)
        {
            return TransactionAsync(transaction => action());
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

        public virtual Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
