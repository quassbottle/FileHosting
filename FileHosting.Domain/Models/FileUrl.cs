namespace FileHosting.Domain.Models;

public class FileUrl
{
    public Guid Id { get; set; }
    public FileUploadedDto FileUploadedDto { get; set; }
}