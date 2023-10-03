namespace FileHosting.Domain.Models;

public class FileData
{
    public Guid Id { get; set; }
    public byte[] Data { get; set; }
    public FileMeta FileMeta { get; set; }
}