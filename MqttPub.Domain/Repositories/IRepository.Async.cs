using MqttPub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MqttPub.Domain.Repositories
{
    public partial interface IRepository<T> where T : class, IEntity
    {
        Task<bool> AnyAsync(Expression<Func<T, bool>> where, bool useCache = true);
        ValueTask<T?> GetByIdAsync(int id, bool useCache = true);

        Task<T> FirstAsync(Expression<Func<T, bool>> where, bool useCache = true);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> where, bool useCache = true);

        Task<T> FirstAsNoTrackingAsync(Expression<Func<T, bool>> where, bool useCache = true);
        Task<T?> FirstOrDefaultAsNoTrackingAsync(Expression<Func<T, bool>> where, bool useCache = true);

        Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> where, bool useCache = true);
        Task<IEnumerable<T>> WhereAsNoTrackingAsync(Expression<Func<T, bool>> where, bool useCache = true);

        Task<TResult> SelectFirstAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true);
        Task<TResult?> SelectFirstOrDefaultAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true);

        Task<TResult> SelectFirstAsNoTrackingAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true);
        Task<TResult?> SelectFirstOrDefaultAsNoTrackingAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true);
        
        Task<IEnumerable<TResult>> WhereSelectAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true);
        Task<IEnumerable<TResult>> WhereSelectAsNoTrackingAsync<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true);
        
        Task AddAsync(T entity);
        Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> where);
        Task TransactionAsync(Func<Task> action);
        //Task TransactionAsync(Func<IDbContextTransaction, Task> action);

        Task<int> SaveChangesAsync();
    }
}
