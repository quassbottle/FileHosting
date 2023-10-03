namespace FileHosting.Domain.Models;

public class FileMeta
{
    public Guid Id { get; set; }
    public long SizeInBytes { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string FileType { get; set; }
    public FileData FileData { get; set; }
}