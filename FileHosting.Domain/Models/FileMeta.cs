namespace FileHosting.Domain.Models;

public class FileMeta
{
    public Guid Id { get; set; }
    public long Size { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public FileData FileData { get; set; }
}