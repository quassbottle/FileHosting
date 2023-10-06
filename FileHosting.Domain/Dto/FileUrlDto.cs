using FileHosting.Domain.Models;

namespace FileHosting.Domain.Dto;

public class FileUrlDto
{
    public Guid UrlId { get; set; }
    public string Name { get; set; }
}