using Microsoft.EntityFrameworkCore;
namespace Lanchonetes.Infrastructure.Repositories;
public interface IRepository<T> where T : class { IQueryable<T> Query(); Task<T?> GetAsync(Guid id); Task AddAsync(T entity); void Update(T entity); void Remove(T entity); Task SaveChangesAsync(); }
public class Repository<T>(Data.AppDbContext db) : IRepository<T> where T : class { public IQueryable<T> Query() => db.Set<T>(); public Task<T?> GetAsync(Guid id) => db.Set<T>().FindAsync(id).AsTask(); public Task AddAsync(T entity) => db.Set<T>().AddAsync(entity).AsTask(); public void Update(T entity) => db.Set<T>().Update(entity); public void Remove(T entity) => db.Set<T>().Remove(entity); public Task SaveChangesAsync() => db.SaveChangesAsync(); }
