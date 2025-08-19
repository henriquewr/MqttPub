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
    public interface IRepository<T> where T : class, IEntity
    {
        bool Any(Expression<Func<T, bool>> where, bool useCache = true);
        Task<bool> AnyAsync(Expression<Func<T, bool>> where, bool useCache = true);

        T? GetById(int id, bool useCache = true);
        Task<T?> GetByIdAsync(int id, bool useCache = true);

        T? FirstOrDefault(Expression<Func<T, bool>> where, bool useCache = true);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> where, bool useCache = true);

        T? FirstOrDefaultAsNoTracking(Expression<Func<T, bool>> where, bool useCache = true);
        Task<T?> FirstOrDefaultAsNoTrackingAsync(Expression<Func<T, bool>> where, bool useCache = true);

        IEnumerable<T> Where(Expression<Func<T, bool>> where, bool useCache = true);
        Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> where, bool useCache = true);

        IEnumerable<T> WhereAsNoTracking(Expression<Func<T, bool>> where, bool useCache = true);
        Task<IEnumerable<T>> WhereAsNoTrackingAsync(Expression<Func<T, bool>> where, bool useCache = true);

        IQueryable<T> WhereAsQueryable(Expression<Func<T, bool>> where);
        IQueryable<T> WhereAsQueryableAndAsNoTracking(Expression<Func<T, bool>> where);

        TResult? SelectFirst<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true);
        Task<TResult?> SelectFirstAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true);

        TResult? SelectFirstAsNoTracking<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true);
        Task<TResult?> SelectFirstAsNoTrackingAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true);

        IEnumerable<TResult> WhereSelect<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true);
        Task<IEnumerable<TResult>> WhereSelectAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true);

        IEnumerable<TResult> WhereSelectAsNoTracking<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true);
        Task<IEnumerable<TResult>> WhereSelectAsNoTrackingAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true);

        IQueryable<TResult> WhereSelectAsQueryable<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector);
        IQueryable<TResult> WhereSelectAsQueryableAndAsNoTracking<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector);

        EntityEntry<T> Add(T entity);
        ValueTask<EntityEntry<T>> AddAsync(T entity);
        EntityEntry<T> Attach(T entity);
        EntityEntry<T> Update(T entity);

        EntityEntry<T> Delete(T entity);
        int ExecuteDelete(Expression<Func<T, bool>> where);
        Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> where);

        void Transaction(Action action);
        Task TransactionAsync(Func<Task> action);
        void Transaction(Action<IDbContextTransaction> action);
        Task TransactionAsync(Func<IDbContextTransaction, Task> action);

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}