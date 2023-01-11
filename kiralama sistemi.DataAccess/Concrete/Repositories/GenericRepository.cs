using kiralamaSistemi.Entities.Extensions;
using kiralamaSistemi.DataAccess.Extensions;
using kiralamaSistemi.DataAccess.Sevices;
using kiralamaSistemi.Entities.Abstract;
using kiralamaSistemi.Entities.Validations.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace kiralamaSistemi.DataAccess.Concrete.Repositories
{
    public class GenericRepository<T> : Abstract.IRepository<T>
         where T : class
    {
        private readonly AppDbContext _context;
        public GenericRepository()  
        {
            _context= new AppDbContext() ;
        }
        //Get List
        public List<T> GetList(Expression<Func<T, bool>>? filter = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
          int? skip = null, int? take = null)
        {
            try
            {
                //using var context = new AppDbContext();
                IQueryable<T> query = _context.Set<T>().AsQueryable();

                if (include != null)
                {
                    query = include(query);
                }

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                if (skip > 0)
                {
                    query = query.Skip((int)skip);
                }

                if (take > 0)
                {
                    query = query.Take((int)take);
                }
                return query.ToList();

            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(GetList)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public List<TModel> GetList<TModel>(Func<IQueryable<T>, IQueryable<TModel>> select,
         Expression<Func<T, bool>>? filter = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
          int? skip = null, int? take = null)
        {
            try
            {
                //using var context = new AppDbContext();
                IQueryable<T> query = _context.Set<T>().AsQueryable();

                if (include != null)
                {
                    query = include(query);
                }

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (orderBy != null)
                {
                    query = orderBy(query);

                }

                if (skip > 0)
                {
                    query = query.Skip((int)skip);
                }

                if (take > 0)
                {
                    query = query.Take((int)take);
                }

                var model = select(query);
                return model.ToList();

            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(GetList)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>>? filter = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
          int? skip = null, int? take = null)
        {
            try
            {
                //using var context = new AppDbContext();
                IQueryable<T> query = _context.Set<T>().AsQueryable();

                if (include != null)
                {
                    query = include(query);
                }

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (orderBy != null)
                {
                    query = orderBy(query);

                }

                if (skip > 0)
                {
                    query = query.Skip((int)skip);
                }

                if (take > 0)
                {
                    query = query.Take((int)take);
                }

                return await query.ToListAsync();

            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(GetListAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public async Task<List<TModel>> GetListAsync<TModel>(Func<IQueryable<T>, IQueryable<TModel>> select,
          Expression<Func<T, bool>>? filter = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
          int? skip = null, int? take = null)
        {
            try
            {
                //using var context = new AppDbContext();
                IQueryable<T> query = _context.Set<T>().AsQueryable();

                if (include != null)
                {
                    query = include(query);
                }

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                if (skip > 0)
                {
                    query = query.Skip((int)skip);
                }

                if (take > 0)
                {
                    query = query.Take((int)take);
                }

                var model = select(query);
                return await model.ToListAsync();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(GetListAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public async Task<(List<T> data, int count)> GetListAndCountAsync(Expression<Func<T, bool>>? filter = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
          int? skip = null, int? take = null)
        {
            try
            {
              //  using var context = new AppDbContext();
                IQueryable<T> query = _context.Set<T>().AsQueryable();

                if (include != null)
                {
                    query = include(query);
                }

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                int count = await query.CountAsync();

                if (orderBy != null)
                {
                    query = orderBy(query);

                }

                if (skip > 0)
                {
                    query = query.Skip((int)skip);
                }

                if (take > 0)
                {
                    query = query.Take((int)take);
                }

                return (await query.ToListAsync(), count);

            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(GetListAndCountAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public async Task<(List<TModel> data, int count)> GetListAndCountAsync<TModel>(Func<IQueryable<T>, IQueryable<TModel>> select,
          Expression<Func<T, bool>>? filter = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
          int? skip = null, int? take = null)
        {
            try
            {
               // using var context = new AppDbContext();
                IQueryable<T> query = _context.Set<T>().AsQueryable();

                if (include != null)
                {
                    query = include(query);
                }

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                var data = select(query);

                int count = await data.CountAsync();

                if (skip > 0)
                {
                    data = data.Skip((int)skip);
                }

                if (take > 0)
                {
                    data = data.Take((int)take);
                }

                return (await data.ToListAsync(), count);
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(GetListAndCountAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }



        //Get One
        public T? Find(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            try
            {
                //using var context = new AppDbContext();
                IQueryable<T> query = _context.Set<T>().AsQueryable();

                if (include != null)
                {
                    query = include(query);
                }

                return query.FirstOrDefault(filter);
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(Find)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            try
            {
                //using var context = new AppDbContext();
                IQueryable<T> query = _context.Set<T>().AsQueryable();

                if (include != null) query = include(query);

                if(filter == null) return await query.FirstOrDefaultAsync();

                return await query.FirstOrDefaultAsync(filter);
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(FindAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public async Task<TModel?> FindAsync<TModel>(Func<IQueryable<T>, IQueryable<TModel>> select,
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            try
            {
                //using var context = new AppDbContext();
                IQueryable<T> query = _context.Set<T>().AsQueryable();

                if (filter != null) query = query.Where(filter);

                if (include != null) query = include(query);

                return await select(query).FirstOrDefaultAsync();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(FindAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }


        //Filter
        public bool Any(Expression<Func<T, bool>> filter)
        {
            try
            {
                //using var context = new AppDbContext();
                IQueryable<T> query = _context.Set<T>().AsQueryable();
                return query.Any(filter);
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(Any)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                //using var context = new AppDbContext();
                IQueryable<T> query = _context.Set<T>().AsQueryable();
                if (filter != null)
                {
                    return await query.AnyAsync(filter);
                }
                return await query.AnyAsync();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(AnyAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public int Count(Expression<Func<T, bool>> filter)
        {
            try
            {
                //  using var context = new AppDbContext();
                IQueryable<T> query = _context.Set<T>().AsQueryable();
                return query.Count(filter);
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(Count)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                //using var context = new AppDbContext();
                IQueryable<T> query = _context.Set<T>().AsQueryable();
                return await query.CountAsync(filter);
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(CountAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public async Task<Y> CustomAsync<Y>(Func<IQueryable<T>, Task<Y>> custom,
            Expression<Func<T, bool>>? filter = null)
        {
            try
            {
                //using var context = new AppDbContext();
                IQueryable<T> query = _context.Set<T>().AsQueryable();

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                return await custom(query);
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(CustomAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }


        //Create
        public void Create(T entity, Action<AppDbContext>? validation = null)
        {
            Validate(entity);
            try
            {
                //using var context = new AppDbContext();

                validation?.Invoke(_context);

                _context.Set<T>().Add(entity);
                _context.SaveChanges();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(Create)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public virtual async Task CreateAsync(T entity, Func<AppDbContext, Task>? validation = null)
        {
            Validate(entity);
            try
            {
                //using var context = new AppDbContext();

                if (validation != null) await validation.Invoke(_context);

                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(CreateAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }
        //
        public void CreateRange(List<T> entities, Action<AppDbContext>? validation = null)
        {
            var errors = new List<Error>();
            entities.ForEach(entity =>
            {
                try
                {
                    Validate(entity);
                }
                catch (OzelException ex)
                {
                    errors.AddRange(ex.GetListError());
                }
            });
            if (errors.Count > 0) throw new OzelException(errors);

            try
            {
                //using var context = new AppDbContext();
                _context.Set<T>().AddRange(entities);
                _context.SaveChanges();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(CreateRange)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public async Task CreateRangeAsync(List<T> entities, Func<AppDbContext, Task>? validation = null)
        {
            var errors = new List<Error>();
            entities.ForEach(entity =>
            {
                try
                {
                    Validate(entity);
                }
                catch (OzelException ex)
                {
                    errors.AddRange(ex.GetListError());
                }
            }); 
            if (errors.Count > 0) throw new OzelException(errors);

            try
            {
                //using var context = new AppDbContext();
                await _context.Set<T>().AddRangeAsync(entities);
                await _context.SaveChangesAsync();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(CreateRangeAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }
        //Update
        public void Update(T entity, Action<AppDbContext>? validation = null)
        {
            Validate(entity);
            try
            {
                //using var context = new AppDbContext();
                _context.Set<T>().Update(entity);
                _context.SaveChanges();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(Update)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public virtual async Task UpdateAsync(T entity, Func<AppDbContext, Task>? validation = null)
        {
            Validate(entity);
            try
            {
                //using var context = new AppDbContext();

                if (validation != null) await validation.Invoke(_context);

                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(UpdateAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public virtual async Task UpdateAsync(Action<T> action, Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<AppDbContext, T, Task>? validation = null)
        {

            try
            {
                //using var context = new AppDbContext();
                IQueryable<T> query = _context.Set<T>().Where(filter);

                if (include != null)
                {
                    query = include(query);
                }

                var entity = await query.FirstOrDefaultAsync();
                if (entity == null)
                {
                    throw new OzelException(ErrorProvider.NotFoundData);
                }
                action(entity);

                if (validation != null) await validation.Invoke(_context, entity);

                _context.Update(entity);

                await _context.SaveChangesAsync();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {                                                               
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(UpdateAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public void UpdateRange(List<T> entities, Action<AppDbContext>? validation = null)
        {
            var errors = new List<Error>();
            entities.ForEach(entity =>
            {
                try
                {
                    Validate(entity);
                }
                catch (OzelException ex)
                {
                    errors.AddRange(ex.GetListError());
                }
            });
            if (errors.Count > 0) throw new OzelException(errors);

            try
            {
                //using var context = new AppDbContext();
                _context.Set<T>().UpdateRange(entities);
                _context.SaveChanges();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(UpdateRange)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public async Task UpdateRangeAsync(List<T> entities, Func<AppDbContext, Task>? validation = null)
        {
            var errors = new List<Error>();
            entities.ForEach(entity =>
            {
                try
                {
                    Validate(entity);
                }
                catch (OzelException ex)
                {
                    errors.AddRange(ex.GetListError());
                }
            });
            if (errors.Count > 0) throw new OzelException(errors);

            try
            {
                //using var context = new AppDbContext();
                _context.Set<T>().UpdateRange(entities);
                await _context.SaveChangesAsync();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(UpdateRangeAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public async Task UpdateRangeAsync(Action<T> action, Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<AppDbContext, T, Task>? validation = null)
        {
            try
            {
                //using var context = new AppDbContext();
                IQueryable<T> query = _context.Set<T>().Where(filter);

                if (include != null)
                {
                    query = include(query);
                }

                var entities = await query.ToListAsync();

                entities.ForEach(entity =>
                {
                    action(entity);
                    _context.Update(entity);
                });
                await _context.SaveChangesAsync();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(UpdateRangeAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }


        //Delete
        public void Delete(T entity)
        {
            try
            {
                //using var context = new AppDbContext();
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(Delete)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public virtual async Task DeleteAsync(T entity)
        {
            try
            {
                //using var context = new AppDbContext();
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(DeleteAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                //using var context = new AppDbContext();
                var entity = await _context.Set<T>().FirstOrDefaultAsync(filter);
                if (entity == null)
                {
                    throw new OzelException(ErrorProvider.NotFoundData);
                }
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(DeleteAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public async Task DeleteAsync<T>(Expression<Func<T, bool>> filter, LogInfo logInfo)
            where T : BaseEntity
        {
            try
            {
                //using var context = new AppDbContext();
                var entity = await _context.Set<T>().FirstOrDefaultAsync(filter)
                    ?? throw new OzelException(ErrorProvider.NotFoundData);

                entity.LogInfo = logInfo;
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(DeleteAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public void DeleteRange(List<T> entities)
        {
            try
            {
                //using var context = new AppDbContext();
                _context.RemoveRange(entities);
                _context.SaveChanges();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(DeleteRange)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public async Task DeleteRangeAsync(List<T> entities)
        {
            try
            {
                //using var context = new AppDbContext();
                _context.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(DeleteRangeAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public async Task DeleteRangeAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                //using var context = new AppDbContext();
                var entities = _context.Set<T>().Where(filter);
                if (entities?.Any() ?? false)
                {
                    _context.Set<T>().RemoveRange(entities);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new OzelException(ErrorProvider.NotFoundData);
                }
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(DeleteRangeAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }
        public async Task DeleteRangeAsync<T>(Expression<Func<T, bool>> filter, LogInfo logInfo) where T : BaseEntity
        {
            try
            {
                //using var context = new AppDbContext();
                var entities = await _context.Set<T>().Where(filter).ToListAsync();
                if (entities?.Any() ?? false)
                {
                    entities.ForEach(i => i.LogInfo = logInfo);
                    _context.Set<T>().RemoveRange(entities);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new OzelException(ErrorProvider.NotFoundData);
                }
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(GenericRepository<T>)} - {nameof(DeleteRangeAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }


        //Validate
        public void Validate<T>(T entity) where T : class
        {
            entity.RemoveObjectWhiteSpaces();
            var type = entity.GetValidationType();
            if (type == null)
            {
                return;
            }
            if (type.BaseType == typeof(AbstractValidatorBase<T>))
            {
                var validator = Activator.CreateInstance(type) as AbstractValidatorBase<T>;
                var validation = validator?.Validate(entity);
                if (!(validation?.IsValid ?? true))
                {
                    throw new OzelException(validation.GetListError());
                }
            }
            else
            {
                throw new Exception($"Hatalı validation attribute {entity.GetType().Name}");
            }
        }
    }
}
