namespace ReadingCommunityApi.Core.Interfaces;

public interface IBaseRepository<T> where T : class 
{
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}