using CachedEfCore.Cache.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using MqttPub.Domain.Entities;
using MqttPub.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MqttPub.Infrastructure
{
    internal partial class Repository<T> : IRepository<T> where T : class, IEntity
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

        public virtual T? FirstOrDefault(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.FirstOrDefault(where);
            }
            var result = _dbQueryCacheHelper.GetOrAdd<T?, T>(_dbContext, () => _dbset.FirstOrDefault(where), where);
            return result;
        }

        public virtual T? FirstOrDefaultAsNoTracking(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.AsNoTrackingWithIdentityResolution().FirstOrDefault(where);
            }

            var result = _dbQueryCacheHelper.GetOrAdd<T?, T>(_dbContext, () => _dbset.AsNoTrackingWithIdentityResolution().FirstOrDefault(where), where);
            return result;
        }

        public virtual T? GetById(int id, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.Find(id);
            }

            var result = _dbQueryCacheHelper.GetOrAdd<T?, T>(_dbContext, () => _dbset.Find(id), id.ToString());
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

        public virtual IEnumerable<T> WhereAsNoTracking(Expression<Func<T, bool>> where, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.AsNoTrackingWithIdentityResolution().Where(where).ToList();
            }

            var result = _dbQueryCacheHelper.GetOrAdd<IEnumerable<T>, T>(_dbContext, () => _dbset.AsNoTrackingWithIdentityResolution().Where(where).ToList(), where)!;
            return result;
        }

        public virtual IQueryable<T> WhereAsQueryable(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where);
        }

        public virtual IQueryable<T> WhereAsQueryableAndAsNoTracking(Expression<Func<T, bool>> where)
        {
            return _dbset.AsNoTrackingWithIdentityResolution().Where(where);
        }

        public virtual TResult? SelectFirstOrDefault<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true)
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

        public virtual TResult? SelectFirstOrDefaultAsNoTracking<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true)
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

        public virtual IEnumerable<TResult> WhereSelect<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true)
        {
            if (useCache == false)
            {
                return _dbset.Where(where).Select(selector).ToList();
            }

            var result = _dbQueryCacheHelper.GetOrAdd<IEnumerable<TResult>, T>(_dbContext, () => _dbset.Where(where).Select(selector).ToList(), [where, selector])!;
            return result;
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

        public virtual EntityEntry<T> Add(T entity)
        {
            return _dbset.Add(entity);
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
        
        public virtual void Transaction(Action action)
        {
            Transaction(transaction => action());
        }

        public virtual int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}