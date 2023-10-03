namespace FileHosting.Domain.Models;

public class FileUrl
{
    public Guid Id { get; set; }
    public FileMeta FileMeta { get; set; }
}