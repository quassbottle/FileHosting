using FileHosting.DataAccess.Entities;

namespace FileHosting.DataAccess.Repositories.Interfaces;

public interface IFileUrlRepository : IBaseRepository<DbFileUrl>
{
    Task<DbFileNameDataTypeJoin> GetFileNameDataTypeJoin(Guid urlId);
}