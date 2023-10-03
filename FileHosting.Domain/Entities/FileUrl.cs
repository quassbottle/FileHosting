namespace FileHosting.Domain.Entities;

public class FileUrl
{
    public Guid Guid { get; set; }
    public FileMeta File { get; set; }
}