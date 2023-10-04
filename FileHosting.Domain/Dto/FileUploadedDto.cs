namespace FileHosting.Domain.Dto;

public class FileUploadedDto
{
    public long SizeInBytes { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string FileType { get; set; }
    public bool IsUploaded { get; set; }
}