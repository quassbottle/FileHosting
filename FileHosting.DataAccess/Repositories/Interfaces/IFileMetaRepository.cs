﻿using FileHosting.DataAccess.Entities;

namespace FileHosting.DataAccess.Repositories.Interfaces;

public interface IFileMetaRepository : IBaseRepository<DbFileMeta>
{
    Task<DbFileDataMetaJoin> GetFileDataAndMetaJoinById(Guid metaId);
    Task<DbFileNameDataJoin> GetFileNameDataJoinById(Guid id);
}