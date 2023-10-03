namespace FileHosting.DataAccess.Repositories.Interfaces;

public interface IBaseRepository<T>
{
    public Task<T> UpdateAsync(T t, Guid id);
    public Task<int> DeleteByGuid(Guid id);
    public Task<T> FindByGuidAsync(Guid id);
    public Task<T> CreateAsync(T t);
    public Task<List<T>> GetAllAsync();
}