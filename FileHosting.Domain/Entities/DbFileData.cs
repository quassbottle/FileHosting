namespace FileHosting.Domain.Entities;

public class DbFileData
{
    public Guid Guid { get; set; }
    public byte[] Data { get; set; }
    public Guid FileMetaId { get; set; }
}