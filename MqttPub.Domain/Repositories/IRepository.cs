using MqttPub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MqttPub.Domain.Repositories
{
    public partial interface IRepository<T> where T : class, IEntity
    {
        bool Any(Expression<Func<T, bool>> where, bool useCache = true);
        T? GetById(int id, bool useCache = true);
        T? FirstOrDefault(Expression<Func<T, bool>> where, bool useCache = true);
        T? FirstOrDefaultAsNoTracking(Expression<Func<T, bool>> where, bool useCache = true);
        IEnumerable<T> Where(Expression<Func<T, bool>> where, bool useCache = true);
        IEnumerable<T> WhereAsNoTracking(Expression<Func<T, bool>> where, bool useCache = true);
        IQueryable<T> WhereAsQueryable(Expression<Func<T, bool>> where);
        IQueryable<T> WhereAsQueryableAndAsNoTracking(Expression<Func<T, bool>> where);
        TResult? SelectFirstOrDefault<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true);
        TResult? SelectFirstOrDefaultAsNoTracking<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true);
        IEnumerable<TResult> WhereSelect<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true);
        IEnumerable<TResult> WhereSelectAsNoTracking<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector, bool useCache = true);

        //EntityEntry<T> Add(T entity);
        //EntityEntry<T> Attach(T entity);
        //EntityEntry<T> Update(T entity);

        //EntityEntry<T> Delete(T entity);
        int ExecuteDelete(Expression<Func<T, bool>> where);

        void Transaction(Action action);
        //void Transaction(Action<IDbContextTransaction> action);

        int SaveChanges();
    }
}