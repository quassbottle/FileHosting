namespace FileHosting.DataAccess.Entities;

public class DbFileData
{
    public Guid Id { get; set; }
    public byte[] Data { get; set; }
    public Guid FileMetaId { get; set; }
}