using Core.Entities;

namespace Core.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T?> GetEntityWithSpec(ISpecification<T> specification);
    Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> specification);
    Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> specification);
    Task<IReadOnlyList<TResult>> GetAllWithSpec<TResult>(ISpecification<T, TResult> specification);
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<bool> SaveAllAsync();
    bool Exists(int id);
}