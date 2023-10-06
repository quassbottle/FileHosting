using FileHosting.DataAccess.Entities;

namespace FileHosting.DataAccess.Repositories.Interfaces;

public interface IFileMetaRepository : IBaseRepository<DbFileMeta>
{
    Task<DbFileDataMetaJoin> GetFileDataMetaJoin(Guid metaId);
    Task<DbFileNameDataTypeJoin> GetFileNameDataTypeJoin(Guid id);
    Task<List<DbFileMeta>> GetNextOffsetAsync(int offset, int next);
}