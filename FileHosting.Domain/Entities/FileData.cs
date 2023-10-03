namespace FileHosting.Domain.Entities;

public class FileData
{
    public Guid Guid { get; set; }
    public byte[] Data { get; set; }
    public FileMeta meta;
}