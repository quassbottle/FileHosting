namespace FileHosting.DataAccess.Entities;

public class DbFileDataMetaJoin : DbFileMeta
{
    public byte[] Data { get; set; }   
    public Guid DataId { get; set; }
}
