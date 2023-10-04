namespace FileHosting.Domain.Models;

public class FileUploadedDto
{
    public Guid Id { get; set; }
    public long SizeInBytes { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string FileType { get; set; }
    public byte[] Data { get; set; }   
    public Guid DataId { get; set; }
}