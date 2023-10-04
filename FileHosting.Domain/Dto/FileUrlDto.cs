using FileHosting.Domain.Models;

namespace FileHosting.Domain.Dto;

public class FileUrlDto
{
    public Guid Id { get; set; }
    public long SizeInBytes { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
}