using FileHosting.DataAccess.Attributes;

namespace FileHosting.DataAccess.Entities;

public class DbFileDataMetaJoin : DbFileMeta
{
    [Column("data")]
    public byte[] Data { get; set; }   
    
    [Column("data_id")]
    public Guid DataId { get; set; }
}
