using kiralamaSistemi.Entities.Abstract;
using kiralamaSistemi.DataAccess.Concrete;
using kiralamaSistemi.Entities.Abstract;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace kiralamaSistemi.DataAccess.Abstract
{
    public interface IRepository<T>
     where T : class
    {
        //Get List
        List<T> GetList(Expression<Func<T, bool>>? filter = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
          int? skip = null, int? take = null);

        List<TModel> GetList<TModel>(Func<IQueryable<T>, IQueryable<TModel>> select,
          Expression<Func<T, bool>>? filter = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
          int? skip = null, int? take = null);

        Task<List<T>> GetListAsync(Expression<Func<T, bool>>? filter = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
          int? skip = null, int? take = null);

        Task<List<TModel>> GetListAsync<TModel>(Func<IQueryable<T>, IQueryable<TModel>> select,
          Expression<Func<T, bool>>? filter = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
          int? skip = null, int? take = null);

        Task<(List<T> data, int count)> GetListAndCountAsync(Expression<Func<T, bool>>? filter = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
          int? skip = null, int? take = null);

        Task<(List<TModel> data, int count)> GetListAndCountAsync<TModel>(Func<IQueryable<T>, IQueryable<TModel>> select,
          Expression<Func<T, bool>>? filter = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
          int? skip = null, int? take = null);


        //Get One
        T? Find(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<T?> FindAsync(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<TModel?> FindAsync<TModel>(Func<IQueryable<T>, IQueryable<TModel>> select,
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);


        //Filter
        bool Any(Expression<Func<T, bool>> filter);

        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);

        int Count(Expression<Func<T, bool>> filter);

        Task<int> CountAsync(Expression<Func<T, bool>> filter);
        Task<Y> CustomAsync<Y>(Func<IQueryable<T>, Task<Y>> custom, Expression<Func<T, bool>>? filter = null);


        //Create
        void Create(T entity, Action<AppDbContext>? validation = null);

        Task CreateAsync(T entity, Func<AppDbContext, Task>? validation = null);

        Task CreateRangeAsync(List<T> entity, Func<AppDbContext, Task>? validation = null);

        void CreateRange(List<T> entity, Action<AppDbContext>? validation = null);


        //Update
        void Update(T entity, Action<AppDbContext>? validation = null);

        Task UpdateAsync(T entity, Func<AppDbContext, Task>? validation = null);

        Task UpdateAsync(Action<T> action, Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<AppDbContext, T, Task>? validation = null);

        void UpdateRange(List<T> entities, Action<AppDbContext>? validation = null);

        Task UpdateRangeAsync(List<T> entities, Func<AppDbContext, Task>? validation = null);

        Task UpdateRangeAsync(Action<T> action,
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<AppDbContext, T, Task>? validation = null);

        //Delete
        void Delete(T entity);

        Task DeleteAsync(T entity);

        Task DeleteAsync(Expression<Func<T, bool>> filter);

        Task DeleteAsync<T>(Expression<Func<T, bool>> filter, LogInfo logInfo) where T : BaseEntity;

        void DeleteRange(List<T> entities);

        Task DeleteRangeAsync(List<T> entities);
        Task DeleteRangeAsync(Expression<Func<T, bool>> filter);
        Task DeleteRangeAsync<T>(Expression<Func<T, bool>> filter, LogInfo logInfo) where T : BaseEntity;

        public void Validate<T>(T entity)
            where T : class;
    }
}
